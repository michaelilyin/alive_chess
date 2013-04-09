/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;
import commands.Util.Commands;
import Serializer.Utils.*;
import java.io.IOException;
//import Commands.Util.*;

/**
 *
 * @author Admin
 */
public class GetKingRequest implements IProtoSerializableRequest {

    private int com_id;
    private int king_id;

    public GetKingRequest(){
        com_id = Commands.GET_KING_REQUEST;
    }

    public void setKingId(int value){
        king_id = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, king_id);
    }

    public byte[] toByte() throws IOException{
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, king_id);
        return result;
    }
}
