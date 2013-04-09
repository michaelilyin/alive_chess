
package gui;

import javax.microedition.lcdui.*;
import javax.microedition.lcdui.game.*;

/**
 * @author  e
 * @version 1.0
 */
public class GameDesign {
    
    //<editor-fold defaultstate="collapsed" desc=" Generated Fields ">//GEN-BEGIN:|fields|0|
    private Image Border30x30;
    private TiledLayer BattleBoard;
    private TiledLayer BattleBack1;
    private Image topview_tiles;
    public int AnimWaterSeq001Delay = 200;
    public int[] AnimWaterSeq001 = {71, 72, 73, 74, 75};
    private TiledLayer Water;
    public int AnimWaterWater;
    private TiledLayer Base;
    private Image back1;
    private TiledLayer Trees;
    private Image Back16x16;
    private TiledLayer CastleBack1;
    private Image cur1;
    private Sprite Cursor;
    public int Cursorseq001Delay = 200;
    public int[] Cursorseq001 = {0};
    private Image blood;
    private Sprite CellBorder;
    public int CellBorderseq001Delay = 200;
    public int[] CellBorderseq001 = {0};
    private TiledLayer Things;
    private Sprite spriteBlood;
    public int spriteBloodseq001Delay = 200;
    public int[] spriteBloodseq001 = {0};
    private TiledLayer CastleMenuBoard;
    private TiledLayer CastleBackground;
    private Image Border16x16;
    private Image CastleBackground1;
    private Image platform_tiles;
    private Sprite Thomas;
    public int ThomasSeqWalkHorizDelay = 140;
    public int[] ThomasSeqWalkHoriz = {80, 81, 82, 83};
    public int ThomasSeqWalkVertDelay = 140;
    public int[] ThomasSeqWalkVert = {84, 85, 86, 87};
    private Sprite ArrowSprite;
    public int ArrowSpriteseq002Delay = 200;
    public int[] ArrowSpriteseq002 = {0, 1, 2};
    private Image arrow1;
    private Sprite Karel;
    public int KarelSeqWalkSideFastDelay = 100;
    public int[] KarelSeqWalkSideFast = {100, 101, 102, 103};
    public int KarelSeqWalkDownDelay = 150;
    public int[] KarelSeqWalkDown = {88, 89, 90, 91};
    public int KarelSeqWalkUpDelay = 150;
    public int[] KarelSeqWalkUp = {94, 95, 96, 97};
    public int KarelSeqWalkSideDelay = 150;
    public int[] KarelSeqWalkSide = {100, 101, 102, 103};
    private Image castle1;
    private TiledLayer Castle;
    private Image CastleBackground2;
    private TiledLayer CastleBackGround2;
    //</editor-fold>//GEN-END:|fields|0|
    
    //<editor-fold defaultstate="collapsed" desc=" Generated Methods ">//GEN-BEGIN:|methods|0|
    //</editor-fold>//GEN-END:|methods|0|

    public void updateLayerManagerForForest(LayerManager lm) throws java.io.IOException {//GEN-LINE:|1-updateLayerManager|0|1-preUpdate
        // write pre-update user code here
        getCursor().setPosition(130, 144);//GEN-BEGIN:|1-updateLayerManager|1|1-postUpdate
        getCursor().setVisible(true);
        lm.append(getCursor());
        getKarel().setPosition(48, 48);
        getKarel().setVisible(true);
        lm.append(getKarel());
        getThomas().setPosition(67, 80);
        getThomas().setVisible(true);
        lm.append(getThomas());
        getCastle().setPosition(256, 192);
        getCastle().setVisible(true);
        lm.append(getCastle());
        getTrees().setPosition(192, 0);
        getTrees().setVisible(true);
        lm.append(getTrees());
        getThings().setPosition(16, 112);
        getThings().setVisible(true);
        lm.append(getThings());
        getWater().setPosition(112, 96);
        getWater().setVisible(true);
        lm.append(getWater());
        getBase().setPosition(0, 0);
        getBase().setVisible(true);
        lm.append(getBase());//GEN-END:|1-updateLayerManager|1|1-postUpdate

    }//GEN-BEGIN:|1-updateLayerManager|2|
//GEN-END:|1-updateLayerManager|2|

