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
public class GetArmyCastleToKingResponse implements IProtoDeserializable, ICommand {
    
    private Vector army;        //proto 1   //array of <Unit> objects

    public GetArmyCastleToKingResponse(){}

    public Vector getArmy(){return army;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        army = new Vector();
        int res = dsr.readMessage(1);
        while(res != -1){
            Unit u = new Unit();
            u.LoadFrom(dsr.getMessageBytes());
            army.addElement(u);
            res = dsr.readMessage(1);
        }
    }

    public void Execute(Object listener) {
        ICastleCommandListener l = (ICastleCommandListener)listener;
        l.GetArmyCastleToKingResponseReceived(this);
    }


}
