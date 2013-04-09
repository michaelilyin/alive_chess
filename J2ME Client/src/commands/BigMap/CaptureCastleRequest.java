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
public class CaptureCastleRequest implements IProtoSerializableRequest {

    private int com_id;
    private int castle_id;
    
    public CaptureCastleRequest(){
        com_id = Commands.CAPTURE_CASTLE_REQUEST;
    }
    
    public void setCastleId(int value){
        castle_id = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, castle_id);
    }

    public byte[] toByte() throws IOException{
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, castle_id);
        return result;
    }


}
