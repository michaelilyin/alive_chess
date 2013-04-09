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
public class TakeResourceMessage implements IProtoSerializableRequest, IProtoDeserializable, ICommand{

    private int com_id;
    private Resource resource;

    public TakeResourceMessage(){com_id = Commands.TAKE_RESOURCE_MESSAGE;}

    public Resource getResource(){return resource;}
    public void setResource(Resource value){resource = value;}

    public int ComputeSize() {
        int result = 0;
        if (resource != null){
            int size = resource.ComputeSize();
            result += ComputeSizeUtil.ComputeMessage(1, size) + size;
        }
        return result;
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        if (resource != null){
            sr.SerializeMessage(1, resource.ComputeSize());
            resource.WriteFields(sr);
        }
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        if (res == -1) {
            resource = null;
            return;
        }
        resource = new Resource();
        resource.LoadFrom(dsr.getMessageBytes());
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener)listener;
        l.TakeResourceMessReceived(this);
    }


}
