/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Dialog;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IDialogCommandListener;

/**
 *
 * @author Admin
 */
public class WarDialogMessage extends Message implements ICommand {
    public WarDialogMessage(){
        super();
        com_id = Commands.WAR_DIALOG_MESSAGE;
    }

    public void Execute(Object listener) {
        IDialogCommandListener l = (IDialogCommandListener)listener;
        l.WarDialogMessReceived(this);
    }


}
