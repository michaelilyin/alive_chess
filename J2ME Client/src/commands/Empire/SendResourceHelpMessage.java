/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IEmpireCommandListener;
import Objects.Resource;
import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoDeserializable;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class SendResourceHelpMessage implements IProtoSerializableRequest, IProtoDeserializable, ICommand {
    private int com_id;
    private int receiverId;         //proto 1
    private Vector resources;       //proto 2   //array of <Resource> objects
    public SendResourceHelpMessage(){com_id = Commands.SEND_RESOURCE_HELP_MESSAGE;}

    public int getReceiverId(){return receiverId;}
    public void setReceiverId(int value){receiverId = value;}
    public Vector getResources(){return resources;}
    public void setResources(Vector value){resources = value;}

    public int ComputeSize() {
        int result = 0;
        result += ComputeSizeUtil.ComputeInt(1, receiverId);
        for (int i=0; i<resources.size(); i++){
            Resource r = (Resource)resources.elementAt(i);
            int size = r.ComputeSize();
            result += ComputeSizeUtil.ComputeMessage(2, size) + size;
        }
        return result;
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, receiverId);
        for(int i=0; i<resources.size(); i++){
            Resource r = (Resource)resources.elementAt(i);
            sr.SerializeMessage(2, r.ComputeSize());
            r.WriteFields(sr);
        }
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        receiverId = dsr.readInt(1);
        resources = new Vector();
        int res = dsr.readMessage(2);
        while (res != -1){
            Resource r = new Resource();
            r.LoadFrom(dsr.getMessageBytes());
            resources.addElement(r);
            res = dsr.readMessage(2);
        }
    }

    public void Execute(Object listener) {
        IEmpireCommandListener l = (IEmpireCommandListener)listener;
        l.SendResourceHelpMessReceived(this);
    }


}
