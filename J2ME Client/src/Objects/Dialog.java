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
public class Dialog implements IProtoDeserializable {
    private int id;             //proto 1
    private King respondent;    //proto 2
    private boolean yourStep;   //proto 3

    public Dialog(){}

    public int getId(){return id;}
    public King getRespondent(){return respondent;}
    public boolean getYourStep(){return yourStep;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        id = dsr.readInt(1);
        int res = dsr.readMessage(2);
        if(res == -1) respondent = null;
        else{
            respondent = new King();
            respondent.LoadFrom(dsr.getMessageBytes());
        }
        yourStep = dsr.readBool(3);
    }


    /*public void LoadFrom(FieldDeserializer dsr) throws IOException {
        id = dsr.readInt(1);
        int res = dsr.readMessage(2);
        if(res == -1) respondent = null;
        else{
            respondent = new King();
            respondent.LoadFrom(dsr);
        }
        yourStep = dsr.readBool(3);
    }*/


}
