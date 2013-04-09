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
public class MessageReceiveCommand implements IProtoSerializableRequest{
    private int com_id;
    private String messagetext;
    private byte messagetype;
    private int id_sender;
    private String login_sender;

    public MessageReceiveCommand(){
        com_id = Commands.MESSAGE_RECEIVE_COMMAND;
    }

    public void setMessageText(String value){
        messagetext = value;
    }
    public void setMessageType(byte value){
        messagetype = value;
    }
    public void setIdSender(int value){
        id_sender = value;
    }
    public void setLoginSender(String value){
        login_sender = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeString(1, messagetext)+
               ComputeSizeUtil.ComputeInt(2, (int)messagetype)+
               ComputeSizeUtil.ComputeInt(3, id_sender)+
               ComputeSizeUtil.ComputeString(4, login_sender);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeString(1, messagetext);
        sr.SerializeInt(2, (int)messagetype);
        sr.SerializeInt(3, id_sender);
        sr.SerializeString(4, login_sender);
        return result;
    }


}
