/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IEmpireCommandListener;
import Objects.Negotiate;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class StartNegotiateResponse implements IProtoDeserializable, ICommand {
    private Negotiate discussion;   //proto 1

    public StartNegotiateResponse(){}

    public Negotiate getDiscussion(){return discussion;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        if (res == -1){
            discussion = null;
            return;
        }
        discussion = new Negotiate();
        discussion.LoadFrom(dsr.getMessageBytes());
    }

    public void Execute(Object listener) {
        IEmpireCommandListener l = (IEmpireCommandListener)listener;
        l.StartNegotiateResponseReceived(this);
    }


}
