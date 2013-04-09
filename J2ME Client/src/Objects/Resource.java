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
public class Resource implements IProtoDeserializable, IProtoSerializableObject {

    private int resourceId;        //proto 1
    private int x;                 //proto 2
    private int y;                 //proto 3
    private int resourceType;      //proto 4
        //Coal   = 0, // уголь
        //Gold   = 1, // золото
        //Iron   = 2, // железо
        //Stone  = 3, // камень
        //Wood   = 4  // дерево
    private int resourceCount;     //proto 5
    private float wayCost;         //proto 6

    public Resource(){}

    public int getId(){return resourceId;}
    public void setId(int value){resourceId = value;}
    public int getX(){return x;}
    public void setX(int value){x = value;}
    public int getY(){return y;}
    public void setY(int value){y = value;}
    public int getResourceType(){return resourceType;}
    public void setResourceType(int value){resourceType = value;}
    public int getResourceCount(){return resourceCount;}
    public void setResourceCount(int value){resourceCount = value;}
    public float getWayCost(){return wayCost;}
    public void setWayCost(float value){wayCost = value;}

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, resourceId)+
               ComputeSizeUtil.ComputeInt(2, x)+
               ComputeSizeUtil.ComputeInt(3, y)+
               ComputeSizeUtil.ComputeInt(4, resourceType)+
               ComputeSizeUtil.ComputeInt(5, resourceCount)+
               ComputeSizeUtil.ComputeFloat(6, wayCost);
    }

    public void WriteFields(FieldSerializer sr) throws IOException {
        sr.SerializeInt(1, resourceId);
        sr.SerializeInt(2, x);
        sr.SerializeInt(3, y);
        sr.SerializeInt(4, resourceType);
        sr.SerializeInt(5, resourceCount);
        sr.SerializeFloat(6, wayCost);
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        resourceId = dsr.readInt(1);
        x = dsr.readInt(2);
        y = dsr.readInt(3);
        resourceType = dsr.readInt(4);
        resourceCount = dsr.readInt(5);
        wayCost = dsr.readFloat(6);
    }



    /*public void LoadFrom(FieldDeserializer dsr) throws IOException {
        resourceId = dsr.readInt(1);
        x = dsr.readInt(2);
        y = dsr.readInt(3);
        resourceType = dsr.readInt(4);
        resourceCount = dsr.readInt(5);
        wayCost = dsr.readFloat(6);
    }*/


}
