/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Statistic;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IStatisticCommandListener;
import Objects.WorldDescription;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class GetAvailableMapsResponse implements IProtoDeserializable, ICommand {
    private Vector worlds;      //proto 1   //array of <WorldDescription> objects

    public GetAvailableMapsResponse(){}

    public Vector getWorlds(){return worlds;}
    public void setWorlds(Vector value){worlds = value;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        worlds = new Vector();
        int res = dsr.readMessage(1);
        while(res != -1){
            WorldDescription wd = new WorldDescription();
            wd.LoadFrom(dsr.getMessageBytes());
            worlds.addElement(wd);
            res = dsr.readMessage(2);
        }
    }

    public void Execute(Object listener) {
        IStatisticCommandListener l = (IStatisticCommandListener)listener;
        l.GetAvailableMapsResponseReceived(this);
    }


}
