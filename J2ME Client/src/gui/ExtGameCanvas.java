/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package gui;

import Logic.GameMidlet;
import world.FieldLoader;
import gui.SpriteLoader;
import java.io.IOException;
import java.util.Timer;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Graphics;
import javax.microedition.lcdui.game.GameCanvas;
import javax.microedition.lcdui.game.LayerManager;
import javax.microedition.lcdui.game.Sprite;
import javax.microedition.lcdui.game.TiledLayer;

/**
 *
 * @author е
 */
public abstract class ExtGameCanvas extends GameCanvas implements Runnable {

    //<editor-fold defaultstate="collapsed" desc=" Fields ">
    protected final int CURSOR_SPEED = 5;
    protected final int MIN_BUFFER = 50;

    protected GameMidlet midlet;
    protected Timer timer;
    protected LayerManager lm;
    //protected FieldLoader fl;
    protected SpriteLoader sl;
    protected Sprite spriteCursor;
    protected TiledLayer tlBase;

    protected int viewPortX = 0;
    protected int viewPortY = 0;
        
    protected byte lastDirection = -1;
    protected boolean interrupted;

    protected final Object interrBlock = new Object();//mutex
    //</editor-fold>


    public ExtGameCanvas(GameMidlet m) {
        super(true);
        midlet = m;
        this.setFullScreenMode(true);

//        try {
//            this.setFullScreenMode(true);
//            this.init();
//        } catch (IOException ex) {
//            ex.printStackTrace();
//        }

    }

    /**
     * Инициализация канвы
     * @throws java.io.IOException
     */
    protected void init() throws IOException {
        timer = new Timer();
        //fl = new FieldLoader();
        sl = new SpriteLoader();
        lm = new LayerManager();

        spriteCursor = sl.getCursor();
        spriteCursor.defineReferencePixel(7, 7);
        spriteCursor.setPosition(0, 0);
        interrupted = false;

        this.addCommand(
			new Command("EXIT", Command.EXIT, 2));
		this.addCommand(
			new Command("HELP", Command.HELP, 2));
        this.addCommand(
			new Command("MENU", Command.SCREEN, 1));

        this.lm.setViewWindow(0, 0, this.getWidth(), this.getHeight());
    }


    /**
     * Остановка потока (цикла в методе run())
     * и посылка события через переменную interrBlock
     */
    public void stop() {
            interrupted = true;
            try {                
                synchronized(interrBlock) {
                    interrBlock.notify();
                }               
                //getInterrupted().notifyAll();
            } catch (IllegalMonitorStateException ex) {
                ex.printStackTrace();
            }
    }

    protected void repaintOnIteration() {
        Graphics g = getGraphics();
        this.lm.paint(g, 0, 0);
            flushGraphics(0, 0, this.getWidth(), this.getHeight());

            try {
                Thread.sleep(30);
            } catch (InterruptedException ex) {
                ex.printStackTrace();
            }
    }


    /**
     * Adjust the viewport to keep the main animated sprite inside the screen.
     * The coordinates are checked for game bounaries and adjusted only if it
     * makes sense.
     *
     * @param x viewport X coordinate
     * @param y viewport Y coordinate
     */
    protected void adjustViewport(int x, int y, Sprite sprite) {

        int sx = sprite.getX();
        int sy = sprite.getY();

        int xmin = this.viewPortX + MIN_BUFFER;
        int xmax = this.viewPortX + this.getWidth() - sprite.getWidth() - MIN_BUFFER;
        int ymin = this.viewPortY + MIN_BUFFER;
        int ymax = this.viewPortY + this.getHeight() - sprite.getHeight() - MIN_BUFFER;

        //if the sprite is not near the any screen edges don't adjust
        if (sx >= xmin && sx <= xmax && sy >= ymin && sy <= ymax) {
            return;
        }

        //if the sprite is moving left but isn't near the left edge of the screen don't adjust
        if (this.lastDirection == LEFT && sx >= xmin) {
            return;
        }
        //if the sprite is moving right but isn't near the right edge of the screen don't adjust
        if (this.lastDirection == RIGHT && sx <= xmax) {
            return;
        }
        //if the sprite is moving up but isn't at near top edge of the screen don't adjust
        if (this.lastDirection == UP && sy >= ymin) {
            return;
        }
        //if the sprite is moving down but isn't at near bottom edge of the screen don't adjust
        if (this.lastDirection == DOWN && sy <= ymax) {
            return;
        }

        //only adjust x to values that ensure the base tiled layer remains visible
        //and no white space is shown
        if (x < this.tlBase.getX()) {
            this.viewPortX = this.tlBase.getX();
        } else if (x > this.tlBase.getX() + this.tlBase.getWidth() - this.getWidth()) {
            this.viewPortX = this.tlBase.getX() + this.tlBase.getWidth() - this.getWidth();
        } else {
            this.viewPortX = x;
        }

        //only adjust y to values that ensure the base tiled layer remains visible
        //and no white space is shown
        if (y < this.tlBase.getY()) {
            this.viewPortY = this.tlBase.getY();
        } else if (y > this.tlBase.getY() + this.tlBase.getHeight() - this.getHeight()) {
            this.viewPortY = this.tlBase.getY() + this.tlBase.getHeight() - this.getHeight();
        } else {
            this.viewPortY = y;
        }

        //adjust the viewport
        this.lm.setViewWindow(this.viewPortX, this.viewPortY, this.getWidth(), this.getHeight());
    }

    /**
     * При вызове потоком данного метода поток, вызвавший метод, приостанавливается
     * и ожидает interrupted.notify(), которое выполняется при остановке потока канвы.
     * (Канва работает в своем отдельном потоке)
     *
     * @throws java.lang.InterruptedException
     */
    public void waitForInterrupted() throws InterruptedException {
         synchronized(interrBlock) {
            interrBlock.wait();
        }
    }

    public abstract boolean spriteCollides(Sprite sprite);

    public boolean spriteCollidesWithBorder(Sprite sprite) {
        return
            sprite.getX() < 0 || sprite.getY() < 0
            || sprite.getX() > this.tlBase.getWidth() - sprite.getWidth()/2
            || sprite.getY() > this.tlBase.getHeight() - sprite.getHeight()/2
            ;
    }


}
