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
public class Mine implements IProtoDeserializable{

        private int mineId;                    //proto 1
        private int leftX;                     //proto 2
        private int topY;                      //proto 3
        private int width;                     //proto 4
        private int height;                    //proto 5
        private float wayCost;                 //proto 6
        private Resource gainingResource;      //proto 7
        private int sizeMine;                  //proto 8

        public Mine(){}

        public int getId(){return mineId;}
        public int getLeftX(){return leftX;}
        public int getTopY(){return topY;}
        public int getWidth(){return width;}
        public int getHeight(){return height;}
        public float getWayCost(){return wayCost;}
        public Resource getGainingResource(){return gainingResource;}
        public int getSizeMine(){return sizeMine;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        mineId = dsr.readInt(1);
            leftX = dsr.readInt(2);
            topY = dsr.readInt(3);
            width = dsr.readInt(4);
            height = dsr.readInt(5);
            wayCost = dsr.readFloat(6);
            int res = dsr.readMessage(7);
            if (res == -1) gainingResource = null;
            else{
                gainingResource = new Resource();
                gainingResource.LoadFrom(dsr.getMessageBytes());
            }
            sizeMine = dsr.readInt(8);
    }


        /*public void LoadFrom(FieldDeserializer dsr) throws IOException {
            mineId = dsr.readInt(1);
            leftX = dsr.readInt(2);
            topY = dsr.readInt(3);
            width = dsr.readInt(4);
            height = dsr.readInt(5);
            wayCost = dsr.readFloat(6);
            int res = dsr.readMessage(7);
            if (res == -1) gainingResource = null;
            else{
                gainingResource = new Resource();
                gainingResource.LoadFrom(dsr);
            }
            sizeMine = dsr.readInt(8);
        }*/


}
