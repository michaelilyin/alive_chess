/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.Commands;
import java.io.IOException;
//import Commands.Util.*;
import Serializer.Utils.*;

/**
 *
 * @author Admin
 */
public class GetGameStateRequest implements IProtoSerializableRequest {

    private int com_id;

    public GetGameStateRequest() {
        com_id = Commands.GET_GAME_STATE_REQUEST;
    }

    public int ComputeSize() {
        return 0;
    }

    public byte[] toByte() throws IOException{
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        return result;
    }

}
