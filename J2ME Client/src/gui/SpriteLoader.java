/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package gui;

import world.*;
import gui.SpriteAnimationTask;
import java.io.IOException;
import java.util.Vector;
import javax.microedition.lcdui.Canvas;
import javax.microedition.lcdui.Image;
import javax.microedition.lcdui.game.LayerManager;
import javax.microedition.lcdui.game.Sprite;
import javax.microedition.lcdui.game.TiledLayer;
import obj.LayeredObject;
import util.ImageLoader;

/**
 *
 * @author е
 */
//в классе должны храниться используемые нами спрайты с их sequences.
//к примеру, public int KarelSeqWalkDownDelay = 150;
//и public int[] KarelSeqWalkDown = {88, 89, 90, 91};
//и AnimWaterWater = Water.createAnimatedTile(AnimWaterSeq001[0]);
//а картинки для них - через util.ImageLoader
//также класс по FieldSector создает TiledLayer.
public class SpriteLoader {

    //private static SpriteLoader instance;

   //здесь будет экземпляр cachedstrategylayers

    public static final int TILE_WIDTH = 16;
    public static final int TILE_HEIGHT = 16;

    private TiledLayer tlBattleBase;
    //private TiledLayer tlCastle;

    private SpriteAnimationTask[] KingAnimators;

    private Sprite Cursor;
    private Sprite CursorSword;
    private Sprite CursorIntoCastle;

    private Sprite TraceArrow;
    
