/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IEmpireCommandListener;
import Objects.MemberInfo;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class GetAlianceInfoResponse implements IProtoDeserializable, ICommand{

    private int unionId;            //proto 1
    private Vector members;         //proto 2 //array of <MemberInfo> objects

    public GetAlianceInfoResponse(){}

    public int getUnionId(){return unionId;}
    public Vector getMembers(){return members;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        unionId = dsr.readInt(1);
        members = new Vector();
        int res = dsr.readMessage(2);
        while(res != -1){
            MemberInfo mi = new MemberInfo();
            mi.LoadFrom(dsr.getMessageBytes());
            members.addElement(mi);
            res = dsr.readMessage(2);
        }
    }

    public void Execute(Object listener) {
        IEmpireCommandListener l = (IEmpireCommandListener)listener;
        l.GetAlianceInfoResponseReceived(this);
    }


}
