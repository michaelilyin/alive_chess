/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package gui;

import Logic.GameMidlet;
import world.Field;
import java.io.IOException;
import java.util.Vector;
import javax.microedition.lcdui.Graphics;
import javax.microedition.lcdui.game.Sprite;
import javax.microedition.lcdui.game.TiledLayer;
import util.ImageLoader;

/**
 *
 * @author е
 */
public class BattleCanvas extends ExtGameCanvas{

    //<editor-fold defaultstate="collapsed" desc=" Fields ">
//    private static final int SPEED = 3;
//    private static final int CURSOR_SPEED = 6;
//    private static final int MIN_BUFFER = 50;
//    private int viewPortX = 0;
//    private int viewPortY = 0;
//    private byte lastDirection = -1;
    //private boolean first=true;
    protected GameDesign gd;
    private Sprite spriteKarel;
    private Sprite spriteTrace;
    
    private Sprite spriteThomas;
    private SpriteRandomMovement spriteThomasRandomMovement;
    private SpriteAnimationTask spriteKarelAnimator;
    private SpriteAnimationTask spriteThomasAnimator;
    //private TiledLayer tlBase;
    //private TiledLayer tlBattleBack1;


    private Vector SpriteVector;
    private Field BattleField;

    private boolean PlayerWon = false;
    //</editor-fold>

