/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package gui;

import Logic.GameMidlet;
import java.io.IOException;
import javax.microedition.lcdui.game.Sprite;

/**
 *
 * @author е
 */
public class CastleCanvas  extends ExtGameCanvas {

    //<editor-fold defaultstate="collapsed" desc=" Fields ">
    protected GameDesign gd;
    //</editor-fold>
    
    public CastleCanvas(GameMidlet m) {
        super(m);

        try {
             this.init();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    protected void init() throws IOException, IOException {
        super.init();
        gd = new GameDesign();
        gd.updateLayerManagerForCastleScene(lm);
    }

    public void run() {

        while (!this.interrupted) {
            int keyState = getKeyStates();
            if ((keyState & FIRE_PRESSED) != 0) {
                this.stop();
            }

            repaintOnIteration();
        }
    }

    public boolean spriteCollides(Sprite sprite) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public boolean spriteCollidesWithBorder(Sprite sprite) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

}
