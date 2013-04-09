/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.Resource;
import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoDeserializable;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class GetResourceMessage implements IProtoSerializableRequest, IProtoDeserializable, ICommand {

    private int com_id;
    private Resource resource;  //proto 1
    private boolean fromMine;   //proto 2

    public GetResourceMessage(){com_id = Commands.GET_RESOURCE_MESSAGE;}

    public Resource getResource(){return resource;}
    public void setResource(Resource value){resource = value;}
    public boolean getFromMine(){return fromMine;}
    public void setFromMine(boolean value){fromMine = value;}

    public int ComputeSize() {
        int result = 0;
        if (resource != null){
            int s = resource.ComputeSize();
            result += s + ComputeSizeUtil.ComputeMessage(1, s);
        }
        result += ComputeSizeUtil.ComputeBool(2, fromMine);
        return result;
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        if (resource != null){
            len = resource.ComputeSize();
            sr.SerializeMessage(1, len);
            resource.WriteFields(sr);
        }
        sr.SerializeBool(2, fromMine);
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res= dsr.readMessage(1);
        if (res == -1) resource = null;
        else{
            resource = new Resource();
            resource.LoadFrom(dsr.getMessageBytes());
        }
        fromMine = dsr.readBool(2);
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener)listener;
        l.GetResourceMessReceived(this);
    }


}
