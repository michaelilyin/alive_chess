/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.Commands;
import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class StartVoteRequest implements IProtoSerializableRequest {

    private int com_id;
    private String message;

    public StartVoteRequest(){
        com_id = Commands.START_VOTE_REQUEST;
    }

    public void setMessage(String value){
        message = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeString(1, message);
    }

    public byte[] toByte() throws IOException{
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeString(1, message);
        return result;
    }

}
