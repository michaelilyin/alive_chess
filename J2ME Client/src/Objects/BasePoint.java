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
public class BasePoint implements IProtoDeserializable {

    private int basePointId;
    private int x;
    private int y;
    private int landscapePointType;
        //None   = 0,
        //Grass  = 1, // трава
        //Ground = 2, // земля
        //Snow   = 3  // снег
    private float wayCost;

    public BasePoint(){}

    public int getId(){return basePointId;}
    public int getX(){return x;}
    public int getY(){return y;}
    public int getLandscapePointType(){return landscapePointType;}
    public float getWayCost(){return wayCost;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        basePointId = dsr.readInt(1);
        x = dsr.readInt(2);
        y = dsr.readInt(3);
        landscapePointType = dsr.readInt(4);
        wayCost = dsr.readFloat(5);
    }


}
