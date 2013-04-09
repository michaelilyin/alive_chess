/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package world;

import Logic.Location;
import java.util.Vector;
/**
 *
 * @author е
 */
//добавить тип почвы - т.е. затраты на перемещение из ячейки
public class Field {

    protected Cell[][] Cells;

    public Field() {
        Cells = new Cell[1][];
        Cells[0] = new Cell[1];
        Cells[0][0] = new Cell();
    }

    public Field(int Xsize, int Ysize){
        Cells = new Cell[Xsize][];
        for(int i = 0; i < Xsize; i++){
            Cells[i] = new Cell[Ysize];
            for(int j = 0; j < Ysize; j++)
                Cells[i][j] = new Cell();
        }
    }

    //если эл-т массива не равен 0, то ячейка помечается как препятствие
    public Field(int[][] tiles){
        if (tiles == null) return;
        Cells = new Cell[tiles.length][];
        for(int i = 0; i < tiles.length; i++){
            Cells[i] = new Cell[tiles[0].length];
            for(int j = 0; j < tiles[0].length; j++) {
                Cells[i][j] = new Cell(tiles[i][j]);
            }
        }
    }

    public Field(Field f) {
        if (f == null) return;
        Cells = new Cell[f.getWidth()][];
        for(int i = 0; i < f.getWidth(); i++){
            Cells[i] = new Cell[f.getHeight()];
            for(int j = 0; j < f.getHeight(); j++) {
                Cells[i][j] = new Cell(f.getCell(i, j));
            }
        }
    }

    public void print() {
        for(int j = 0; j < getHeight(); j++){
            for(int i = 0; i < getWidth(); i++){
                System.out.print("[" + getCell(i,j).getTerrain()+"]");
            }
            System.out.println();
        }
    }

    public Cell getCell(int x, int y){
        if (CheckBounds(x, y))
            return null;
        return Cells[x][y];
    }

    public void setCell(int x, int y, Cell cell){
        if (CheckBounds(x, y))
            return;
        if (cell == null) return;
        Cells[x][y] = cell;
    }

    private boolean CheckBounds(int x, int y) {
        return (x < 0 || y < 0 || x >= getWidth() || y >= getHeight());
    }

    private Location getLocation(Cell cell){
        for(int i = 0; i < getWidth(); i++){
            for(int j = 0; j < getHeight(); j++){
                if (Cells[i][j] == cell)
                    return new Location(i, j);
            }
        }
        return null;
    }

    public int getWidth() {
        return Cells.length;
    }

    public int getHeight() {
        return Cells[0].length;
    }

    public void ClearDistances(){
        for(int i = 0; i < getWidth(); i++){
            for(int j = 0; j < getHeight(); j++){
                Cells[i][j].setDistance(Integer.MAX_VALUE);
            }
        }
    }

    public void ClearVisits(){
        for(int i = 0; i < getWidth(); i++){
            for(int j = 0; j < getHeight(); j++){
                Cells[i][j].setVisited(false);
            }
        }
    }

