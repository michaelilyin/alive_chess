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
public class CapitulateDialogMessage extends Message implements ICommand {
    public  CapitulateDialogMessage(){
        super();
        com_id = Commands.CAPITULATE_DIALOG_MESSAGE;
    }

    public void Execute(Object listener) {
        IDialogCommandListener l = (IDialogCommandListener)listener;
        l.CapitulateDialogMessReceived(this);
    }


}
