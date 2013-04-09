/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package gui;

import java.io.IOException;
import javax.microedition.lcdui.game.TiledLayer;
import Logic.Location;
import javax.microedition.lcdui.game.LayerManager;
import util.ImageLoader;
import world.FieldLoader;
import world.FieldSector;
import world.WorldContext;

/**
 *
 * @author е
 */
public class LayerCache {

    
    public static final int CENTER = 0;
    public static final int UP = 1;
    public static final int RIGHT = 2;
    public static final int DOWN = 3;
    public static final int LEFT = 4;

    public static final int UPRIGHT = 5;
    public static final int DOWNRIGHT = 6;
    public static final int DOWNLEFT = 7;
    public static final int UPLEFT = 8;
    //добавить работу с диагоналями!!!

    FieldLoader fl;
    TiledLayer Center, Up, Right, Down, Left, UpRight, DownRight, DownLeft, UpLeft;
//    Vector InternalTLArr;
    LayerManager lm;
    private Location CenterCoords;
    private int sizeX, sizeY;

    public LayerCache(WorldContext context, LayerManager lm, Location InitialCenterPos) {
        //System.out.println("LayerCache init: " + context.getSizeX() + context.getSizeY());
        fl = context.getFieldLoader();
        this.lm = lm;
        sizeX = context.getSizeX();
        sizeY = context.getSizeY();
        CenterCoords = new Location(InitialCenterPos);
        //FieldSector buf = null;
        loadLayer(CENTER);
        loadLayer(UP);
        loadLayer(RIGHT);
        loadLayer(DOWN);
        loadLayer(LEFT);

        loadLayer(UPRIGHT);
        loadLayer(DOWNRIGHT);
        loadLayer(DOWNLEFT);
        loadLayer(UPLEFT);
        
//        //инициализируем вспомогательный массив ссылок на слои - порядок важен!
        //отказался от использования из-за проблем с null-эл-тами, нарушающими порядок
//        InternalTLArr = new Vector();
//        InternalTLArr.addElement(Center);
//        InternalTLArr.addElement(Up);
//        InternalTLArr.addElement(Right);
//        InternalTLArr.addElement(Down);
//        InternalTLArr.addElement(Left);
//        InternalTLArr.addElement(UpRight);
//        InternalTLArr.addElement(DownRight);
//        InternalTLArr.addElement(DownLeft);
//        InternalTLArr.addElement(UpLeft);
    }

    public TiledLayer getLayer(int layertype) {
        switch (layertype) {
            case CENTER: { return Center; }
            case UP: { return Up; }
            case RIGHT: { return Right; }
            case DOWN: { return Down; }
            case LEFT: { return Left; }
            case UPRIGHT: { return UpRight; }
            case DOWNRIGHT: { return DownRight; }
            case DOWNLEFT: { return DownLeft; }
            case UPLEFT: { return UpLeft; }
            default: { return null; }
        }
    }

    public void MoveCenter(int direction) {
        System.out.println("MoveCenter starts:" + String.valueOf(direction));
        switch (direction) {
            case UP: {
                if (getCenterCoords().getY() > 0) {
                    getCenterCoords().setY(getCenterCoords().getY() - 1);
                    unloadLayer(DOWN);
                    unloadLayer(DOWNRIGHT);
                    unloadLayer(DOWNLEFT);
                    Down = Center;
                    Center = Up;
                    DownLeft = Left;
                    Left = UpLeft;
                    DownRight = Right;
                    Right = UpRight;
                    loadLayer(UP);
                    loadLayer(UPRIGHT);
                    loadLayer(UPLEFT);
                }
                break;
            }
            case RIGHT: { 
                if (getCenterCoords().getX() < getSizeX() - 1) {
                    getCenterCoords().setX(getCenterCoords().getX() + 1);
                    unloadLayer(LEFT);
                    unloadLayer(UPLEFT);
                    unloadLayer(DOWNLEFT);
                    Left = Center;
                    Center = Right;
                    UpLeft = Up;
                    Up = UpRight;
                    DownLeft = Down;
                    Down = DownRight;
                    loadLayer(RIGHT);
                    loadLayer(UPRIGHT);
                    loadLayer(DOWNRIGHT);
                }
                break;
            }
            case DOWN: { 
                if (getCenterCoords().getY() < getSizeY() - 1) {
                    getCenterCoords().setY(getCenterCoords().getY() + 1);
                    unloadLayer(UP);
                    unloadLayer(UPRIGHT);
                    unloadLayer(UPLEFT);
                    Up = Center;
                    Center = Down;
                    UpRight = Right;
                    Right = DownRight;
                    UpLeft = Left;
                    Left = DownLeft;                   
                    loadLayer(DOWN);                    
                    loadLayer(DOWNRIGHT);
                    loadLayer(DOWNLEFT);
                }
                break;
            }
            case LEFT: { 
                if (getCenterCoords().getX() > 0) {
                    getCenterCoords().setX(getCenterCoords().getX() - 1);
                    unloadLayer(RIGHT);
                    unloadLayer(UPRIGHT);
                    unloadLayer(DOWNRIGHT);
                    Right = Center;
                    Center = Left;
                    UpRight = Up;
                    Up = UpLeft;
                    DownRight = Down;
                    Down = DownLeft;
                    loadLayer(LEFT);
                    loadLayer(UPLEFT);
                    loadLayer(DOWNLEFT);
                }
                break;
            }
//            case UPRIGHT: {
//                if (getCenterCoords().getX() < getSizeX() - 1 && getCenterCoords().getY() > 0) {
//                    getCenterCoords().setX(getCenterCoords().getX() + 1);
//                    getCenterCoords().setY(getCenterCoords().getY() - 1);
//                }
//                break;
//            }
//            case DOWNRIGHT: {
//                if (getCenterCoords().getX() < getSizeX() - 1 && getCenterCoords().getY() < getSizeY() - 1) {
//                    getCenterCoords().setX(getCenterCoords().getX() + 1);
//                    getCenterCoords().setY(getCenterCoords().getY() + 1);
//                }
//                break;
//            }
//            case DOWNLEFT: {
//                if (getCenterCoords().getX() > 0 && getCenterCoords().getY() < getSizeY() - 1) {
//                    getCenterCoords().setX(getCenterCoords().getX() - 1);
//                    getCenterCoords().setY(getCenterCoords().getY() + 1);
//                }
//                break;
//            }
//            case UPLEFT: {
//                if (getCenterCoords().getX() > 0 && getCenterCoords().getY() > 0) {
//                    getCenterCoords().setX(getCenterCoords().getX() - 1);
//                    getCenterCoords().setY(getCenterCoords().getY() - 1);
//                }
//                break;
//            }

            default: { break; }
        }
        System.out.println("MoveCenter ends:" + String.valueOf(direction));
    }

