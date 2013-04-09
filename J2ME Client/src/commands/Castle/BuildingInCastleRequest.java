/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Castle;

import commands.Util.Commands;
import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class BuildingInCastleRequest implements IProtoSerializableRequest {

    private int com_id;

    private int inner_type;
        //Voencomat = 0, //для создания пешек
        //Stable = 1, // для создания коней
        //SchoolOfficers = 2, // для создание офицеров
        //VVU = 3, // для создания лодьи
        //GeneralStaff = 4  // для создания королев
    
    public BuildingInCastleRequest(){
        com_id = Commands.BUILDING_IN_CASTLE_REQUEST;
    }

    public void setBuildingType(int value){
        inner_type = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, inner_type);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, inner_type);
        return result;
    }


}
