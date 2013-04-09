/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.Castle;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class CaptureCastleResponse implements IProtoDeserializable, ICommand {
    private Castle castle;

    public CaptureCastleResponse(){ }

    public Castle getCastle(){return castle;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        if (res == -1){
            castle = null;
            return;
        }
        castle = new Castle();
        castle.LoadFrom(dsr.getMessageBytes());
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener) listener;
        l.CaptureCastleResponseReceived(this);
    }


}
