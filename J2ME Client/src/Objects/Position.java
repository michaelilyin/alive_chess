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
public class Position implements IProtoDeserializable {

    private int x;      //proto 1
    private int y;      //proto 2

    public Position(){}

    public int getX(){return x;}
    public int getY(){return y;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        x = dsr.readInt(1);
        y = dsr.readInt(2);
    }


}
