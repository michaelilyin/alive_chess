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
public class King implements IProtoDeserializable, IProtoSerializableObject{
        private int kingId;                     //protomember 1
        private int x;                          //protomember 2
        private int y;                          //protomember 3
        private String kingName;                //protomember 4
        private int kingExperience;             //protomember 5
        private int kingMilitaryRank;           //protomember 6

    public King() {}

    public int getId(){return kingId;}
    public void setId(int value){kingId = value;}
    public int getX(){return x;}
    public void setX(int value){x = value;}
    public int getY(){return y;}
    public void setY(int value){y = value;}
    public String getName(){return kingName;}
    public void setName(String value){kingName = value;}
    public int getExperience(){return kingExperience;}
    public void setExperience(int value){kingExperience = value;}
    public int getMilitaryRank(){return kingMilitaryRank;}
    public void setMilitaryRank(int value){kingMilitaryRank = value;}

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, kingId) +
               ComputeSizeUtil.ComputeInt(2, x) +
               ComputeSizeUtil.ComputeInt(3, y) +
               ComputeSizeUtil.ComputeString(4, kingName) +
               ComputeSizeUtil.ComputeInt(5, kingExperience) +
               ComputeSizeUtil.ComputeInt(6, kingMilitaryRank);
    }

    public void WriteFields(FieldSerializer sr) throws IOException {
        sr.SerializeInt(1, kingId);
        sr.SerializeInt(2, x);
        sr.SerializeInt(3, y);
        sr.SerializeString(4, kingName);
        sr.SerializeInt(5, kingExperience);
        sr.SerializeInt(6, kingMilitaryRank);
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        kingId = dsr.readInt(1);
        x = dsr.readInt(2);
        y = dsr.readInt(3);
        kingName = dsr.readString(4);
        kingExperience = dsr.readInt(5);
        kingMilitaryRank = dsr.readInt(6);
    }


    /*public void LoadFrom(FieldDeserializer dsr) throws IOException {
        kingId = dsr.readInt(1);
        x = dsr.readInt(2);
        y = dsr.readInt(3);
        kingName = dsr.readString(4);
        kingExperience = dsr.readInt(5);
        kingMilitaryRank = dsr.readInt(6);
    }*/


}
