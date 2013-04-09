/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package world;

import Logic.Location;
import java.util.Enumeration;
import java.util.Vector;
import obj.BaseObject;

/**
 * Класс поиска кратчайшего пути на глобальной карте.
 * Используя FieldLoader, принадлежащий переданному WorldContext, и размеры карты,
 * работает со всеми ее секторами
 * @author е
 */
public class WorldPathFinder {

    WorldContext World;

    public WorldPathFinder(WorldContext context) {
        World = context;
    }

    public Cell getCell(int x, int y){
        if (CheckBoundsFails(x, y))
            return null;
        int sectorX = x / FieldSector.SECTOR_WIDTH;
        int sectorY = y / FieldSector.SECTOR_HEIGHT;
        return World.getFieldLoader().getStrategyFieldSector(new Location(sectorX, sectorY))
                .getCell(x % FieldSector.SECTOR_WIDTH, y % FieldSector.SECTOR_HEIGHT);
    }

    public Cell getCell(Location loc) {
        return getCell(loc.getX(), loc.getY());
    }
    
    protected boolean CheckBoundsFails(int x, int y) {
        return (x < 0 || y < 0 || x >= World.getSizeX() * FieldSector.SECTOR_WIDTH
                || y >= World.getSizeY() * FieldSector.SECTOR_HEIGHT);
    }

    public Vector findGlobalShortestPath(int StartsX, int StartsY, int EndsX, int EndsY){
        ClearDistances();
        ClearVisits();
        //System.out.println("Distances and visits cleared.");
        Cell CurCell;
        Location CurLoc, buf;
        int qstep;
        Vector queue = new Vector();//type - Location
        Vector neighbours = new Vector(); //type - Location
        //SEARCH
        CurLoc = new Location(StartsX, StartsY);
        //если некорректные входные данные
        if (!checkCell(CurLoc) || !checkCell(new Location(EndsX , EndsY)))
            return null;
        if ( StartsX == EndsX && StartsY == EndsY)
            return queue;
        
        getCell(CurLoc).setDistance(0);
        queue.addElement(CurLoc);
        qstep = 0; //номер текущего узла в очереди обработки

        System.out.println("Initialization complete.");
        //обновление расстояний до соседей
        while (queue.size() != 0) {
            //System.out.println("iteration: " + qstep);
            CurLoc = (Location)queue.elementAt(0);
            CurCell = getCell(CurLoc);
            //if (CurCell.getDistance() > 50) return null;//ограничение глубины просчета
            if (new Location(EndsX , EndsY).equals(CurLoc)) break;
            if (!CurCell.isVisited()) {
                //получаем соседей (не включая посещенных или препятствия)
                neighbours = getCheckedNeighbours(CurLoc);
                //вычисляем и переопределяем кратчайшие пути до соседей
                for (int i = 0; i < neighbours.size(); i++) {
                        buf = (Location)neighbours.elementAt(i);
                        Field.setDistance(CurCell, getCell(buf));
                        //помещаем соседей в очередь обхода
                        queue.addElement(buf);
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
        CurLoc = new Location(EndsX, EndsY);
        Vector res = new Vector();
        res.addElement(CurLoc);
        qstep = 0;
        //int i = 0;
        while (getCell(CurLoc).getDistance() > 0) {
            CurLoc = (Location)getMinDistanceNeighbour(CurLoc);
            res.addElement(CurLoc);
        }
        res.removeElementAt(res.size() - 1);
        System.out.println("Traceback passed.");
        return res;
    }

    /**
     * поиск соседа ячейки с, имеющего мин.расстояние до цели(Distance)
     * среди соседей ячейки с
     */
    protected Location getMinDistanceNeighbour(Location loc) {
        Vector neighbours = getCheckedNeighbours(loc);
        //Cell res = this.getCell(loc);
        Location buf = null;
        int min = getCell(loc).getDistance();
        for (int i = 0; i < neighbours.size(); i++) {
            buf = (Location)neighbours.elementAt(i);
            Cell c = getCell(buf);
            if (c.getDistance() + c.getTerrainType() == min)
                return buf;
        }
        return loc;
    }

    protected Vector getCheckedNeighbours(Location loc){
        Vector res = new Vector();

        int x = loc.getX(), y = loc.getY();
        addCellLoc(res, x, y - 1);
        addCellLoc(res, x + 1, y);
        addCellLoc(res, x, y + 1);
        addCellLoc(res, x - 1, y);

        addCellLoc(res, x + 1, y - 1);
        addCellLoc(res, x + 1, y + 1);
        addCellLoc(res, x - 1, y + 1);
        addCellLoc(res, x - 1, y - 1);

        //System.out.println("getCheckedNeighbours ends: " + res.size());
        return res;
    }

    private void ClearDistances() {
        for (Enumeration e = World.getFieldLoader().FieldSectors.elements(); e.hasMoreElements(); ) {
            ((FieldSector)e.nextElement()).ClearDistances();
        }
    }

    private void ClearVisits() {
        for (Enumeration e = World.getFieldLoader().FieldSectors.elements(); e.hasMoreElements(); ) {
            ((FieldSector)e.nextElement()).ClearVisits();
        }
    }

    /**
     * Вставить ячейку в массив в порядке возрастания TerrainType
     * @param vec
     * @param x
     * @param y
     */
    private void addCellLoc(Vector vec, int x, int y){
        Location loc = new Location(x, y);
        Cell buf = getCell(loc);
        if (checkCell(loc)) {
            for (int i = 0; i < vec.size(); i++)
                if (buf.getTerrainType() < getCell((Location)vec.elementAt(i)).getTerrainType()) {
                     vec.insertElementAt(loc, i);
                     return;
                }
            vec.addElement(loc);
        }
    }

    protected boolean checkCell(Location loc) {
        if (!Field.checkCell(getCell(loc))) return false;
        for(int i = 0; i < World.getLayeredObjects().size(); i++) {
            BaseObject fobj = (BaseObject)World.getLayeredObjects().elementAt(i);
            if (fobj.getRefX() == loc.getX() && fobj.getRefY() == loc.getY()) return true;
            if (BaseObject.checkLocationWithin(fobj, loc)) return false;
        }
        return true;
    }
}
