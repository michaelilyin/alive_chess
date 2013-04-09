/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package world;

//import commands.Util.DefaultCommandSender;
//import commands.Util.ICommandListener;
//import commands.Util.Command;
import java.io.IOException;
import obj.*;
import Logic.Location;
//import commands.*;
import gui.SpriteAnimationTask;
import java.util.Vector;

/**
 *
 * @author е
 */
public class WorldContext{ //extends DefaultCommandSender implements ICommandListener {

    private int SizeX;
    private int SizeY;

    private FieldLoader fl;

    private Vector Heroes;
    private Vector Objects;
    private Player player;

    private static WorldContext instance;

    protected WorldContext() {
//        try {
            fl = new FieldLoader();
            player = new Player("tester", "testpass");
            player.addHero(ObjFactory.getInstance().createHero("TestHero", new Location(5, 3), 0));
            SizeX = SizeY = 2; //это должно быть результатом update command
            Heroes = new Vector();
            Objects = new Vector();
            Heroes.addElement(player.getMainHero());

            Hero h = ObjFactory.getInstance().createHero("TestNonPlayerHero", new Location(10, 10), 1);
            Heroes.addElement(h);
            h.getAnimator().setMoving(true);
            Objects.addElement(ObjFactory.getInstance().createCastle("TestCastle", new Location(8, 6), 1));
            Objects.addElement(ObjFactory.getInstance().createCastle("SandCastle", new Location(26, 2), 0));
            Objects.addElement(ObjFactory.getInstance().createBottle(new Location(7, 1)));
            Objects.addElement(ObjFactory.getInstance().createRoadSign(new Location(9, 9)));
            //fireUpdate();
//        } catch (IOException ex) {
//            ex.printStackTrace();
//        }

    }

    public static WorldContext getInstance() {
        if (instance == null)
            instance = new WorldContext();
        return instance;
    }

    /**
     * @return the fl
     */
    public FieldLoader getFieldLoader() {
        return fl;
    }

    /**
     * Послать сообщение Update слушателям данного класса
     */
    public void fireUpdate(Object cmd) {
        System.out.println("Update fired. There will be socket transmitting.");
        //fireCommand(cmd);
    }

    /**
     * обработка пришедшего сообщения данным классом
     * @param e
     */
    public void commandFired(Object e) {
        //e.execute();
    }

    /**
     * @return the WorldSize
     */
    public int getSizeX() {
        return SizeX;
    }

    /**
     * @return the WorldSizeY
     */
    public int getSizeY() {
        return SizeY;
    }

    /**
     * @return the Heroes
     */
    public Vector getAnimatedObjects() {
        return Heroes;
    }

    /**
     * @return the Castles
     */
    public Vector getLayeredObjects() {
        return Objects;
    }

    /**
     * @return the player
     */
    public Player getPlayer() {
        return player;
    }

}
