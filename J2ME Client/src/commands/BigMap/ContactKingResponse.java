/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.Dialog;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class ContactKingResponse implements IProtoDeserializable, ICommand {

    private Dialog dialog;

    public ContactKingResponse(){}

    public Dialog getDialog(){return dialog;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        if (res == -1){
            dialog = null;
            return;
        }
        dialog = new Dialog();
        dialog.LoadFrom(dsr.getMessageBytes());
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener)listener;
        l.ContactKingResponseReceived(this);
    }



}
