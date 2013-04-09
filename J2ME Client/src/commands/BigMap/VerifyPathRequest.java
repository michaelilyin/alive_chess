/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.Commands;
import Serializer.Utils.*;
import java.io.IOException;
//import Commands.Util.*;
import Objects.FPosition;
import java.util.Vector;
/**
 *
 * @author Admin
 */
public class VerifyPathRequest implements IProtoSerializableRequest {
    private int com_id;
    private Vector positions;

    public VerifyPathRequest(){
        com_id = Commands.VERIFY_PATH_REQUEST;
    }

    public void setPositions(Vector pos){
        positions = pos;
    }

    public int ComputeSize() {
        int size = 0;
        int tmp_size = 0;
        for (int i=0; i<positions.size(); i++){
            FPosition pos = (FPosition)positions.elementAt(i);
            tmp_size = pos.ComputeSize();
            size+=tmp_size+ComputeSizeUtil.ComputeMessage(1, tmp_size);
        }
        return size;
    }



    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        int tmp_size = 0;
        for(int i=0; i<positions.size(); i++){
            FPosition p = (FPosition)positions.elementAt(i);
            tmp_size = p.ComputeSize();
            sr.SerializeMessage(1, tmp_size);
            p.WriteFields(sr);
        }
        return result;
    }


}
