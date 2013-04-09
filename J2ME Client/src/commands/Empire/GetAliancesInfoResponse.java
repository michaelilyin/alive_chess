/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IEmpireCommandListener;
import Objects.AlianceInfo;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class GetAliancesInfoResponse implements IProtoDeserializable, ICommand {

    private Vector aliances;        //proto 1   //array of <AlianceInfo> objects

    public GetAliancesInfoResponse(){}

    public Vector getAliances(){return aliances;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        aliances = new Vector();
        int res = dsr.readMessage(1);
        while(res != -1){
            AlianceInfo ai = new AlianceInfo();
            ai.LoadFrom(dsr.getMessageBytes());
            aliances.addElement(ai);
            res = dsr.readMessage(1);
        }
    }

    public void Execute(Object listener) {
        IEmpireCommandListener l = (IEmpireCommandListener)listener;
        l.GetAliancesInfoResponseReceived(this);
    }


}
