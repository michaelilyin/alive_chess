/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.King;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class GetKingResponse implements IProtoDeserializable, ICommand {

    private King king;

    public GetKingResponse(){}

    public King getKing(){return king;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        if (res == -1){
            king = null;
            return;
        }
        king = new King();
        king.LoadFrom(dsr.getMessageBytes());
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener) listener;
        l.GetKingResponseReceived(this);
    }


}
