/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package gui;

import Logic.GameMidlet;
import java.io.IOException;
import javax.microedition.lcdui.Graphics;
import javax.microedition.lcdui.game.Sprite;
//import net.SocketTransport;

/**
 *
 * @author Вадим
 */
public class WaitingScreenCanvas extends ExtGameCanvas {

    public WaitingScreenCanvas(GameMidlet m) {
        super(m);

        try {
            super.init();            
            receiveData();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    public void receiveData() {
//        Thread t=new Thread(new SocketTransport(midlet, this));
//        t.start();
    }

    public boolean spriteCollides(Sprite sprite) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public boolean spriteCollidesWithBorder(Sprite sprite) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void run() {
       int i=0;
       while (!this.interrupted) {
            int keyState = getKeyStates();
            if ((keyState & FIRE_PRESSED) != 0) {
                this.stop();
            }
            Graphics g=this.getGraphics();
            g.setColor(255, 255, 255);
            g.fillRect(60, 10, 100, 20);
            g.setColor(0, 0, 0);
            g.drawString("Waiting:" +i++, 100, 20, g.BASELINE | g.HCENTER);
            repaintOnIteration();
        }
    }

}
