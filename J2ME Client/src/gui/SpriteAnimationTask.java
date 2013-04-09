/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package gui;

import java.util.TimerTask;
import javax.microedition.lcdui.Canvas;
import javax.microedition.lcdui.game.Sprite;

/**
 *
 * @author е
 * Animates a sprite.
 */
public class SpriteAnimationTask extends TimerTask {

    public static final int[] ZERO_SEQ = {0};
    public static final int DEFAULT_DELAY = 200;

    private boolean moving = false;
    private boolean forward = true;
    private Sprite sprite;
    int direction;
    int[][] frameseqs;
    int[] framedelays;

    public SpriteAnimationTask(Sprite sprite, boolean forward) {
        this.sprite = sprite;
        this.forward = forward;
        frameseqs = new int[4][];
        framedelays = new int[4];
        for (int i = 0; i < 4; i++) {
            framedelays[i] = DEFAULT_DELAY;
            frameseqs[i] = ZERO_SEQ;
        }
        setDirection(Canvas.DOWN);
    }

    public SpriteAnimationTask(SpriteAnimationTask s) {
        this.sprite = new Sprite(s.sprite);
        this.forward = s.forward;
        this.moving = s.moving;
        frameseqs = new int[4][];
        framedelays = new int[4];
        for (int i = 0; i < 4; i++) {
            framedelays[i] = s.framedelays[i];
            frameseqs[i] = s.frameseqs[i];
        }
        setDirection(s.direction);
    }

    public void run() {
        if (!this.moving) {
            return;
        }
        if (this.forward) {
            this.sprite.nextFrame();
        } else {
            this.sprite.prevFrame();
        }
    }

    public void forward() {
        this.forward = true;
        this.moving = true;
    }

    public void backward() {
        this.forward = false;
        this.moving = true;
    }

    public void setMoving(boolean isMoving) {
        this.moving = isMoving;
    }

    /**
     * Преобразует Canvas.Direction в диапазон 0..3
     * @param direction
     * @return
     */
    protected static int getDirInt(int direction) {
        int i;
        switch (direction) {
            case Canvas.UP: { i = 0; break; }
            case Canvas.RIGHT: { i = 1; break; }
            case Canvas.DOWN: { i = 2; break; }
            case Canvas.LEFT: { i = 3; break; }
            default: { throw new IllegalArgumentException("Invalid direction."); }
        }
        return i;
    }
    
    public void setDirection(int direction) {
        if (this.direction == direction) return;
        this.direction = direction;
        sprite.setFrameSequence(frameseqs[getDirInt(direction)]);
    }

    public int getDirection() {
        return direction;
    }

    public void setFrameSequence(int direction, int[] seq) {
        frameseqs[getDirInt(direction)] = seq;
    }
    public int[] getFrameSequence(int direction) {
        return frameseqs[getDirInt(direction)];
    }
    public void setFrameSequences(int[][] seqs) {
        if (seqs == null) return;
        for (int i = 0; i < 4; i++)
            if (i < seqs.length && seqs[i] != null)
                frameseqs[i] = seqs[i];
    }
    public int[] getCurrentFrameSequence() {
        return frameseqs[getDirInt(direction)];
    }

    public void setFrameDelay(int direction, int delay) {
        framedelays[getDirInt(direction)] = delay;
    }
    public int getFrameDelay(int direction) {
        return framedelays[getDirInt(direction)];
    }
    public void setFrameDelays(int[] delays) {
        if (delays == null) return;
        for (int i = 0; i < 4; i++)
            if (i < delays.length)
                framedelays[i] = delays[i];
    }
    public int getCurrentFrameDelay() {
        return framedelays[getDirInt(direction)];
    }

    public Sprite getSprite() {
        return sprite;
    }
}

