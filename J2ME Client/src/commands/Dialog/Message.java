/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Dialog;

import commands.Util.Commands;
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
public class Message implements IProtoDeserializable, IProtoSerializableRequest{
    protected int com_id;
    protected int type;           //proto 1
        //NoState = 0, // состояние не определено
        //Agree   = 1, // согласие
        //Refuse  = 2, // отказ
        //Offer   = 3, // предложение
        //Coerce  = 4, // принудить
    protected int disputeId;      //proto 2

    protected Message(){}

    public int getType(){return type;}
    public void setType(int value){type = value;}
    public int getDisputeId(){return disputeId;}
    public void setDisputeId(int value){disputeId = value;}

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, type) + ComputeSizeUtil.ComputeInt(2, disputeId);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, type);
        sr.SerializeInt(2, disputeId);
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        type = dsr.readInt(1);
        disputeId = dsr.readInt(2);
    }
}
