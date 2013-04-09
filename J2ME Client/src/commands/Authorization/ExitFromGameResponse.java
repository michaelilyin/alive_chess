/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Authorization;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IAuthorizationCommandListener;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class ExitFromGameResponse implements IProtoDeserializable, ICommand {

    public ExitFromGameResponse() {
    }

    public void LoadFrom(byte[] buffer) throws IOException {

    }

    public void Execute(Object listener) {
        IAuthorizationCommandListener l = (IAuthorizationCommandListener) listener;
        l.ExitFromGameResponseReceived(this);
    }


}
