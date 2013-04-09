/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Authorization;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IAuthorizationCommandListener;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class RegisterResponse implements IProtoDeserializable, ICommand {

    private boolean isSuccessed;

    public RegisterResponse() {}

    public boolean getIsSucceed(){
        return isSuccessed;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        isSuccessed = dsr.readBool(1);
    }

    public void Execute(Object listener) {
        IAuthorizationCommandListener l = (IAuthorizationCommandListener)listener;
        l.RegisterResponseReceived(this);
    }



}
