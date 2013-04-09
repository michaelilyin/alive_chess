/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IEmpireCommandListener;
import Objects.Resource;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class GetHelpResourceResponse implements IProtoDeserializable, ICommand {
    private Vector resources;       //proto 1   //array of <Resource> objects

    public GetHelpResourceResponse(){}

    public Vector getResources(){return resources;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        resources = new Vector();
        int res = dsr.readMessage(1);
        while(res != -1){
            Resource r = new Resource();
            r.LoadFrom(dsr.getMessageBytes());
            resources.addElement(r);
            res = dsr.readMessage(1);
        }
    }

    public void Execute(Object listener) {
        IEmpireCommandListener l = (IEmpireCommandListener)listener;
        l.GetHelpResourceResponseReceived(this);
    }


}
