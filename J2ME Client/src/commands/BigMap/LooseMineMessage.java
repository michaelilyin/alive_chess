/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package commands.BigMap;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
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
public class LooseMineMessage implements IProtoSerializableRequest, IProtoDeserializable, ICommand {
    private int com_id;
    private int mineId;       //proto 1

    public LooseMineMessage(){ com_id = Commands.LOOSE_MINE_MESSAGE; }

    public int getMineId(){return mineId;}
    public void setMineId(int value){mineId = value;}

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, mineId);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, mineId);
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        mineId = dsr.readInt(1);
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener)listener;
        l.LooseMineMessReceived(this);
    }


}
