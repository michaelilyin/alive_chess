/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Castle;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.ICastleCommandListener;
import Objects.InnerBuilding;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class GetListBuildingsInCastleResponse implements IProtoDeserializable, ICommand {
    private Vector buildings;       //proto 1   //array of <InnerBuilding> objects

    public GetListBuildingsInCastleResponse(){}

    public Vector getBuildings(){return buildings;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        buildings = new Vector();
        int res = dsr.readMessage(1);
        while (res != -1){
            InnerBuilding ib = new InnerBuilding();
            ib.LoadFrom(dsr.getMessageBytes());
            buildings.addElement(ib);
            res = dsr.readMessage(1);
        }
    }

    public void Execute(Object listener) {
        ICastleCommandListener l = (ICastleCommandListener)listener;
        l.GetListBuildingsInCastleResponseReceived(this);
    }


}
