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
public class GetRecBuildingsRequest implements IProtoSerializableRequest{

    private int com_id;

    private int building_type;
        //Voencomat = 0, //для создания пешек
        //Stable = 1, // для создания коней
        //SchoolOfficers = 2, // для создание офицеров
        //VVU = 3, // для создания лодьи
        //GeneralStaff = 4  // для создания королев

    public GetRecBuildingsRequest(){
        com_id = Commands.GET_REC_BUILDINGS_REQUEST;
    }

    public void setBuildingType(int value){
        building_type = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, building_type);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, building_type);
        return result;
    }



}
