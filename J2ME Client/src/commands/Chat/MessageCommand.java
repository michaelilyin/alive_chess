/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Chat;

import commands.Util.Commands;
import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class MessageCommand implements IProtoSerializableRequest {

    private int com_id;
    private String message;
    private byte msgtype;
    private int id_getter;
    private int id_sender;

    public MessageCommand(){
        com_id = Commands.MESSAGE_COMMAND;
    }

    public void setMessage(String value){
        message = value;
    }
    public void setMessType(byte value){
        msgtype = value;
    }
    public void setGetterId(int value){
        id_getter = value;
    }
    public void setSenderId(int value){
        id_sender = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeString(1, message)+
               ComputeSizeUtil.ComputeInt(2, (int)msgtype)+
               ComputeSizeUtil.ComputeInt(3, id_getter)+
               ComputeSizeUtil.ComputeInt(4, id_sender);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeString(1, message);
        sr.SerializeInt(2, (int)msgtype);
        sr.SerializeInt(3, id_getter);
        sr.SerializeInt(4, id_sender);
        return result;
    }


}
