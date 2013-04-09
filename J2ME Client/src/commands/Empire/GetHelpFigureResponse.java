/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IEmpireCommandListener;
import Objects.Unit;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class GetHelpFigureResponse implements IProtoDeserializable, ICommand {
    private Vector units;       //proto 1   //array of <Unit> objects

    public GetHelpFigureResponse(){}

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
        IEmpireCommandListener l = (IEmpireCommandListener)listener;
        l.GetHelpFigureResponseReceived(this);
    }


}
