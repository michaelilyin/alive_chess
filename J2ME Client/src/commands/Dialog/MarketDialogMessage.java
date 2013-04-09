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
public class MarketDialogMessage extends Message implements ICommand {
    public MarketDialogMessage(){
        super();
        com_id = Commands.MARKET_DIALOG_MESSAGE;
    }

    public void Execute(Object listener) {
        IDialogCommandListener l = (IDialogCommandListener)listener;
        l.MarketDialogMessReceived(this);
    }


}
