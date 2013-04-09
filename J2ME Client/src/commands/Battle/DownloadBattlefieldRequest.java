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
public class DownloadBattlefieldRequest implements IProtoSerializableRequest {
    private int com_id;
    private int opponent_id;

    public DownloadBattlefieldRequest(){
        com_id = Commands.DOWNLOAD_BATTLEFIELD_REQUEST;
    }

    public void setOpponentId(int value){
        opponent_id = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, opponent_id);
    }

    public byte[] toByte() throws IOException{
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, opponent_id);
        return result;
    }
}
