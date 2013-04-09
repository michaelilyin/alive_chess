/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Objects;

import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoDeserializable;
import Serializer.Utils.IProtoSerializableObject;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class Unit implements IProtoDeserializable, IProtoSerializableObject{
    private int unitId;     //protomember 1
    private int unitType;   //protomember 2
        //Knight = 0,
        //Queen  = 1,
        //Rook   = 2,
        //Bishop = 3,
        //Pawn   = 10
    private int unitCount;  //protomember 3

    public Unit() {    }

    public int getId(){return unitId;}
    public void setId(int value){unitId = value;}
    public int getType(){return unitType;}
    public void setType(int value){unitType = value;}
    public int getCount(){return unitCount;}
    public void setCount(int value){unitCount = value;}

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, unitId) +
               ComputeSizeUtil.ComputeInt(2, unitType) +
               ComputeSizeUtil.ComputeInt(3, unitCount);
    }

    public void WriteFields(FieldSerializer sr) throws IOException {
        sr.SerializeInt(1, unitId);
        sr.SerializeInt(2, unitType);
        sr.SerializeInt(3, unitCount);
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        unitId = dsr.readInt(1);
        unitType = dsr.readInt(2);
        unitCount = dsr.readInt(3);
    }


    /*public void LoadFrom(FieldDeserializer dsr) throws IOException {
        unitId = dsr.readInt(1);
        unitType = dsr.readInt(2);
        unitCount = dsr.readInt(3);
    }*/


}
