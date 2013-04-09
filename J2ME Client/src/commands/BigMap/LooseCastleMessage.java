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
public class LooseCastleMessage implements IProtoSerializableRequest, IProtoDeserializable, ICommand {
    private int com_id;
    private int castleId;       //proto 1

    public LooseCastleMessage(){ com_id = Commands.LOOSE_CASTLE_MESSAGE; }

    public int getCastleId(){return castleId;}
    public void setCastleId(int value){castleId = value;}

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, castleId);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, castleId);
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        castleId = dsr.readInt(1);
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener)listener;
        l.LooseCastleMessReceived(this);
    }


}
