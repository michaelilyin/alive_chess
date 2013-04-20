using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;

namespace AliveChessServer.LogicLayer.AI.MonitorLayer
{
    public class AStar
    {
        private sbyte[,] direction;
        private int mHEstimate = 2;
        private AStarPriorityQueue mOpen;
        private List<AStarNode> mClose;
        private ILocalizable map;
        private int mSearchLimit = 2000; // максимальное количество вершин которые король может пройти

        private GameData context;

        public AStar(ILocalizable map, GameData context)
        {
            this.map = map;
            this.context = context;

            direction = direction = new sbyte[8, 2] { { 0, -1 }, { 1, 0 }, 
            { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } }; // направления по которым разрешено перемещатся

            mOpen = new AStarPriorityQueue(new AStarNodeComparer()); // открытый список
            mClose = new List<AStarNode>(); // закрытый список
        }

        /// <summary>
        /// поиск кратчайшего пути
        /// </summary>
        /// <returns>возвращаем путь</returns>
        public List<Position> FindPath(Position start, Position goal)
        {
            List<Position> path = null;
            bool isFound = false;

            mOpen.Clear();
            mClose.Clear();

            AStarNode parentNode = new AStarNode();

            parentNode.G = 0;
            parentNode.H = mHEstimate;
            parentNode.F = parentNode.H + parentNode.G;
            parentNode.X = start.X;
            parentNode.Y = start.Y;
            parentNode.PX = parentNode.X;
            parentNode.PY = parentNode.Y;

            mOpen.Push(parentNode);

            while (mOpen.Count > 0)
            {
                parentNode = mOpen.Pop();

                // нашли конечеую точку
                if (parentNode.X == goal.X && parentNode.Y == goal.Y)
                {
                    mClose.Add(parentNode);
                    isFound = true;
                    break;
                }

                // объект не проходим
                if (map.GetWayCost(parentNode.X, parentNode.Y) > context.GetImpassableAreaWayCost)
                    continue;

                // конечная точка слишком далеко
                if (mClose.Count > mSearchLimit)
                    return null;

                // ищем соседей
                for (int i = 0; i < 8; i++)
                {
                    AStarNode newNode = new AStarNode();
                    newNode.X = parentNode.X + direction[i, 0];
                    newNode.Y = parentNode.Y + direction[i, 1];

                    // вершина вышла за границы карты
                    if (newNode.X < 0 || newNode.Y < 0 || newNode.X >= map.SizeX ||
                        newNode.Y >= map.SizeY)
                        continue;

                    // запрещаем ходить по воде и другим непроходимым участкам
                    if (map.GetWayCost(newNode.X, newNode.Y) > context.GetImpassableAreaWayCost)
                    {
                        mClose.Add(newNode);
                        continue;
                    }

                    float newG; // расчитываем стоимость прохождения
                    newG = parentNode.G + map.GetWayCost(newNode.X, newNode.Y);

                    //if (newG == parentNode.G)
                    //    continue;

                    int foundInOpenIndex = -1;
                    for (int j = 0; j < mOpen.Count; j++)
                    {
                        if (mOpen[j].X == newNode.X && mOpen[j].Y == newNode.Y)
                        {
                            foundInOpenIndex = j;
                            break;
                        }
                    }
                    if (foundInOpenIndex != -1 && mOpen[foundInOpenIndex].G <= newG)
                        continue;

                    int foundInCloseIndex = -1;
                    for (int j = 0; j < mClose.Count; j++)
                    {
                        if (mClose[j].X == newNode.X && mClose[j].Y == newNode.Y)
                        {
                            foundInCloseIndex = j;
                            break;
                        }
                    }
                    if (foundInCloseIndex != -1 && mClose[foundInCloseIndex].G <= newG)
                        continue;

                    newNode.PX = parentNode.X;
                    newNode.PY = parentNode.Y;
                    newNode.G = newG;

                    // стоимость H считаем по формуле Манхэттена
                    newNode.H = mHEstimate * (Math.Abs(newNode.X - goal.X) +
                        Math.Abs(newNode.Y - goal.Y));

                    // расчитываем полную стоимость прохождения через вершину
                    newNode.F = newNode.G + newNode.H;

                    mOpen.Push(newNode); // ложим вершину в открытый список
                }

                mClose.Add(parentNode);
            }

            // конечная вершина найдена
            if (isFound)
            {
                // возвращаем путь
                AStarNode fNode = mClose[mClose.Count - 1];
                for (int i = mClose.Count - 1; i >= 0; i--)
                {
                    if (fNode.PX == mClose[i].X && fNode.PY == mClose[i].Y || i == mClose.Count - 1)
                        fNode = mClose[i];
                    else
                        mClose.RemoveAt(i);
                }

                path = new List<Position>();

                foreach (AStarNode n in mClose)
                {
                    Position s = new Position();
                    s.X = n.X;
                    s.Y = n.Y;
                    path.Add(s);
                }

                return path;
            }
            return null;
        }
    }
}