    public Image getTopview_tiles() throws java.io.IOException {//GEN-BEGIN:|2-getter|0|2-preInit
        if (topview_tiles == null) {//GEN-END:|2-getter|0|2-preInit
            // write pre-init user code here
            topview_tiles = Image.createImage("/topview_tiles.png");//GEN-BEGIN:|2-getter|1|2-postInit
        }//GEN-END:|2-getter|1|2-postInit
        // write post-init user code here
        return this.topview_tiles;//GEN-BEGIN:|2-getter|2|
    }
//GEN-END:|2-getter|2|

    public TiledLayer getWater() throws java.io.IOException {//GEN-BEGIN:|3-getter|0|3-preInit
        if (Water == null) {//GEN-END:|3-getter|0|3-preInit
            // write pre-init user code here
            Water = new TiledLayer(8, 6, getTopview_tiles(), 16, 16);//GEN-BEGIN:|3-getter|1|3-midInit
            AnimWaterWater = Water.createAnimatedTile(AnimWaterSeq001[0]);
            int[][] tiles = {
                { 0, 0, 44, 31, 31, 31, 31, 32 },
                { 0, 0, 47, 59, AnimWaterWater, 59, AnimWaterWater, 48 },
                { 0, 0, 47, 59, 59, 61, 64, 45 },
                { 0, 0, 47, 59, 59, 48, 0, 0 },
                { 44, 31, 62, 59, 59, 48, 0, 0 },
                { 46, 64, 64, 64, 64, 45, 0, 0 }
            };//GEN-END:|3-getter|1|3-midInit
            // write mid-init user code here
            for (int row = 0; row < 6; row++) {//GEN-BEGIN:|3-getter|2|3-postInit
                for (int col = 0; col < 8; col++) {
                    Water.setCell(col, row, tiles[row][col]);
                }
            }
        }//GEN-END:|3-getter|2|3-postInit
        // write post-init user code here
        return Water;//GEN-BEGIN:|3-getter|3|
    }
//GEN-END:|3-getter|3|

