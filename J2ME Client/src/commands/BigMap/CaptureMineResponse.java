/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.Mine;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class CaptureMineResponse implements IProtoDeserializable, ICommand {

    private Mine mine;

    public CaptureMineResponse(){}

    public Mine getMine(){return mine;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        if (res == -1){
            mine = null;
            return;
        }
        mine = new Mine();
        mine.LoadFrom(dsr.getMessageBytes());
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener) listener;
        l.CaptureMineResponseReceived(this);
    }


}
