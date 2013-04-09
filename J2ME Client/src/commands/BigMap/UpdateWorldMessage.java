/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.FPosition;
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
public class UpdateWorldMessage implements IProtoSerializableRequest, IProtoDeserializable, ICommand {
    private int com_id;
    private int objectId;              //proto 1
    private FPosition location;        //proto 2
    private int updateType;            //proto 3
        //KingMove          = 0, // король переместился
        //KingAppear        = 1, // король появился
        //KingDisappear     = 2, // король исчез
        //ResourceDisappear = 3  // ресурс исчез

    public UpdateWorldMessage(){com_id = Commands.UPDATE_WORLD_MESSAGE;}

    public int getObjectId(){return objectId;}
    public void setObjectId(int value){objectId = value;}
    public FPosition getLocation(){return location;}
    public void setLocation(FPosition value){location = value;}
    public int getUpdateType(){return updateType;}
    public void setUpdateType(int value){updateType = value;}

    public int ComputeSize() {
        int result = 0;
        result += ComputeSizeUtil.ComputeInt(1, objectId);
        if (location != null){
            int size = location.ComputeSize();
            result += ComputeSizeUtil.ComputeMessage(2, size) + size;
        }
        result += ComputeSizeUtil.ComputeInt(3, updateType);
        return result;
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, objectId);
        if (location != null) {
            sr.SerializeMessage(2, location.ComputeSize());
            location.WriteFields(sr);
        }
        sr.SerializeInt(3, updateType);
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        objectId = dsr.readInt(1);
        int res = dsr.readMessage(2);
        if (res == -1) location = null;
        else{
            location = new FPosition();
            location.LoadFrom(dsr.getMessageBytes());
        }
        updateType = dsr.readInt(3);
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener)listener;
        l.UpdateWorldMessReceived(this);
    }


}