    public TiledLayer getBase() throws java.io.IOException {//GEN-BEGIN:|6-getter|0|6-preInit
        if (Base == null) {//GEN-END:|6-getter|0|6-preInit
            // write pre-init user code here
            Base = new TiledLayer(24, 23, getTopview_tiles(), 16, 16);//GEN-BEGIN:|6-getter|1|6-midInit
            int[][] tiles = {
                { 7, 3, 17, 6, 6, 6, 6, 35, 23, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
                { 7, 3, 5, 16, 16, 16, 16, 26, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
                { 24, 6, 23, 16, 16, 16, 16, 26, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 16, 26, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 16, 26, 16, 16, 16, 11, 11, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 16, 26, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
                { 16, 15, 16, 16, 16, 16, 16, 30, 28, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
                { 16, 16, 15, 16, 16, 8, 16, 16, 26, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 16, 16, 26, 16, 16, 16, 16, 16, 16, 16, 16, 16, 14, 15, 16, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 16, 16, 30, 25, 25, 25, 25, 28, 16, 16, 16, 16, 14, 16, 16, 16, 16, 16 },
                { 16, 16, 16, 16, 8, 16, 16, 11, 16, 16, 16, 16, 16, 26, 16, 16, 16, 14, 16, 16, 15, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 11, 11, 11, 11, 16, 16, 16, 26, 16, 16, 16, 16, 15, 16, 16, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 11, 11, 11, 11, 11, 11, 16, 16, 26, 16, 16, 16, 16, 16, 15, 15, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 11, 11, 11, 11, 11, 11, 16, 22, 36, 21, 16, 16, 26, 16, 16, 16, 16, 15, 16 },
                { 16, 16, 16, 16, 16, 11, 11, 11, 11, 11, 11, 16, 7, 3, 5, 16, 16, 26, 16, 16, 16, 15, 16, 16 },
                { 16, 16, 16, 16, 16, 11, 11, 11, 11, 11, 16, 27, 34, 1, 33, 25, 25, 29, 16, 16, 14, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 26, 24, 6, 23, 16, 16, 16, 16, 16, 14, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 26, 16, 16, 16, 16, 16, 15, 14, 14, 16, 16, 16, 16 },
                { 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 29, 16, 16, 16, 16, 16, 15, 15, 14, 16, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 15, 15, 16, 16, 16, 16, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 15, 16, 16, 15, 16, 16, 16, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 15, 16, 16, 16, 15, 16, 16, 16, 16 },
                { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 15, 16, 16, 16, 16 }
            };//GEN-END:|6-getter|1|6-midInit
            // write mid-init user code here
            for (int row = 0; row < 23; row++) {//GEN-BEGIN:|6-getter|2|6-postInit
                for (int col = 0; col < 24; col++) {
                    Base.setCell(col, row, tiles[row][col]);
                }
            }
        }//GEN-END:|6-getter|2|6-postInit
        // write post-init user code here
        return Base;//GEN-BEGIN:|6-getter|3|
    }
//GEN-END:|6-getter|3|

    public TiledLayer getThings() throws java.io.IOException {//GEN-BEGIN:|31-getter|0|31-preInit
        if (Things == null) {//GEN-END:|31-getter|0|31-preInit
            // write pre-init user code here
            Things = new TiledLayer(15, 15, getTopview_tiles(), 16, 16);//GEN-BEGIN:|31-getter|1|31-midInit
            int[][] tiles = {
                { 0, 0, 0, 0, 0, 0, 0, 68, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 67, 67, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 67, 67, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 67, 67, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0 },
                { 0, 0, 67, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 69, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 69, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };//GEN-END:|31-getter|1|31-midInit
            // write mid-init user code here
            for (int row = 0; row < 15; row++) {//GEN-BEGIN:|31-getter|2|31-postInit
                for (int col = 0; col < 15; col++) {
                    Things.setCell(col, row, tiles[row][col]);
                }
            }
        }//GEN-END:|31-getter|2|31-postInit
        // write post-init user code here
        return Things;//GEN-BEGIN:|31-getter|3|
    }
//GEN-END:|31-getter|3|

    public Sprite getThomas() throws java.io.IOException {//GEN-BEGIN:|51-getter|0|51-preInit
        if (Thomas == null) {//GEN-END:|51-getter|0|51-preInit
                // write pre-init user code here
            Thomas = new Sprite(getTopview_tiles(), 16, 16);//GEN-BEGIN:|51-getter|1|51-postInit
            Thomas.setFrameSequence(ThomasSeqWalkVert);//GEN-END:|51-getter|1|51-postInit
                // write post-init user code here
        }//GEN-BEGIN:|51-getter|2|
        return Thomas;
    }
//GEN-END:|51-getter|2|

    public Sprite getKarel() throws java.io.IOException {//GEN-BEGIN:|70-getter|0|70-preInit
        if (Karel == null) {//GEN-END:|70-getter|0|70-preInit
                // write pre-init user code here
            Karel = new Sprite(getTopview_tiles(), 16, 16);//GEN-BEGIN:|70-getter|1|70-postInit
            Karel.setFrameSequence(KarelSeqWalkDown);//GEN-END:|70-getter|1|70-postInit
                // write post-init user code here
        }//GEN-BEGIN:|70-getter|2|
        return Karel;
    }
//GEN-END:|70-getter|2|



    public Image getPlatform_tiles() throws java.io.IOException {//GEN-BEGIN:|161-getter|0|161-preInit
        if (platform_tiles == null) {//GEN-END:|161-getter|0|161-preInit
                // write pre-init user code here
            platform_tiles = Image.createImage("/platform_tiles.png");//GEN-BEGIN:|161-getter|1|161-postInit
        }//GEN-END:|161-getter|1|161-postInit
            // write post-init user code here
        return this.platform_tiles;//GEN-BEGIN:|161-getter|2|
    }
//GEN-END:|161-getter|2|

    public TiledLayer getTrees() throws java.io.IOException {//GEN-BEGIN:|276-getter|0|276-preInit
        if (Trees == null) {//GEN-END:|276-getter|0|276-preInit
                // write pre-init user code here
            Trees = new TiledLayer(11, 22, getTopview_tiles(), 16, 16);//GEN-BEGIN:|276-getter|1|276-midInit
            int[][] tiles = {
                { 10, 10, 10, 10, 10, 10, 10, 0, 0, 0, 0 },
                { 0, 12, 10, 10, 10, 10, 10, 10, 0, 0, 0 },
                { 0, 12, 10, 12, 10, 10, 10, 10, 0, 10, 0 },
                { 9, 0, 12, 10, 10, 10, 10, 10, 10, 10, 0 },
                { 0, 0, 12, 12, 10, 10, 10, 10, 10, 10, 0 },
                { 0, 0, 0, 12, 10, 10, 10, 10, 0, 0, 0 },
                { 0, 0, 0, 12, 12, 10, 10, 10, 0, 0, 0 },
                { 0, 0, 0, 12, 10, 10, 12, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 10, 10, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 12, 10, 0, 0, 13, 0, 0 },
                { 0, 0, 0, 12, 0, 0, 0, 0, 13, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 13, 13, 13 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 13, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 13, 13, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 13, 0, 13, 13, 13, 0, 0, 0 }
            };//GEN-END:|276-getter|1|276-midInit
                // write mid-init user code here
            for (int row = 0; row < 22; row++) {//GEN-BEGIN:|276-getter|2|276-postInit
                for (int col = 0; col < 11; col++) {
                    Trees.setCell(col, row, tiles[row][col]);
                }
            }
        }//GEN-END:|276-getter|2|276-postInit
            // write post-init user code here
        return Trees;//GEN-BEGIN:|276-getter|3|
    }
//GEN-END:|276-getter|3|






    public Image getArrow1() throws java.io.IOException {//GEN-BEGIN:|663-getter|0|663-preInit
        if (arrow1 == null) {//GEN-END:|663-getter|0|663-preInit
            // write pre-init user code here
            arrow1 = Image.createImage("/arrows.png");//GEN-BEGIN:|663-getter|1|663-postInit
        }//GEN-END:|663-getter|1|663-postInit
        // write post-init user code here
        return this.arrow1;//GEN-BEGIN:|663-getter|2|
    }
//GEN-END:|663-getter|2|

    public Sprite getArrowSprite() throws java.io.IOException {//GEN-BEGIN:|664-getter|0|664-preInit
        if (ArrowSprite == null) {//GEN-END:|664-getter|0|664-preInit
            // write pre-init user code here
            ArrowSprite = new Sprite(getArrow1(), 16, 16);//GEN-BEGIN:|664-getter|1|664-postInit
            ArrowSprite.setFrameSequence(ArrowSpriteseq002);//GEN-END:|664-getter|1|664-postInit
            // write post-init user code here
        }//GEN-BEGIN:|664-getter|2|
        return ArrowSprite;
    }
//GEN-END:|664-getter|2|

    public Image getBorder30x30() throws java.io.IOException {//GEN-BEGIN:|687-getter|0|687-preInit
        if (Border30x30 == null) {//GEN-END:|687-getter|0|687-preInit
            // write pre-init user code here
            Border30x30 = Image.createImage("/Border30x30.png");//GEN-BEGIN:|687-getter|1|687-postInit
        }//GEN-END:|687-getter|1|687-postInit
        // write post-init user code here
        return this.Border30x30;//GEN-BEGIN:|687-getter|2|
    }
//GEN-END:|687-getter|2|

    public Sprite getCellBorder() throws java.io.IOException {//GEN-BEGIN:|688-getter|0|688-preInit
        if (CellBorder == null) {//GEN-END:|688-getter|0|688-preInit
            // write pre-init user code here
            CellBorder = new Sprite(getBorder30x30(), 30, 30);//GEN-BEGIN:|688-getter|1|688-postInit
            CellBorder.setFrameSequence(CellBorderseq001);//GEN-END:|688-getter|1|688-postInit
            // write post-init user code here
        }//GEN-BEGIN:|688-getter|2|
        return CellBorder;
    }
//GEN-END:|688-getter|2|

    public Image getCur1() throws java.io.IOException {//GEN-BEGIN:|690-getter|0|690-preInit
        if (cur1 == null) {//GEN-END:|690-getter|0|690-preInit
            // write pre-init user code here
            cur1 = Image.createImage("/cursor.png");//GEN-BEGIN:|690-getter|1|690-postInit
        }//GEN-END:|690-getter|1|690-postInit
        // write post-init user code here
        return this.cur1;//GEN-BEGIN:|690-getter|2|
    }
//GEN-END:|690-getter|2|

    public Sprite getCursor() throws java.io.IOException {//GEN-BEGIN:|691-getter|0|691-preInit
        if (Cursor == null) {//GEN-END:|691-getter|0|691-preInit
            // write pre-init user code here
            Cursor = new Sprite(getCur1(), 16, 16);//GEN-BEGIN:|691-getter|1|691-postInit
            Cursor.setFrameSequence(Cursorseq001);//GEN-END:|691-getter|1|691-postInit
            // write post-init user code here
        }//GEN-BEGIN:|691-getter|2|
        return Cursor;
    }
//GEN-END:|691-getter|2|

    public Image getBlood() throws java.io.IOException {//GEN-BEGIN:|809-getter|0|809-preInit
        if (blood == null) {//GEN-END:|809-getter|0|809-preInit
            // write pre-init user code here
            blood = Image.createImage("/blood.png");//GEN-BEGIN:|809-getter|1|809-postInit
        }//GEN-END:|809-getter|1|809-postInit
        // write post-init user code here
        return this.blood;//GEN-BEGIN:|809-getter|2|
    }
//GEN-END:|809-getter|2|

    public Sprite getSpriteBlood() throws java.io.IOException {//GEN-BEGIN:|810-getter|0|810-preInit
        if (spriteBlood == null) {//GEN-END:|810-getter|0|810-preInit
            // write pre-init user code here
            spriteBlood = new Sprite(getBlood(), 16, 16);//GEN-BEGIN:|810-getter|1|810-postInit
            spriteBlood.setFrameSequence(spriteBloodseq001);//GEN-END:|810-getter|1|810-postInit
            // write post-init user code here
        }//GEN-BEGIN:|810-getter|2|
        return spriteBlood;
    }
//GEN-END:|810-getter|2|

    public Image getBack1() throws java.io.IOException {//GEN-BEGIN:|819-getter|0|819-preInit
        if (back1 == null) {//GEN-END:|819-getter|0|819-preInit
            // write pre-init user code here
            back1 = Image.createImage("/back1.png");//GEN-BEGIN:|819-getter|1|819-postInit
        }//GEN-END:|819-getter|1|819-postInit
        // write post-init user code here
        return this.back1;//GEN-BEGIN:|819-getter|2|
    }
//GEN-END:|819-getter|2|

    public TiledLayer getBattleBack1() throws java.io.IOException {//GEN-BEGIN:|820-getter|0|820-preInit
        if (BattleBack1 == null) {//GEN-END:|820-getter|0|820-preInit
            // write pre-init user code here
            BattleBack1 = new TiledLayer(8, 8, getBack1(), 30, 30);//GEN-BEGIN:|820-getter|1|820-midInit
            int[][] tiles = {
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 }
            };//GEN-END:|820-getter|1|820-midInit
            // write mid-init user code here
            for (int row = 0; row < 8; row++) {//GEN-BEGIN:|820-getter|2|820-postInit
                for (int col = 0; col < 8; col++) {
                    BattleBack1.setCell(col, row, tiles[row][col]);
                }
            }
        }//GEN-END:|820-getter|2|820-postInit
        // write post-init user code here
        return BattleBack1;//GEN-BEGIN:|820-getter|3|
    }
//GEN-END:|820-getter|3|

    public TiledLayer getBattleBoard() throws java.io.IOException {//GEN-BEGIN:|821-getter|0|821-preInit
        if (BattleBoard == null) {//GEN-END:|821-getter|0|821-preInit
            // write pre-init user code here
            BattleBoard = new TiledLayer(8, 8, getBorder30x30(), 30, 30);//GEN-BEGIN:|821-getter|1|821-midInit
            int[][] tiles = {
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 }
            };//GEN-END:|821-getter|1|821-midInit
            // write mid-init user code here
            for (int row = 0; row < 8; row++) {//GEN-BEGIN:|821-getter|2|821-postInit
                for (int col = 0; col < 8; col++) {
                    BattleBoard.setCell(col, row, tiles[row][col]);
                }
            }
        }//GEN-END:|821-getter|2|821-postInit
        // write post-init user code here
        return BattleBoard;//GEN-BEGIN:|821-getter|3|
    }
//GEN-END:|821-getter|3|

    public void updateLayerManagerForBattle(LayerManager lm) throws java.io.IOException {//GEN-LINE:|822-updateLayerManager|0|822-preUpdate
        // write pre-update user code here
        getCursor().setPosition(97, 98);//GEN-BEGIN:|822-updateLayerManager|1|822-postUpdate
        getCursor().setVisible(true);
        lm.append(getCursor());
        getBattleBoard().setPosition(0, 0);
        getBattleBoard().setVisible(true);
        lm.append(getBattleBoard());
        getBattleBack1().setPosition(0, 0);
        getBattleBack1().setVisible(true);
        lm.append(getBattleBack1());//GEN-END:|822-updateLayerManager|1|822-postUpdate
        // write post-update user code here
    }//GEN-BEGIN:|822-updateLayerManager|2|
//GEN-END:|822-updateLayerManager|2|

    public Image getCastle1() throws java.io.IOException {//GEN-BEGIN:|869-getter|0|869-preInit
        if (castle1 == null) {//GEN-END:|869-getter|0|869-preInit
            // write pre-init user code here
            castle1 = Image.createImage("/castle1.png");//GEN-BEGIN:|869-getter|1|869-postInit
        }//GEN-END:|869-getter|1|869-postInit
        // write post-init user code here
        return this.castle1;//GEN-BEGIN:|869-getter|2|
    }
//GEN-END:|869-getter|2|

    public TiledLayer getCastle() throws java.io.IOException {//GEN-BEGIN:|870-getter|0|870-preInit
        if (Castle == null) {//GEN-END:|870-getter|0|870-preInit
            // write pre-init user code here
            Castle = new TiledLayer(3, 2, getCastle1(), 16, 16);//GEN-BEGIN:|870-getter|1|870-midInit
            int[][] tiles = {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };//GEN-END:|870-getter|1|870-midInit
            // write mid-init user code here
            for (int row = 0; row < 2; row++) {//GEN-BEGIN:|870-getter|2|870-postInit
                for (int col = 0; col < 3; col++) {
                    Castle.setCell(col, row, tiles[row][col]);
                }
            }
        }//GEN-END:|870-getter|2|870-postInit
        // write post-init user code here
        return Castle;//GEN-BEGIN:|870-getter|3|
    }
//GEN-END:|870-getter|3|

    public Image getCastleBackground1() throws java.io.IOException {//GEN-BEGIN:|919-getter|0|919-preInit
        if (CastleBackground1 == null) {//GEN-END:|919-getter|0|919-preInit
            // write pre-init user code here
            CastleBackground1 = Image.createImage("/CastleBackground1.png");//GEN-BEGIN:|919-getter|1|919-postInit
        }//GEN-END:|919-getter|1|919-postInit
        // write post-init user code here
        return this.CastleBackground1;//GEN-BEGIN:|919-getter|2|
    }
//GEN-END:|919-getter|2|

    public TiledLayer getCastleBackground() throws java.io.IOException {//GEN-BEGIN:|920-getter|0|920-preInit
        if (CastleBackground == null) {//GEN-END:|920-getter|0|920-preInit
            // write pre-init user code here
            CastleBackground = new TiledLayer(1, 1, getCastleBackground1(), 320, 144);//GEN-BEGIN:|920-getter|1|920-midInit
            int[][] tiles = {
                { 1 }
            };//GEN-END:|920-getter|1|920-midInit
            // write mid-init user code here
            for (int row = 0; row < 1; row++) {//GEN-BEGIN:|920-getter|2|920-postInit
                for (int col = 0; col < 1; col++) {
                    CastleBackground.setCell(col, row, tiles[row][col]);
                }
            }
        }//GEN-END:|920-getter|2|920-postInit
        // write post-init user code here
        return CastleBackground;//GEN-BEGIN:|920-getter|3|
    }
//GEN-END:|920-getter|3|

    public Image getBorder16x16() throws java.io.IOException {//GEN-BEGIN:|921-getter|0|921-preInit
        if (Border16x16 == null) {//GEN-END:|921-getter|0|921-preInit
            // write pre-init user code here
            Border16x16 = Image.createImage("/Border16x16.png");//GEN-BEGIN:|921-getter|1|921-postInit
        }//GEN-END:|921-getter|1|921-postInit
        // write post-init user code here
        return this.Border16x16;//GEN-BEGIN:|921-getter|2|
    }
//GEN-END:|921-getter|2|

    public TiledLayer getCastleMenuBoard() throws java.io.IOException {//GEN-BEGIN:|922-getter|0|922-preInit
        if (CastleMenuBoard == null) {//GEN-END:|922-getter|0|922-preInit
            // write pre-init user code here
            CastleMenuBoard = new TiledLayer(15, 6, getBorder16x16(), 16, 16);//GEN-BEGIN:|922-getter|1|922-midInit
            int[][] tiles = {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
            };//GEN-END:|922-getter|1|922-midInit
            // write mid-init user code here
            for (int row = 0; row < 6; row++) {//GEN-BEGIN:|922-getter|2|922-postInit
                for (int col = 0; col < 15; col++) {
                    CastleMenuBoard.setCell(col, row, tiles[row][col]);
                }
            }
        }//GEN-END:|922-getter|2|922-postInit
        // write post-init user code here
        return CastleMenuBoard;//GEN-BEGIN:|922-getter|3|
    }
//GEN-END:|922-getter|3|

    public void updateLayerManagerForCastleScene(LayerManager lm) throws java.io.IOException {//GEN-LINE:|923-updateLayerManager|0|923-preUpdate
        // write pre-update user code here
        getCastleMenuBoard().setPosition(0, 224);//GEN-BEGIN:|923-updateLayerManager|1|923-postUpdate
        getCastleMenuBoard().setVisible(true);
        lm.append(getCastleMenuBoard());
        getCastleBack1().setPosition(0, 224);
        getCastleBack1().setVisible(true);
        lm.append(getCastleBack1());
        getCastleBackGround2().setPosition(0, 0);
        getCastleBackGround2().setVisible(true);
        lm.append(getCastleBackGround2());//GEN-END:|923-updateLayerManager|1|923-postUpdate
        // write post-update user code here
    }//GEN-BEGIN:|923-updateLayerManager|2|
//GEN-END:|923-updateLayerManager|2|

    public Image getBack16x16() throws java.io.IOException {//GEN-BEGIN:|946-getter|0|946-preInit
        if (Back16x16 == null) {//GEN-END:|946-getter|0|946-preInit
            // write pre-init user code here
            Back16x16 = Image.createImage("/Back16x16.png");//GEN-BEGIN:|946-getter|1|946-postInit
        }//GEN-END:|946-getter|1|946-postInit
        // write post-init user code here
        return this.Back16x16;//GEN-BEGIN:|946-getter|2|
    }
//GEN-END:|946-getter|2|

    public TiledLayer getCastleBack1() throws java.io.IOException {//GEN-BEGIN:|947-getter|0|947-preInit
        if (CastleBack1 == null) {//GEN-END:|947-getter|0|947-preInit
            // write pre-init user code here
            CastleBack1 = new TiledLayer(15, 6, getBack16x16(), 16, 16);//GEN-BEGIN:|947-getter|1|947-midInit
            int[][] tiles = {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
            };//GEN-END:|947-getter|1|947-midInit
            // write mid-init user code here
            for (int row = 0; row < 6; row++) {//GEN-BEGIN:|947-getter|2|947-postInit
                for (int col = 0; col < 15; col++) {
                    CastleBack1.setCell(col, row, tiles[row][col]);
                }
            }
        }//GEN-END:|947-getter|2|947-postInit
        // write post-init user code here
        return CastleBack1;//GEN-BEGIN:|947-getter|3|
    }
//GEN-END:|947-getter|3|

    public Image getCastleBackground2() throws java.io.IOException {//GEN-BEGIN:|1010-getter|0|1010-preInit
        if (CastleBackground2 == null) {//GEN-END:|1010-getter|0|1010-preInit
            // write pre-init user code here
            CastleBackground2 = Image.createImage("/CastleBackground2.png");//GEN-BEGIN:|1010-getter|1|1010-postInit
        }//GEN-END:|1010-getter|1|1010-postInit
        // write post-init user code here
        return this.CastleBackground2;//GEN-BEGIN:|1010-getter|2|
    }
//GEN-END:|1010-getter|2|

    public TiledLayer getCastleBackGround2() throws java.io.IOException {//GEN-BEGIN:|1011-getter|0|1011-preInit
        if (CastleBackGround2 == null) {//GEN-END:|1011-getter|0|1011-preInit
            // write pre-init user code here
            CastleBackGround2 = new TiledLayer(1, 1, getCastleBackground2(), 240, 320);//GEN-BEGIN:|1011-getter|1|1011-midInit
            int[][] tiles = {
                { 1 }
            };//GEN-END:|1011-getter|1|1011-midInit
            // write mid-init user code here
            for (int row = 0; row < 1; row++) {//GEN-BEGIN:|1011-getter|2|1011-postInit
                for (int col = 0; col < 1; col++) {
                    CastleBackGround2.setCell(col, row, tiles[row][col]);
                }
            }
        }//GEN-END:|1011-getter|2|1011-postInit
        // write post-init user code here
        return CastleBackGround2;//GEN-BEGIN:|1011-getter|3|
    }
//GEN-END:|1011-getter|3|


    
}
