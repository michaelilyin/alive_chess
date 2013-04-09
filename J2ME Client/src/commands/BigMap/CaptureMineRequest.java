/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.Commands;
import Serializer.Utils.*;
//import Commands.Util.*;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class CaptureMineRequest implements IProtoSerializableRequest {
    private int com_id;
    private int mine_id;

    public CaptureMineRequest(){
        com_id = Commands.CAPTURE_MINE_REQUEST;
    }

    public void setMineId(int value){
        mine_id = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, mine_id);
    }

    public byte[] toByte() throws IOException{
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, mine_id);
        return result;
    }
}
