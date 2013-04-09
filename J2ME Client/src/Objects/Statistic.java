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
public class Statistic implements IProtoDeserializable {

    private int pawnNumber;         //proto 1
    private int bishopNumber;       //proto 2
    private int rookNumber;         //proto 3
    private int queenNumber;        //proto 4
    private int knightNumber;       //proto 5

    public Statistic(){}

    public int getPawnNumber(){return pawnNumber;}
    public void setPawnNumber(int value){pawnNumber = value;}
    public int getBishopNumber(){return bishopNumber;}
    public void setBishopNumber(int value){bishopNumber = value;}
    public int getRookNumber(){return rookNumber;}
    public void setRookNumber(int value){rookNumber = value;}
    public int getQueenNumber(){return queenNumber;}
    public void setQueenNumber(int value){queenNumber = value;}
    public int getKnightNumber(){return knightNumber;}
    public void setKnightNumber(int value){knightNumber = value;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        pawnNumber = dsr.readInt(1);
        bishopNumber = dsr.readInt(2);
        rookNumber = dsr.readInt(3);
        queenNumber = dsr.readInt(4);
        knightNumber = dsr.readInt(5);
    }


}