    /**
     * загрузка слоя по tiles(полученным по координатам) в член класса
     * и обновление layermanager.
     * метод использует getCenterCoords!!!
     * @param layertype
     */
    private void loadLayer(int layertype) {
        //System.out.println("loadLayer starts:" + String.valueOf(layertype));
        FieldSector fs;
        TiledLayer tlNew = null;
        Location loc = null;
        switch (layertype) {
            case CENTER: { loc = new Location(getCenterCoords().getX(), getCenterCoords().getY()); break; }
            case UP: {
                if (getCenterCoords().getY() > 0) {
                    loc = new Location(getCenterCoords().getX(), getCenterCoords().getY() - 1);
                }
                break;
            }
            case RIGHT: {
                if (getCenterCoords().getX() < getSizeX() - 1) {
                    loc = new Location(getCenterCoords().getX() + 1, getCenterCoords().getY());
                }
                break;
            }
            case DOWN: {
                if (getCenterCoords().getY() < getSizeY() - 1) {
                    loc = new Location(getCenterCoords().getX(), getCenterCoords().getY() + 1);
                }
                break;
            }
            case LEFT: {
                if (getCenterCoords().getX() > 0) {
                    loc = new Location(getCenterCoords().getX() - 1, getCenterCoords().getY());
                }
                break;
            }

            case UPRIGHT: {
                if (getCenterCoords().getX() < getSizeX() - 1 && getCenterCoords().getY() > 0) {
                    loc = new Location(getCenterCoords().getX() + 1, getCenterCoords().getY() - 1);
                }
                break;
            }
            case DOWNRIGHT: {
                if (getCenterCoords().getX() < getSizeX() - 1 && getCenterCoords().getY() < getSizeY() - 1) {
                    loc = new Location(getCenterCoords().getX() + 1, getCenterCoords().getY() + 1);
                }
                break;
            }
            case DOWNLEFT: {
                if (getCenterCoords().getX() > 0 && getCenterCoords().getY() < getSizeY() - 1) {
                    loc = new Location(getCenterCoords().getX() - 1, getCenterCoords().getY() + 1);
                }
                break;
            }
            case UPLEFT: {
                if (getCenterCoords().getX() > 0 && getCenterCoords().getY() > 0) {
                    loc = new Location(getCenterCoords().getX() - 1, getCenterCoords().getY() - 1);
                }
                break;
            }

            default: { break; }
        }
        //if (loc == null) return;
        try {
            fs = fl.getStrategyFieldSector(loc);
            //System.out.println(loc.getX());
            tlNew = SpriteLoader.fillLayer(fs, ImageLoader.getStrategyField());
            //if (tlNew == null) throw new NullPointerException("Error while loading layer");
        } catch (NullPointerException ex) {}
        catch (IOException ex) {
            ex.printStackTrace();
        }
        switch (layertype) {
            case CENTER: { Center = tlNew; break; }
            case UP: { Up = tlNew; break; }
            case RIGHT: { Right = tlNew; break; }
            case DOWN: { Down = tlNew; break; }
            case LEFT: { Left = tlNew; break; }
            case UPRIGHT: { UpRight = tlNew; break; }
            case DOWNRIGHT: { DownRight = tlNew; break; }
            case DOWNLEFT: { DownLeft = tlNew; break; }
            case UPLEFT: { UpLeft = tlNew; break; }
            default: { break; }
        }
        if (tlNew != null) lm.append(tlNew);
        
        //System.out.println("loadLayer ends:" + String.valueOf(layertype));
    }

    private void unloadLayer(int layertype) {
//        if (layertype < 0 || layertype > InternalTLArr.size() - 1) return;
//        TiledLayer tl = (TiledLayer)InternalTLArr.elementAt(layertype);
//        if (tl != null)
//            lm.remove(tl);
        TiledLayer tl = null;
        switch (layertype) {
            case CENTER: { tl = Center;break; }
            case UP: { tl = Up; break; }
            case RIGHT: { tl = Right; break; }
            case DOWN: { tl = Down; break; }
            case LEFT: { tl = Left; break; }
            case UPRIGHT: { tl = UpRight; break; }
            case DOWNRIGHT: { tl = DownRight; break; }
            case DOWNLEFT: { tl = DownLeft; break; }
            case UPLEFT: { tl = UpLeft; break; }
            default: { break; }
        }
        if (tl != null) lm.remove(tl);
    }

    /**
     * @return the CenterCoords
     */
    public Location getCenterCoords() {
        return CenterCoords;
    }

    /**
     * @return the sizeX
     */
    public int getSizeX() {
        return sizeX;
    }

    /**
     * @return the sizeY
     */
    public int getSizeY() {
        return sizeY;
    }
}
