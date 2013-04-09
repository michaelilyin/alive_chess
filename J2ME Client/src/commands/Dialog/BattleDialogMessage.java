/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Dialog;


import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IDialogCommandListener;
import Serializer.Utils.IProtoDeserializable;
import Serializer.Utils.IProtoSerializableRequest;

/**
 *
 * @author Admin
 */
public class BattleDialogMessage extends Message implements ICommand {

    public BattleDialogMessage(){
        super();
        com_id = Commands.BATTLE_DIALOG_MESSAGE;
    }

    public void Execute(Object listener) {
        IDialogCommandListener l = (IDialogCommandListener)listener;
        l.BattleDialogMessReceived(this);
    }


}
