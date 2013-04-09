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
public class IncludeKingInEmpireRequest implements IProtoSerializableRequest {

    private int com_id;
    private int king_id;

    public IncludeKingInEmpireRequest(){
        com_id = Commands.INCLUDE_KING_IN_EMPIRE_REQUEST;
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
