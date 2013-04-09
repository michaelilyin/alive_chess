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
public class GetUnityMapRequest implements IProtoSerializableRequest {

    private int com_id;
    private String name;

    public GetUnityMapRequest(){
        com_id = Commands.GET_UNITY_MAP_REQUEST;
    }

    public void setName(String value){
        name = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeString(1, name);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeString(1, name);
        return result;
    }
}
