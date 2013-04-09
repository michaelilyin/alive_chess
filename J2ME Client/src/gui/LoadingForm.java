/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui;

import javax.microedition.lcdui.*;

/**
 *
 * @author OLEG
 */
public class LoadingForm extends Form {

    private TextBox textBox;
private StringItem s;
    public LoadingForm(final Thread thr) {
        super("Loading...");
        try {
            //textBox = new TextBox(title, text, maxSize, constraints)
            s = new StringItem("Loading", ".....");
            this.append(s);
        } catch (Exception exc) {
            System.out.println(exc.getMessage());
        }
        this.addCommand(new Command("Cancel", Command.ITEM, 1));
        this.setCommandListener(new CommandListener() {

            public void commandAction(Command c, Displayable d) {
                if ("Cancel".equals(c.getLabel())) {
                    thr.interrupt();
                }
            }
        });
    }
}
