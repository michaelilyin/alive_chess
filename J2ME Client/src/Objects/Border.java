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
public class Border implements IProtoDeserializable {


    private int borderId;      //proto 1
    private int x;             //proto 2
    private int y;             //proto 3
    private float wayCost;     //proto 4

    public Border(){}

    public int getId(){return borderId;}
    public int getX(){return x;}
    public int getY(){return y;}
    public float getWayCost(){return wayCost;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        borderId = dsr.readInt(1);
        x = dsr.readInt(2);
        y = dsr.readInt(3);
        wayCost = dsr.readFloat(4);
    }


}
