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
public class JoinLeaveCommand implements IProtoSerializableRequest {

    private int com_id;
    private int id_client;
    private byte conordisc;

    public JoinLeaveCommand(){
        com_id = Commands.JOIN_LEAVE_COMMAND;
    }

    public void setIdClient(int value){
        id_client = value;
    }

    public void setConOrDisc(byte value){
        conordisc = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, id_client)+
               ComputeSizeUtil.ComputeInt(2, (int)conordisc);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, id_client);
        sr.SerializeInt(2, (int)conordisc);
        return result;
    }


}
