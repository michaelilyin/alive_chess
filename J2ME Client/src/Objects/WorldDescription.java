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
public class WorldDescription implements IProtoDeserializable {

    private String mapName;     //proto 1

    public WorldDescription(){}

    public String getMapName(){return mapName;}
    public void setMapName(String value){mapName = value;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        mapName = dsr.readString(1);
    }


}
