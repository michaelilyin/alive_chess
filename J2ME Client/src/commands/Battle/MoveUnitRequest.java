/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Battle;

import commands.Util.Commands;
import Serializer.Utils.*;
//import Commands.Util.*;
import java.io.IOException;
/**
 *
 * @author Admin
 */
public class MoveUnitRequest implements IProtoSerializableRequest {
    private int com_id;
    byte position;
    int enemyId;
    private int battleId;

    public MoveUnitRequest(){
        com_id = Commands.MOVE_UNIT_REQUEST;
    }

    public void setPosition(byte value){
        position = value;
    }

    public void setEnemyId(int value){
        enemyId = value;
    }

    public void setBattleId(int value){
        battleId = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, (int)position)+
                ComputeSizeUtil.ComputeInt(2, enemyId)+
                ComputeSizeUtil.ComputeInt(3, battleId);
    }

    public byte[] toByte() throws IOException{
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, (int)position);
        sr.SerializeInt(2, enemyId);
        sr.SerializeInt(3, battleId);
        return result;
    }
}
