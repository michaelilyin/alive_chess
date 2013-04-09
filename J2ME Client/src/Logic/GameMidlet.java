package Logic;

import gui.*;
import obj.Player;
//import commands.Util.ICommandListener;
import gui.WaitingScreenCanvas;
import java.util.Vector;
import javax.microedition.midlet.*;
import javax.microedition.lcdui.*;
//import net.SocketTransport;
//import net.StubTransport;
import net.SocketTransport;
import world.WorldContext;

/**
 * MIDlet creates, runs and displays StrategyGameCanvas.
 *
 * @author  e
 * @version 1.0
 */
public class GameMidlet extends MIDlet implements CommandListener {

    //private StrategyGameCanvas strategyCanvas;
    private BattleCanvas battleCanvas;
    private CastleCanvas castleCanvas; //сворачиваемая и постоянно запущена? или запускается по требованию?
    private MenuForm Menu;
    private SettingForm Setting;
    private Thread tGame;//, tBattle, tCastle;
    private Display d;
    private Vector battleCanvasVector; //предполагается ввести возможность ведения нескольких битв одновременно
    private Vector battleThreadsVector;
    private final GameMidlet mySelf = this;
    WorldCanvas WorldCanvas;
    WorldContext World;
    Player Player;

    public void startApp() {
        battleCanvasVector = new Vector();

        Menu = new MenuForm();
        Menu.setCommandListener(this);
        Setting = new SettingForm();
        Setting.setCommandListener(this);
//        strategyCanvas = new StrategyGameCanvas(this);
//        strategyCanvas.setCommandListener(this);
//        this.tGame = new Thread(strategyCanvas);

        d = Display.getDisplay(this);
        //при переключении форма-меню-форма поток не останавливается.
        //viewBattle();
        //viewGame();


        Player = new Player("tester", "testpass");

        viewMenu();
    }

    public void pauseApp() {
    }

    public void destroyApp(boolean unconditional) {
        //this.strategyCanvas.stop();
    }

    // handle commands
    public void commandAction(Command com, Displayable dis) {

        String label = com.getLabel();

        if ("EXIT".equals(label)) {
            notifyDestroyed();
        } else if ("HELP".equals(label)) {
            viewHelp();
        } else if ("PAUSE".equals(label)) {
            notifyPaused();
        } else if ("MENU".equals(label)) {
            viewMenu();
        } //        else if("GAME".equals(label))
        //            viewGame();
        //        else if("SCREEN".equals(label))
        //            switchScreens();
        else if ("Connect".equals(label)) {
            connect();
        } else if (("Setting").equals(label)) {
            viewSetting();
        }
    }

    public void viewHelp() {
        Alert HelpAlert = new Alert("Help", " It will be in a short time!", null, AlertType.INFO);
        d.setCurrent(HelpAlert, d.getCurrent());
    }

//    public void viewGame() {
//        System.out.println("Viewgame");
//        if (!tGame.isAlive())
//            tGame.start();
//        d.setCurrent(strategyCanvas);
//	}
    public void viewBattle() {
        System.out.println("viewBattle");
        battleCanvas = new BattleCanvas(this);
        viewCanvasInOwnThread(battleCanvas);
    //viewGame();
    //strategyCanvas.AfterBattleEnd(battleCanvas.isPlayerWon());
    }

    public void viewCastle() {
        System.out.println("viewCastle");
        castleCanvas = new CastleCanvas(this);
        viewCanvasInOwnThread(castleCanvas);
    //viewGame();
    }

    public void viewCanvas(ExtGameCanvas canvas) {
        canvas.setCommandListener(this);
        d.setCurrent(canvas);
        Thread t = new Thread(canvas);
        t.start();
    }

    public void viewCanvasInOwnThread(ExtGameCanvas canvas) {
        canvas.setCommandListener(this);
        d.setCurrent(canvas);
        try {
            Thread t = new Thread(canvas);
            t.start();
        } catch (IllegalThreadStateException ex) {
            ex.printStackTrace();
        }

        try {
            canvas.waitForInterrupted();
        } catch (InterruptedException ex) {
            ex.printStackTrace();
        } catch (IllegalMonitorStateException ex) {
            ex.printStackTrace();
        }
        System.out.println("Waiting ends");
    }

//    protected void viewCanvasInOwnThread(ExtGameCanvas canvas, ExtGameCanvas next) {
//        canvas.setCommandListener(this);
//        d.setCurrent(canvas);
//        try {
//            Thread t = new Thread(canvas);
//            t.start();
//        } catch (IllegalThreadStateException ex) {
//            ex.printStackTrace();
//        }
//
//        try {
//            canvas.waitForInterrupted();
//        } catch (InterruptedException ex) {
//            ex.printStackTrace();
//        } catch (IllegalMonitorStateException ex) {
//            ex.printStackTrace();
//        }
//        System.out.println("Waiting ends");
//        viewCanvas(next);
//    }
    public void viewMenu() {
        System.out.println("viewMenu Called");
        d.setCurrent(Menu);
    }

    public void viewSetting() {
        d.setCurrent(Setting);
    }

    /**
     * По задумке, это будет аналог alt+tab для переключения между картой, запущенными сражениями
     * и даже замком о_0
     */
    public void switchScreens() {
    }

    public void connect() {
        new Thread() {

            public void run() {
                //StubTransport transport = StubTransport.getInstance();
                try {
                    final SocketTransport t = new SocketTransport();
                    Thread thr = new Thread(new Runnable() {

                        public void run() {
                            t.Connect("127.0.0.1", 22000);
                        }
                    });
                    thr.start();
                    //Thread thr = transport.connect(Setting.getLogin(), Setting.getPassword());
                    LoadingForm loadingForm = new LoadingForm(thr);
                    d.setCurrent(loadingForm);
                    World = WorldContext.getInstance();
                    // пока заглушка
                    //transport.getWorld_sender().addCommandListener(World);
                    //transport.addCommandListener(World);
                    //World.addCommandListener(transport);
                    //World.fireUpdate();
                    WorldCanvas = new WorldCanvas(mySelf, World);
                    //thr.join();
                } catch (Exception exc) {
                    System.out.println("!!!!!!!!!!!!!!!!!!kljnkjnkjnkj");
                }
                //if (!transport.isShutdown()) {
                    viewCanvas(WorldCanvas);
                //} else {
                    //d.setCurrent(Menu);
                //}
            }
        }.start();
    }
}