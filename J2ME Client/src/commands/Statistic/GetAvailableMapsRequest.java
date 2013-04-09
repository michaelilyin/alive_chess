/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Statistic;

import commands.Util.Commands;
import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class GetAvailableMapsRequest implements IProtoSerializableRequest {

    private int com_id;

    private int world_type;
        //local = 0
        //global = 1


    public GetAvailableMapsRequest(){
        com_id = Commands.GET_AVAILABLE_MAPS_REQUEST;
    }

    public void setWorldType(int value){
        world_type = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, world_type);
    }

    public byte[] toByte() throws IOException{
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, world_type);
        return result;
    }

}
