/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Logic;

/**
 *
 * @author е
 */
public class Location {

    private int x;
    private int y;

    public Location(){
        this(0,0);
    }

    public Location(int x, int y){
        this.x = x;
        this.y = y;
    }

    public Location(Location loc) {
        this(loc.getX(), loc.getY());
    }

    /**
     * @return the x
     */
    public int getX() {
        return x;
    }

    /**
     * @param x the x to set
     */
    public void setX(int x) {
        this.x = x;
    }

    /**
     * @return the y
     */
    public int getY() {
        return y;
    }

    /**
     * @param y the y to set
     */
    public void setY(int y) {
        this.y = y;
    }

    /**
     * Увеличивает координату X на dx
     * @param dx
     */
    public void incX(int dx) {
        x += dx;
    }

    /**
     * Увеличивает координату Y на dy
     */
    public void incY(int dy) {
        y += dy;
    }

    public int hashCode() {
        int hash = 3;
        hash = 37 * hash + this.x;
        hash = 37 * hash + this.y;
        return hash;
    }

    public boolean equals(Object obj) {
        if (!obj.getClass().equals(this.getClass())) return false;
            Location loc = (Location)obj;
            return (this.getX() == loc.getX() && this.getY() == loc.getY());
    }

    public String toString() {
        return "x = " + String.valueOf(this.getX()) + ", y = " + String.valueOf(this.getY());
    }
}
