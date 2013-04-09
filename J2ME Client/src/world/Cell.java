/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package world;

/**
 *
 * @author е
 * Ячейка, предназначенная для использования в алгоритме поиска пути.
 * Внимание! Численные значения для определения типа местности и препятствия
 * жестко зашиты в класс и основываются на порядке расположения изображений
 * в файле field.png
 */
public class Cell {

    public static final int T_ROAD  = 1; //автострада :-)
    public static final int T_GRASS = 2; //обычная местность
    public static final int T_SWAMP = 6; //болото
    public static final int T_WATER = 20; //переправа, переправа, берег левый, берег правый...
    public static final int T_UNKNOWN = Integer.MAX_VALUE;//неопределен

    private int     Distance; //расстояние до ячейки. используется при просчете кратчайшего пути
    private boolean Visited;  //используется при просчете кратчайшего пути
    private int     Terrain;  //местность. совпадает с номером tile в tiledlayer.
                              //делится на типы по проходимости

    public Cell(){
        this(0);
    }

    public Cell(int terrain)  {
        Distance = Integer.MAX_VALUE;
        Visited = false;
        Terrain = terrain;
    }

    public Cell(Cell c) {
        this.Terrain = c.getTerrain();
        this.Visited = c.isVisited();
        this.Distance = c.getDistance();
    }

    /**
     * @return the Distance
     */
    public int getDistance() {
        return Distance;
    }

    /**
     * @param Distance the Distance to set
     */
    public void setDistance(int Distance) {
        this.Distance = Distance;
    }

    /**
     * @return the Visited
     */
    public boolean isVisited() {
        return Visited;
    }

    /**
     * @param Visited the Visited to set
     */
    public void setVisited(boolean Visited) {
        this.Visited = Visited;
    }

    /**
     * тип местности(проходимость)
     * метод необходимо держать в соответствии с файлом, из которого загружаются изображения
     * @return the TerrainType
     */
    public static int getTerrainType(int Terrain) {
        if (Terrain == 0)
            return Cell.T_GRASS;
        if ((Terrain >= 1 && Terrain <= 25) || Terrain == 78)
            return Cell.T_ROAD;
        if (Terrain >= 26 && Terrain <= 27)
            return Cell.T_SWAMP;
        return Cell.T_UNKNOWN;
    }

    public int getTerrainType() {
        return Cell.getTerrainType(this.Terrain);
    }

    public boolean isBarrier() {
        return Cell.getTerrainType(this.Terrain) == T_UNKNOWN;
    }

    /**
     * @return the Terrain
     * номер tile из png
     */
    public int getTerrain() {
        return Terrain;
    }

    /**
     * @param Terrain the Terrain to set
     */
    public void setTerrain(int Terrain) {
        this.Terrain = Terrain;
    }
}
