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
public class Castle implements IProtoDeserializable {

        private int castleId;      //proto 1
        private int leftX;         //proto 2
        private int topY;          //proto 3
        private int width;         //proto 4
        private int height;        //proto 5
        private float wayCost;     //proto 6

        public Castle(){

        }

        public int getId(){ return castleId; }
        public int getLeftX(){return leftX;}
        public int getTopY(){return topY;}
        public int getWidth(){return width;}
        public int getHeight(){return height;}
        public float getWayCost(){return wayCost;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        castleId = dsr.readInt(1);
        leftX = dsr.readInt(2);
        topY = dsr.readInt(3);
        width = dsr.readInt(4);
        height = dsr.readInt(5);
        wayCost = dsr.readFloat(6);
    }


    /*public void LoadFrom(FieldDeserializer dsr) throws IOException {
        castleId = dsr.readInt(1);
        leftX = dsr.readInt(2);
        topY = dsr.readInt(3);
        width = dsr.readInt(4);
        height = dsr.readInt(5);
        wayCost = dsr.readFloat(6);
    }*/


}
