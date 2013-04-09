/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package world;

import Logic.Location;

/**
 *
 * @author е
 */
public class FieldSector extends Field {

    public static final int SECTOR_WIDTH = 16;
    public static final int SECTOR_HEIGHT = 16;

    private Location GlobalCoordinates;

    public FieldSector() {
        super(SECTOR_WIDTH, SECTOR_HEIGHT);
        GlobalCoordinates = new Location();
    }

    public FieldSector(int[][] tiles) {
        if (tiles.length < SECTOR_WIDTH || tiles[0].length < SECTOR_HEIGHT) return;
        //if (tiles == null) return;
        Cells = new Cell[SECTOR_WIDTH][];
        for(int i = 0; i < SECTOR_WIDTH; i++){
            Cells[i] = new Cell[SECTOR_HEIGHT];
            for(int j = 0; j < SECTOR_HEIGHT; j++) {
                Cells[i][j] = new Cell(tiles[i][j]);
            }
        }
        GlobalCoordinates = new Location();
    }

    public FieldSector(Field f) {
        super(f);
        GlobalCoordinates = new Location();
    }

    public FieldSector(FieldSector f) {
        super(f);
        GlobalCoordinates = new Location(f.getGlobalCoordinates());
    }

    /**
     * @return the GlobalCoordinates
     */
    public Location getGlobalCoordinates() {
        return GlobalCoordinates;
    }

    /**
     * @param GlobalCoordinates the GlobalCoordinates to set
     */
    public void setGlobalCoordinates(Location GlobalCoordinates) {
        this.GlobalCoordinates = GlobalCoordinates;
    }
}
