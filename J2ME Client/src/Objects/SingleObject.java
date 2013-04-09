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
public class SingleObject implements IProtoDeserializable{

    private int singleObjectId;         //proto 1
    private int x;                      //proto 2
    private int y;                      //proto 3
    private int singleObjectType;       //proto 4
        //Tree = 0
        //Obstacle = 1
    private float wayCost;              //proto 5

    public SingleObject(){}

    public int getId(){return singleObjectId;}
    public int getX(){return x;}
    public int getY(){return y;}
    public int getSingleObjectType(){return singleObjectType;}
    public float getwayCost(){return wayCost;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        singleObjectId = dsr.readInt(1);
        x = dsr.readInt(2);
        y = dsr.readInt(3);
        singleObjectType = dsr.readInt(4);
        wayCost = dsr.readFloat(5);
    }


}
