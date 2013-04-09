/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Objects;

import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class MultyObject implements IProtoDeserializable {

    private int multyObjectId;         //proto 1
    private int leftX;                 //proto 2
    private int topY;                  //proto 3
    private int width;                 //proto 4
    private int height;                //proto 5
    private float wayCost;             //proto 6
    private int multyObjectType;       //proto 7
        //Rock = 0

    public MultyObject(){}

    public int getId(){return multyObjectId;}
    public int getX(){return leftX;}
    public int getY(){return topY;}
    public int getWidth(){return width;}
    public int getHeight(){return height;}
    public float getWayCost(){return wayCost;}
    public int getMultyObjectType(){return multyObjectType;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        multyObjectId = dsr.readInt(1);
        leftX = dsr.readInt(2);
        topY = dsr.readInt(3);
        width = dsr.readInt(4);
        height = dsr.readInt(5);
        wayCost = dsr.readFloat(6);
        multyObjectType = dsr.readInt(7);
    }


}