    public SpriteLoader() {
        try {
            Sprite s;
            KingAnimators = new SpriteAnimationTask[2];
            s = new Sprite(ImageLoader.getHeroes(), TILE_WIDTH, TILE_HEIGHT);
//            s.defineReferencePixel(TILE_WIDTH / 2, TILE_HEIGHT / 2);
            KingAnimators[0] = new SpriteAnimationTask(s, true);
            KingAnimators[0].setFrameSequence(Canvas.DOWN, new int[]{0, 1, 2, 3});
            KingAnimators[0].setFrameSequence(Canvas.UP, new int[]{6, 7, 8, 9});
            KingAnimators[0].setFrameSequence(Canvas.RIGHT, new int[]{12, 13, 14, 15});           
            KingAnimators[0].setFrameSequence(Canvas.LEFT, new int[]{18, 19, 20, 21});
            KingAnimators[0].setFrameDelays(new int[]{150, 150, 150, 150});
            s = new Sprite(ImageLoader.getHeroes(), TILE_WIDTH, TILE_HEIGHT);
            KingAnimators[1] = new SpriteAnimationTask(s, true);
            KingAnimators[1].setFrameSequence(Canvas.DOWN, new int[]{24, 25, 26, 27});
            KingAnimators[1].setFrameSequence(Canvas.UP, new int[]{28, 29, 30, 31});
            KingAnimators[1].setFrameSequence(Canvas.RIGHT, new int[]{32, 33, 34, 35});
            KingAnimators[1].setFrameSequence(Canvas.LEFT, new int[]{36, 37, 38, 39});
            KingAnimators[1].setFrameDelays(new int[]{100, 100, 100, 100});
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    /*public static SpriteLoader getInstance() {
        if (instance == null)
            instance = new SpriteLoader();
        return instance;
    }*/


    /**
     * Создаем слой в соответствии с наполнением и глобальными координатами сектора поля
     */
    public static TiledLayer fillLayer(FieldSector fs, Image img) throws java.io.IOException {

        if (fs == null || img == null) return null;
        TiledLayer tl = new TiledLayer(FieldSector.SECTOR_WIDTH, FieldSector.SECTOR_HEIGHT,
                img, TILE_WIDTH, TILE_HEIGHT);

        tl.setPosition(fs.getGlobalCoordinates().getX() * FieldSector.SECTOR_WIDTH * TILE_WIDTH,
                fs.getGlobalCoordinates().getY() * FieldSector.SECTOR_HEIGHT * TILE_HEIGHT);

        for (int row = 0; row < tl.getRows(); row++)
            for (int col = 0; col < tl.getColumns(); col++)
                try {
                    tl.setCell(col, row, fs.getCell(col, row).getTerrain() + 1);
                }
                catch (IndexOutOfBoundsException ex) {ex.printStackTrace();}

        return tl;
    }

    public static void updateLayerManager(LayerManager lm, TiledLayer tl) throws java.io.IOException {
        tl.setVisible(true);
        lm.append(tl);
    }

    public static void updateLayerManager(LayerManager lm, Sprite s) throws java.io.IOException {
        s.setVisible(true);
        lm.append(s);
    }
    /**
     * Возвращает вектор слоев для сражения. мб стоит возвращать сразу LayerManager?
     */
    public Vector getBattleFieldLayers() throws IOException {
        Vector res = new Vector();

        res.addElement(this.tlBattleBase);
        //res.addElement(gd.getBattleBack1());

        return res;
    }

    /**
     * Возвращает стандартный слой для сражения
     */
    public TiledLayer getBattleBase() throws java.io.IOException {
        if (tlBattleBase == null) {
            tlBattleBase = new TiledLayer(8, 8, ImageLoader.getBorder30x30(), 16, 16);
            tlBattleBase.setPosition(0, 0);
            tlBattleBase.setVisible(true);
        }
        return tlBattleBase;
    }

    //0 - karel, 1 - thomas
    public SpriteAnimationTask getAnimatedObject(int KingIndex) throws IOException {
        if (KingIndex >= KingAnimators.length || KingIndex < 0) return null;
//        if (KingAnimators[KingIndex] == null) {
//            KingAnimators[KingIndex] = new Sprite(ImageLoader.getHeroes(), 16, 16);
//            KingAnimators[KingIndex].setFrameSequence(new int[]{1, 2, 3, 4});
//        }
        return new SpriteAnimationTask(KingAnimators[KingIndex]);
    }

    public Sprite getCursor() throws java.io.IOException {
        if (Cursor == null) {
            Cursor = new Sprite(ImageLoader.getCursor(), TILE_WIDTH, TILE_HEIGHT);
            Cursor.setFrameSequence(SpriteAnimationTask.ZERO_SEQ);
        }
        return Cursor;
    }

    public Sprite getTraceArrow() throws java.io.IOException {
        if (TraceArrow == null) {
            TraceArrow = new Sprite(ImageLoader.getArrow1(), TILE_WIDTH, TILE_HEIGHT);
            TraceArrow.setFrameSequence(new int[]{0, 1, 2});
        }
        return TraceArrow;
    }

    /**
     *
     * @param app Appearance Type
     * @return
     * @throws java.io.IOException
     */
    public TiledLayer getLayeredObject(int app) throws java.io.IOException {
        TiledLayer tl = null;
        if (app >= 0 || app <= 1) {
            //TODO: реализовать мех-м выбора видов объектов и героев
            tl = new TiledLayer(3, 2, ImageLoader.getFieldObjects(), TILE_WIDTH, TILE_HEIGHT);
                    try {
                        tl.setCell(0, 0, 1 + 6*app);
                        tl.setCell(1, 0, 2 + 6*app);
                        tl.setCell(2, 0, 3 + 6*app);
                        tl.setCell(0, 1, 4 + 6*app);
                        tl.setCell(1, 1, 5 + 6*app);
                        tl.setCell(2, 1, 6 + 6*app);
                    }
                    catch (IndexOutOfBoundsException ex) {ex.printStackTrace();}
        }
        if (app == LayeredObject.T_BOTTLE) {
            tl = new TiledLayer(1, 1, ImageLoader.getFieldObjects(), TILE_WIDTH, TILE_HEIGHT);
            tl.setCell(0, 0, 13);
        }
        if (app == LayeredObject.T_ROAD_SIGN) {
            tl = new TiledLayer(1, 1, ImageLoader.getFieldObjects(), TILE_WIDTH, TILE_HEIGHT);
            tl.setCell(0, 0, 14);
        }
        return tl;
    }
}