    /**
     * Поиск кратчайшего пути алгоритмом Дейкстры
     * @deprecated
     */
    public Vector findShortestPath(int StartsX, int StartsY, int EndsX, int EndsY){
        ClearDistances();
        ClearVisits();
        System.out.println("Distances and visits cleared.");
        Cell CurCell, buf;
        Vector queue = new Vector();//значение длины  - временная заглушка. исправить.
        int qstep;
        Vector neighbours = new Vector();
        //SEARCH
        //если некорректные входные данные
        if (getCell(StartsX , StartsY).isBarrier() || getCell(EndsX , EndsY).isBarrier())
            return null;
        if ( StartsX == EndsX && StartsY == EndsY)
            return queue;
        CurCell = getCell(StartsX , StartsY);
        CurCell.setDistance(0);
        queue.addElement(CurCell);
        qstep = 0; //номер текущего узла в очереди обработки
        //qsize = 1;

        System.out.println("Initialization complete.");
        //обновление расстояний до соседей
        //порядок: up, right, down, left
        while (queue.size() != 0) {
            //System.out.println("iteration: " + qstep);
            CurCell = (Cell)queue.elementAt(0);
            //if (CurCell.getDistance() > 50) return null;//ограничение глубины просчета
            if (getCell(EndsX , EndsY) == CurCell) break;
            if (!CurCell.isVisited()) {
                //получаем соседей (не включая посещенных или препятствия)
                neighbours = getCheckedNeighbours(CurCell);
                //вычисляем и переопределяем кратчайшие пути до соседей
                for (int i = 0; i < neighbours.size(); i++) {
                        buf = (Cell)neighbours.elementAt(i);
                        setDistance(CurCell, buf);
                        //помещаем соседей в очередь обхода
                        queue.addElement(buf);
                        //qsize++;
                    }
                //вычеркиваем текущую вершину
                CurCell.setVisited(true);                
            }

            //если очередь закончилась(т.е. закончились узлы), выходим из цикла
            //иначе переходим на следующую итерацию
            queue.removeElementAt(0);
            qstep++;
            //System.out.println("queue.size: " + queue.size());
        }

        System.out.println("Shortest path found: " + getCell(EndsX, EndsY).getDistance());
        //TRACEBACK: ищем путь назад по длинам путей до каждой из вершин.
        ClearVisits();
        //начинаем с конца
        CurCell = getCell(EndsX, EndsY);
        Vector res = new Vector();
        res.addElement(getLocation(CurCell));
        qstep = 0;
        //int i = 0;
        while (CurCell.getDistance() > 0) {
            CurCell = getMinDistanceNeighbour(CurCell);
            res.addElement(getLocation(CurCell));
        }
        res.removeElementAt(res.size() - 1);
        System.out.println("Traceback passed.");
        return res;
    }

    /**
     * поиск соседа ячейки с, имеющего мин.расстояние до цели(Distance)
     * среди соседей ячейки с
     * @deprecated
     */
    private Cell getMinDistanceNeighbour(Cell c) {
        Vector neighbours = getCheckedNeighbours(c);
        //Cell res = c;
        Cell buf = null;
        int min = c.getDistance();
        for (int i = 0; i < neighbours.size(); i++) {
            buf = (Cell)neighbours.elementAt(i);
            if (buf.getDistance() + buf.getTerrainType() == min)
                return buf;
        }
        return c;
    }

    /**
     * Получить соседей ячейки
     * @param c
     * @return
     * @deprecated
     */
    private Vector getCheckedNeighbours(Cell c){
        Vector res = new Vector();
        Location CurLoc = this.getLocation(c);

        addCell(res, CurLoc.getX(), CurLoc.getY() - 1);
        addCell(res, CurLoc.getX() + 1, CurLoc.getY());
        addCell(res, CurLoc.getX(), CurLoc.getY() + 1);
        addCell(res, CurLoc.getX() - 1, CurLoc.getY());

        addCell(res, CurLoc.getX() + 1, CurLoc.getY() - 1);
        addCell(res, CurLoc.getX() + 1, CurLoc.getY() + 1);
        addCell(res, CurLoc.getX() - 1, CurLoc.getY() + 1);
        addCell(res, CurLoc.getX() - 1, CurLoc.getY() - 1);

        //System.out.println("getCheckedNeighbours ends: " + res.size());
        return res;
    }

    /**
     * Добавить ячейку с координатами в вектор
     * @param vec
     * @param x
     * @param y
     * @deprecated
     */
    private void addCell(Vector vec, int x, int y){
        Cell buf = this.getCell(x, y);
        if (checkCell(buf)) {
            for (int i = 0; i < vec.size(); i++)
                if ( buf.getTerrainType() < ((Cell)vec.elementAt(i)).getTerrainType()) {
                     vec.insertElementAt(buf, i);
                     return;
                }
            vec.addElement(buf);
        }
    }

    public static boolean checkCell(Cell toCheck) {
        return toCheck != null && !toCheck.isBarrier() && !toCheck.isVisited();
    }

    /**
     * Использовать только в Дейкстре для соседних клеток! надо бы сделать проверку этого.
     * @param main
     * @param toCheck
     */
    public static void setDistance(Cell main, Cell toCheck){
        if (checkCell(toCheck))
            if (toCheck.getDistance() > main.getDistance() + main.getTerrainType())
                toCheck.setDistance(main.getDistance() + main.getTerrainType());
    }
}