    public BattleCanvas(GameMidlet m) {
        super(m);

        try {
            this.init();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    protected void init() throws IOException {
        super.init();
        gd = new GameDesign();
        gd.updateLayerManagerForBattle(lm);

        //Vector layers = fl.getBattleFieldLayers();

        this.tlBase = sl.getBattleBase();
        //this.tlBattleBack1 = (TiledLayer) layers.elementAt(1);
        
        spriteCursor.defineReferencePixel(7, 7);
        this.spriteKarel = gd.getKarel();
        //this.SpriteVector = new Vector();
        //define the reference in the midle of sprites frame so that transformations work well
        this.spriteKarel.defineReferencePixel(8, 8);
        this.spriteKarelAnimator = new SpriteAnimationTask(this.spriteKarel, false);
        this.timer.scheduleAtFixedRate(this.spriteKarelAnimator, 0, gd.KarelSeqWalkDownDelay);

        this.spriteTrace= new Sprite(this.gd.getArrowSprite());
        //this.spriteTrace.setVisible(true);
        this.spriteTrace.defineReferencePixel(8, 8);
        this.spriteTrace.setFrame(0);
        //this.spriteTrace.defineCollisionRectangle(UP, UP, UP, UP);



        

        //загружаем препятствия из слоев графики в объект поля
        BattleField = new Field(8, 8);

        this.spriteThomas = gd.getThomas();
        //define the reference in the midle of sprites frame so that transformations work well
        this.spriteThomas.defineReferencePixel(8, 8);
        this.spriteThomasAnimator = new SpriteAnimationTask(this.spriteThomas, true);
        this.spriteThomasAnimator.setMoving(true);
        this.timer.scheduleAtFixedRate(this.spriteThomasAnimator, 0, gd.ThomasSeqWalkHorizDelay);
        this.spriteThomasRandomMovement = new SpriteRandomMovement(this, spriteThomas);
        this.spriteThomasRandomMovement.setSequences(
            gd.ThomasSeqWalkVert, Sprite.TRANS_NONE,
            gd.ThomasSeqWalkVert, Sprite.TRANS_ROT180,
            gd.ThomasSeqWalkHoriz, Sprite.TRANS_ROT180,
            gd.ThomasSeqWalkHoriz, Sprite.TRANS_NONE
            );
        (new Thread(spriteThomasRandomMovement)).start();
        
        spriteKarel.setPosition(217, 9);
        spriteThomas.setPosition(217, 217);
        lm.insert(spriteKarel, 0);
        lm.insert(spriteThomas, 0);
    }

    public void run() {
        //Graphics g = getGraphics();

        while (!this.interrupted) {
        //while (true) {
            //check for user input
            int keyState = getKeyStates();

            if ((keyState & LEFT_PRESSED) != 0) {
                this.lastDirection = LEFT;
                this.spriteCursor.move(-CURSOR_SPEED, 0);
                if (spriteCollidesWithBorder(spriteCursor)) {
                    this.spriteCursor.move(CURSOR_SPEED, 0);
                    continue;
                }
//                this.adjustViewport(this.viewPortX - CURSOR_SPEED, this.viewPortY, spriteCursor);
            }

             else if ((keyState & RIGHT_PRESSED) != 0) {
                this.lastDirection = RIGHT;
                this.spriteCursor.move(CURSOR_SPEED, 0);
                if (spriteCollidesWithBorder(spriteCursor)) {
                    this.spriteCursor.move(-CURSOR_SPEED, 0);
                    continue;
                }
//                this.adjustViewport(this.viewPortX + CURSOR_SPEED, this.viewPortY, spriteCursor);
             }

            else if ((keyState & UP_PRESSED) != 0) {
                this.lastDirection = UP;
                this.spriteCursor.move(0, -CURSOR_SPEED);
                if (spriteCollidesWithBorder(spriteCursor)) {
                    this.spriteCursor.move(0, CURSOR_SPEED);
                    continue;
                }
//                this.adjustViewport(this.viewPortX, this.viewPortY - CURSOR_SPEED, spriteCursor);
            }

            else if ((keyState & DOWN_PRESSED) != 0) {
                this.lastDirection = DOWN;
                this.spriteCursor.move(0, CURSOR_SPEED);
                if (spriteCollidesWithBorder(spriteCursor)) {
                    this.spriteCursor.move(0, -CURSOR_SPEED);
                    continue;
                }
//                this.adjustViewport(this.viewPortX, this.viewPortY + CURSOR_SPEED, spriteCursor);
            }

            else if ((keyState & FIRE_PRESSED) != 0) {
//                if (step==-1) {
//                    clear_trace(this.SpriteVector);
                    //brezen_line(this.spriteKarel.getX()/16, this.spriteKarel.getY()/16, this.spriteCursor.getX()/16, this.spriteCursor.getY()/16);
//                    trace = drawTrace(this.spriteKarel.getX()/16, this.spriteKarel.getY()/16, this.spriteCursor.getX()/16, this.spriteCursor.getY()/16);
//                    step = trace.size()-1;
//                    cur = new Location(this.spriteKarel.getX()/16, this.spriteKarel.getY()/16);

                    if (Math.abs(this.spriteCursor.getX()/30-this.spriteThomas.getX()/30)<1 && Math.abs(this.spriteCursor.getY()/30-this.spriteThomas.getY()/30)<1) {
                        try {
                            this.spriteThomas.setImage(ImageLoader.getBlood(), 16, 16);
                        } catch (IOException ex) {
                            ex.printStackTrace();
                        }
                        this.spriteThomasRandomMovement.stop();
                        repaintOnIteration();
                        try {
                            Thread.sleep(100);
                            //вернуться на стратегическую карту
                        } catch (InterruptedException ex) {
                            ex.printStackTrace();
                        }
                        //вернуться на стратегическую карту

                        PlayerWon = true;
                        this.stop();
                        
                        break;
                       
                    }
                }
            
            repaintOnIteration();
        }
    }

    
    public void stop() {
            this.spriteThomasRandomMovement.stop();
            super.stop();
    }

    public boolean spriteCollides(Sprite sprite) {
        return sprite.collidesWith(
            sprite == this.spriteKarel ? this.spriteThomas : this.spriteKarel, true)
// || sprite.collidesWith(this.tlThings, true)
//                || sprite.collidesWith(this.tlTrees, true)
//                || sprite.collidesWith(this.tlWater, true)
//            || spriteCollidesWithSearchField(sprite)
            || spriteCollidesWithBorder(sprite);
    }

    public boolean spriteCollidesWithBorder(Sprite sprite) {
        return
            sprite.getX() < 0 || sprite.getY() < 0
            || sprite.getX() > (this.tlBase.getWidth() - sprite.getWidth())
            || sprite.getY() > (this.tlBase.getHeight() - sprite.getHeight());
    }

    /**
     * @return the PlayerWon
     */
    public boolean isPlayerWon() {
        return PlayerWon;
    }

}
