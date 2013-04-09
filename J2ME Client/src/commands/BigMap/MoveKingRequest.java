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
public class MoveKingRequest implements IProtoSerializableRequest {

    private int com_id;
    private int x;
    private int y;

    public MoveKingRequest(){
        com_id = Commands.MOVE_KING_REQUEST;
    }

    public void setX(int value){
        x = value;
    }

    public void setY(int value){
        y = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, x)+
               ComputeSizeUtil.ComputeInt(2, y);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, x);
        sr.SerializeInt(2, y);
        return result;
    }


}
