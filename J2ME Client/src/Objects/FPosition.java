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
public class FPosition implements IProtoSerializableObject, IProtoDeserializable{
    private float x;   //1-st field to be serialized by protobuf
    private float y;   //2-nd field to be serialized by protobuf

    public FPosition(){
        x=0;
        y=0;
    }
    public FPosition(float _x, float _y){
        x = _x;
        y = _y;
    }

    public void setX(float value){
        x = value;
    }
    public float getX(){
        return x;
    }
    public void setY(float value){
        y = value;
    }
    public float getY(){
        return y;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeFloat(1, x)+ComputeSizeUtil.ComputeFloat(2, y);
    }

    public void WriteFields(FieldSerializer sr) throws IOException {
        sr.SerializeFloat(1, x);
        sr.SerializeFloat(2, y);
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        x = dsr.readFloat(1);
        y = dsr.readFloat(2);
    }


    /*public void LoadFrom(FieldDeserializer dsr) throws IOException {
        x = dsr.readFloat(1);
        y = dsr.readFloat(2);
    }*/



}
