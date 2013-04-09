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
public class Negotiate implements IProtoDeserializable {

    private int id;             //proto 1
    private King respondent;    //proto 2
    private boolean youStep;    //proto 3

    public Negotiate(){}

    public int getId(){return id;}
    public void setId(int value){id = value;}
    public King getRespondent(){return respondent;}
    public void setRespondent(King value){respondent = value;}
    public boolean getYouStep(){return youStep;}
    public void setYouStep(boolean value){youStep = value;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        id = dsr.readInt(1);
        int res = dsr.readMessage(2);
        if (res == -1) respondent = null;
        else{
            respondent = new King();
            respondent.LoadFrom(buffer);
        }
        youStep = dsr.readBool(3);
    }


}
