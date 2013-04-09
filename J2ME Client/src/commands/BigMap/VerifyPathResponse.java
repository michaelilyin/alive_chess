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
public class VerifyPathResponse implements IProtoDeserializable, ICommand {

    private float x;
    private float y;

    public VerifyPathResponse(){}

    public float getX(){return x;}
    public float getY(){return y;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        x = dsr.readFloat(1);
        y = dsr.readFloat(2);
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener)listener;
        l.VerifyPathResponseReceived(this);
    }


}
