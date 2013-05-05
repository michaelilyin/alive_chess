using System;
using System.Collections.Generic;
using System.Drawing;
using AliveChessLibrary;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;

namespace BehaviorAILibrary.MotionLayer
{
    public class AStar
    {
        private Position start;
        private Position goal;
        private sbyte[,] direction;
        private int mHEstimate = 2;
        private AStarPriorityQueue mOpen;
        private List<AStarNode> mClose;
        private ILocalizable map;
        private int mSearchLimit = 20000; // максимальное количество вершин которые король может пройти
        private bool mDiagonal = true;
        private bool mHeavyDiagonals = false;
        private const float DiagonalCost = 2.41f;

        public AStar(ILocalizable map)
        {
            this.map = map;

            if (mDiagonal)
                direction = new sbyte[8, 2] { { 0, -1 }, { 1, 0 }, 
            { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } }; // направления по которым разрешено перемещатся
            else
                direction = new sbyte[4,2] {{0, -1}, {1, 0}, {0, 1}, {-1, 0}};

            mOpen = new AStarPriorityQueue(new AStarNodeComparer()); // открытый список
            mClose = new List<AStarNode>(); // закрытый список
        }

        public void SetStartEnd(Position start, Position goal)
        {
            this.start = start;
            this.goal = goal;
        }

        public void SetStartEnd(Point start, Point goal)
        {
            SetStartEnd(new Position(start.X, start.Y), new Position(goal.X, goal.Y));
        }

        /// <summary>
        /// поиск кратчайшего пути
        /// </summary>
        /// <returns>возвращаем путь</returns>
        public List<Position> FindPath()
        {
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

            // помещаем в открытый список начальную вершину
            mOpen.Push(parentNode);
            // пока открытый список не пуст
            while (mOpen.Count > 0)
            {
                // берем из открытого списка вершину
                parentNode = mOpen.Pop();

                // нашли конечеую точку
                if (parentNode.X == goal.X && parentNode.Y == goal.Y)
                {
                    // добавляем ее в закрытый список
                    mClose.Add(parentNode);
                    isFound = true;
                    break;
                }

                // объект не проходим
                if (map.GetWayCost(parentNode.X, parentNode.Y) > Constants.ImpassablePoint)
                    continue;

                // конечная точка слишком далеко
                if (mClose.Count > mSearchLimit)
                    return null;

                // ищем соседей
                for (int i = 0; i < (mDiagonal ? 8 : 4); i++)
                {
                    AStarNode newNode = new AStarNode();
                    newNode.X = parentNode.X + direction[i, 0];
                    newNode.Y = parentNode.Y + direction[i, 1];

                    // вершина вышла за границы карты
                    if (newNode.X < 0 || newNode.Y < 0 || newNode.X >= map.SizeX ||
                        newNode.Y >= map.SizeY)
                        continue;

                    // запрещаем ходить по воде и другим непроходимым участкам
                    if (map.GetWayCost(newNode.X, newNode.Y) > Constants.ImpassablePoint)
                    {
                        // помещаем о закрытый список
                        mClose.Add(newNode);
                        continue;
                    }

                    float newG; // расчитываем стоимость прохождения
                    if (mHeavyDiagonals && i > 3)
                        newG = parentNode.G + map.GetWayCost(newNode.X, newNode.Y)*DiagonalCost;
                    else
                        newG = parentNode.G + map.GetWayCost(newNode.X, newNode.Y);

                    //if (newG == parentNode.G)
                    //    continue;

                    //ищем узел в открытом списке
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

                    // ищем узел в закрытом списке
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

                    //инициализируем новый узел
                    newNode.PX = parentNode.X;
                    newNode.PY = parentNode.Y;
                    newNode.G = newG;

                    // стоимость H считаем по формуле Манхэттена
                    newNode.H = mHEstimate*(Math.Abs(newNode.X - goal.X) +
                                            Math.Abs(newNode.Y - goal.Y));

                    // расчитываем полную стоимость прохождения через вершину
                    newNode.F = newNode.G + newNode.H;
                    // ложим вершину в открытый список
                    mOpen.Push(newNode);
                }

                //ложим родительскую вершину в закрытый список
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

                List<Position> path = new List<Position>();
                foreach (AStarNode n in mClose)
                    path.Add(new Position(n.X, n.Y));

                return path;
            }
            else throw new AliveChessException("Path not found");
        }

        public bool Dialonals
        {
            get { return mDiagonal; } 
            set { mDiagonal = value; }
        }

        public bool HeavyDiagonals
        {
            get { return mHeavyDiagonals; }
            set { mHeavyDiagonals = value; }
        }
    }
}
