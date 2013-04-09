/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.FPosition;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class ComputePathResponse implements IProtoDeserializable, ICommand {

    private Vector path;

    public ComputePathResponse(){}

    public Vector getPath() {return path;}

    public void LoadFrom(byte[] buffer) throws IOException {
        path = new Vector();
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        while (res != -1){
            FPosition pos = new FPosition();
            pos.LoadFrom(dsr.getMessageBytes());
            path.addElement(pos);
            res = dsr.readMessage(1);
        }
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener)listener;
        l.ComputePathResponseReceived(this);
    }


}
