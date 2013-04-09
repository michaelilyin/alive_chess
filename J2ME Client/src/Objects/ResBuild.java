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
public class ResBuild implements IProtoDeserializable {

    private int coal = 1; //proto 1    // уголь
    private int gold = 1; //proto 2    // золото
    private int iron = 1; //proto 3    // железо
    private int stone = 1;//proto 4    // камень
    private int wood = 1; //proto 5    // дерево

    public ResBuild(){}

    public int getCoal(){return coal;}
    public void setCoal(int value){coal = value;}
    public int getGold(){return gold;}
    public void setGold(int value){gold = value;}
    public int getIron(){return iron;}
    public void setIron(int value){iron = value;}
    public int getStone(){return stone;}
    public void setStone(int value){stone = value;}
    public int getWood(){return wood;}
    public void setWood(int value){wood = value;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        coal = dsr.readInt(1);
        gold = dsr.readInt(2);
        iron = dsr.readInt(3);
        stone = dsr.readInt(4);
        wood = dsr.readInt(5);
    }


}
