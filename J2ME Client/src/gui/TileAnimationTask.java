/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package gui;

import java.util.TimerTask;
import javax.microedition.lcdui.game.TiledLayer;

/**
 *
 * @author е

     * Animates animated tiles in a tiled layer.
     */
public class TileAnimationTask extends TimerTask {

    private boolean moving = true;
    private boolean forward = true;
    private TiledLayer tiledLayer;
    private int animatedTileIndex;
    private int[] sequence;
    private int sequenceIndex;

    public TileAnimationTask(TiledLayer tiledLayer, int animatedTileIndex, int[] sequence, boolean forward) {
        this.tiledLayer = tiledLayer;
        this.animatedTileIndex = animatedTileIndex;
        this.sequence = sequence;
        this.forward = forward;
    }

    public void run() {
        if (!this.moving) {
            return;
        }
        if (forward) {
            if (++this.sequenceIndex >= this.sequence.length) {
                sequenceIndex = 0;
            }
        } else {
            if (--this.sequenceIndex < 0) {
                sequenceIndex = this.sequence.length - 1;
            }
        }
        this.tiledLayer.setAnimatedTile(this.animatedTileIndex, this.sequence[sequenceIndex]);
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
}

