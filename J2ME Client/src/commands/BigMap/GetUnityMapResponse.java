/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class GetUnityMapResponse implements IProtoDeserializable, ICommand{

    private byte[] array;

    public GetUnityMapResponse(){}

    public byte[] getArray(){return array;}

    public void LoadFrom(byte[] buffer) throws IOException {
        array = buffer;
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener) listener;
        l.GetUnityMapResponseReceived(this);
    }


}
