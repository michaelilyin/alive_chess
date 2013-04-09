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
public class SendContactListCommand implements IProtoSerializableRequest{
    private int com_id;
    private int[] ids;
    private String[] logins;

    public SendContactListCommand(){
        com_id = Commands.SEND_CONTACT_LIST_COMMAND;
    }

    public void setIds(int[] values){
        ids = values;
    }
    public void setLogins(String[] values){
        logins = values;
    }

    public int ComputeSize() {
        int size = 0;
        for (int i=0; i<ids.length; i++){
            size+=ComputeSizeUtil.ComputeInt(1, ids[i]);
        }
        for (int i=0; i<logins.length; i++){
            size+=ComputeSizeUtil.ComputeString(2, logins[i]);
        }
        return size;
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        for(int i=0; i<ids.length; i++){
            sr.SerializeInt(1, ids[i]);
        }
        for(int i=0; i<logins.length; i++){
            sr.SerializeString(1, logins[i]);
        }
        return result;
    }


}
