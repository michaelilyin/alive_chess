/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.Commands;
import Serializer.Utils.*;
//import Commands.Util.*;
import java.io.IOException;
import Objects.FPosition;

/**
 *
 * @author Admin
 */
public class ComputePathRequest implements IProtoSerializableRequest {

    private int com_id;
    private FPosition p1;
    private FPosition p2;
    
    public ComputePathRequest(){
        com_id = Commands.COMPUTE_PATH_REQUEST;
    }
    
    public void setStart(FPosition p){
        p1 = p;
    }
    public void setFin(FPosition p){
        p2 = p;
    }

    public int ComputeSize() {
        int result = 0;
        if (p1 != null){
            result += ComputeSizeUtil.ComputeMessage(1, p1.ComputeSize())+
            p1.ComputeSize();
        }
        if (p2 != null){
            result += ComputeSizeUtil.ComputeMessage(2, p2.ComputeSize())+
            p2.ComputeSize();
        }
        return result;
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        if (p1 != null){
            sr.SerializeMessage(1, p1.ComputeSize());
            p1.WriteFields(sr);
        }
        if (p2 != null){
            sr.SerializeMessage(2, p2.ComputeSize());
            p2.WriteFields(sr);
        }
        return result;
    }


}
