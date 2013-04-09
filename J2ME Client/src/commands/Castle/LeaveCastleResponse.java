/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Castle;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.ICastleCommandListener;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class LeaveCastleResponse implements IProtoDeserializable, ICommand {
    public LeaveCastleResponse(){}

    public void LoadFrom(byte[] buffer) throws IOException {
    }

    public void Execute(Object listener) {
        ICastleCommandListener l = (ICastleCommandListener)listener;
        l.LeaveCastleResponseReceived(this);
    }


}
