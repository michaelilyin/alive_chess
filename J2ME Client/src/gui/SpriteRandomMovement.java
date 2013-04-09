
package gui;

import javax.microedition.lcdui.game.GameCanvas;
import javax.microedition.lcdui.game.Sprite;

/**
 * Animate a sprite on canvas using a simple algorithm
 * to recover from collisions.
 *
 * @author e
 */
public class SpriteRandomMovement implements Runnable {

    private static final int SPEED = 3;
    private ExtGameCanvas canvas;
    private Sprite sprite;
    private byte previousDirection = GameCanvas.DOWN;
    private byte direction = GameCanvas.DOWN;
    private boolean interrupted;
    private int[] downSeq;
    private int downTrans;
    private int[] upSeq;
    private int upTrans;
    private int[] leftSeq;
    private int leftTrans;
    private int[] rightSeq;
    private int rightTrans;

    public SpriteRandomMovement(ExtGameCanvas canvas, Sprite sprite) {
        this.canvas = canvas;
        this.sprite = sprite;
    }

    public void setSequences(int[] downSeq, int downTrans, int[] upSeq, int upTrans, int[] leftSeq, int leftTrans, int[] rightSeq, int rightTrans) {
        this.downSeq = downSeq;
        this.downTrans = downTrans;
        this.upSeq = upSeq;
        this.upTrans = upTrans;
        this.leftSeq = leftSeq;
        this.leftTrans = leftTrans;
        this.rightSeq = rightSeq;
        this.rightTrans = rightTrans;
    }

    public void stop() {
        this.interrupted = true;
    }

    public void run() {
        while (!this.interrupted) {
            if (this.direction == GameCanvas.DOWN) {
                if (this.previousDirection != this.direction) {
                    this.sprite.setFrameSequence(this.downSeq);
                    this.sprite.setTransform(this.downTrans);
                    this.previousDirection = this.direction;
                }
                this.sprite.move(0, SPEED);
                if (this.canvas.spriteCollides(this.sprite)) {
                    this.sprite.move(0, -SPEED);
                    this.direction = GameCanvas.LEFT;
                    continue;
                }
            } else if (this.direction == GameCanvas.UP) {
                if (this.previousDirection != this.direction) {
                    this.sprite.setFrameSequence(this.upSeq);
                    this.sprite.setTransform(this.upTrans);
                    this.previousDirection = this.direction;
                }
                this.sprite.move(0, -SPEED);
                if (this.canvas.spriteCollides(this.sprite)) {
                    this.sprite.move(0, SPEED);
                    this.direction = GameCanvas.RIGHT;
                    continue;
                }
            } else if (this.direction == GameCanvas.LEFT) {
                if (this.previousDirection != this.direction) {
                    this.sprite.setFrameSequence(this.leftSeq);
                    this.sprite.setTransform(this.leftTrans);
                    this.previousDirection = this.direction;
                }
                this.sprite.move(-SPEED, 0);
                if (this.canvas.spriteCollides(this.sprite)) {
                    this.sprite.move(SPEED, 0);
                    this.direction = GameCanvas.UP;
                    continue;
                }
            } else if (this.direction == GameCanvas.RIGHT) {
                if (this.previousDirection != this.direction) {
                    this.sprite.setFrameSequence(this.rightSeq);
                    this.sprite.setTransform(this.rightTrans);
                    this.previousDirection = this.direction;
                }
                this.sprite.move(SPEED, 0);
                if (this.canvas.spriteCollides(this.sprite)) {
                    this.sprite.move(-SPEED, 0);
                    this.direction = GameCanvas.DOWN;
                    continue;
                }
            }
            try {
                Thread.sleep(300);
            } catch (InterruptedException ex) {
                ex.printStackTrace();
            }
        }
    }
}