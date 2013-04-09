/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Castle;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.ICastleCommandListener;
import Objects.Unit;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class BuyFigureResponse implements IProtoDeserializable, ICommand{
    private Vector units;

    public BuyFigureResponse(){}

    public Vector getUnits(){return units;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        units = new Vector();
        int res = dsr.readMessage(1);
        while(res != -1){
            Unit u = new Unit();
            u.LoadFrom(dsr.getMessageBytes());
            units.addElement(u);
            res = dsr.readMessage(1);
        }
    }

    public void Execute(Object listener) {
        ICastleCommandListener l = (ICastleCommandListener)listener;
        l.BuyFigureResponseReceived(this);
    }


}
