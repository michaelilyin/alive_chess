/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui;

import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Image;
import javax.microedition.lcdui.ImageItem;
import javax.microedition.lcdui.StringItem;
import javax.microedition.lcdui.Ticker;

/**
 *
 * @author е
 */
public class MenuForm extends Form {

    Ticker ticker;
    ImageItem imageItem;
    StringItem stringItem;

    public MenuForm() {
        super("Alive Chess Start Menu");

        ticker = new Ticker("Greetings, chess player!");
        this.setTicker(ticker);

        try {
            imageItem = new ImageItem(
                    null,
                    Image.createImage("/logo.png"),
                    ImageItem.LAYOUT_DEFAULT,
                    null);
            this.append(imageItem);
        } catch (Exception ex) {
            System.out.println(ex.getMessage());
        }

        stringItem = new StringItem("Description: ", "Turn Based Strategy On-Line Game");
        this.append(stringItem);

        this.addCommand(
                new Command("EXIT", Command.EXIT, 2));
        this.addCommand(
                new Command("HELP", Command.HELP, 2));
        //this.addCommand(
        //	new Command("GAME", Command.SCREEN, 1));
        this.addCommand(
                new Command("Connect", Command.ITEM, 4));
        this.addCommand(
                new Command("Setting", Command.ITEM, 5));
    }
}
