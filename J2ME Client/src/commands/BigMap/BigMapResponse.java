/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class BigMapResponse implements IProtoDeserializable, ICommand {

    private boolean isAllowed;

    public boolean getAllowedStatus(){
        return isAllowed;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        isAllowed = dsr.readBool(1);
    }

    public void Execute(Object listener) {
        IBigMapCommandListener  l = (IBigMapCommandListener) listener;
        l.BigMapResponseReceived(this);
    }


}
