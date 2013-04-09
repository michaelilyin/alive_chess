/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.King;
import Objects.Resource;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class GetObjectsResponse implements IProtoDeserializable, ICommand {

    private Vector resources;       //proto 1  array of <Resource> objects
    private Vector kings;           //proto 2  array of <King> objects

    public GetObjectsResponse(){}

    public Vector getResources(){return resources;}
    public Vector getKings(){return kings;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        resources = new Vector();
        int res = dsr.readMessage(1);
        while (res != -1){
            Resource r = new Resource();
            r.LoadFrom(dsr.getMessageBytes());
            resources.addElement(r);
            res = dsr.readMessage(1);
        }
        kings = new Vector();
        res = dsr.readMessage(2);
        while(res != -1){
            King k = new King();
            k.LoadFrom(dsr.getMessageBytes());
            kings.addElement(k);
            res = dsr.readMessage(2);
        }
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener)listener;
        l.GetObjectsResponseReceived(this);
    }


}
