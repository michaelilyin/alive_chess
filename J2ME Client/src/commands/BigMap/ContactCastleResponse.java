/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.Castle;
import Objects.Dialog;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class ContactCastleResponse implements IProtoDeserializable, ICommand {

    private Dialog dispute;
    private Castle castle;

    public ContactCastleResponse(){}

    public Dialog getDispute(){return dispute;}
    public Castle getCastle(){return castle;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        if (res == -1) dispute = null;
        else {
            dispute = new Dialog();
            dispute.LoadFrom(dsr.getMessageBytes());
        }
        res = dsr.readMessage(2);
        if (res == -1) castle = null;
        else {
            castle = new Castle();
            castle.LoadFrom(dsr.getMessageBytes());
        }
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener)listener;
        l.ContactCastleResponseReceived(this);
    }


}
