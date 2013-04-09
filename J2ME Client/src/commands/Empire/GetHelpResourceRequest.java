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
public class GetHelpResourceRequest implements IProtoSerializableRequest {

    private int com_id;
    private int resourceCnt;

    private int resourceType;
        //Coal   = 0, // уголь
        //Gold   = 1, // золото
        //Iron   = 2, // железо
        //Stone  = 3, // камень
        //Wood   = 4  // дерево

    public GetHelpResourceRequest(){
        com_id = Commands.GET_HELP_RESOURCE_REQUEST;
    }

    public void setResourceCount(int value){
        resourceCnt = value;
    }
    public void setResourceType(int value){
        resourceType = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, resourceCnt)
               + ComputeSizeUtil.ComputeInt(2, resourceType);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, resourceCnt);
        sr.SerializeInt(2, resourceType);
        return result;
    }

}
