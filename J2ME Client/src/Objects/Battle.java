/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Objects;

import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class Battle implements IProtoDeserializable {
    private int id;                 //protomember 1
    private King respondent;        //protomember 2
    private boolean youStep;        //protomember 3
    private Vector playerArmy;      //protomember 4
    private Vector opponentArmy;    //protomember 5

    public Battle() {
    }


    public int getId(){return id;}
    public King getRespondent(){return respondent;}
    public boolean getYouStep(){return youStep;}
    public Vector getPlayerArmy(){return playerArmy;}
    public Vector getOpponentArmy(){return opponentArmy;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        id = dsr.readInt(1);
        int res = dsr.readMessage(2);
        if (res == -1) respondent = null;
        else{
            respondent = new King();
            respondent.LoadFrom(dsr.getMessageBytes());
        }
        youStep = dsr.readBool(3);
        res = dsr.readMessage(4);
        playerArmy = new Vector();
        while(res != -1){
            
            Unit unit = new Unit();
            unit.LoadFrom(dsr.getMessageBytes());
            playerArmy.addElement(unit);
            res = dsr.readMessage(4);
        }
        opponentArmy = new Vector();
        res = dsr.readMessage(5);
        while(res != -1){
            Unit unit = new Unit();
            unit.LoadFrom(dsr.getMessageBytes());
            opponentArmy.addElement(unit);
            res = dsr.readMessage(5);
        }
    }



    /*public void LoadFrom(FieldDeserializer dsr) throws IOException {
        System.out.println("deserializing id army///");
        id = dsr.readInt(1);
        System.out.println("deserializing opponent army///");
        int res = dsr.readMessage(2);
        if (res == -1){
            respondent = null;
        }else{
            respondent = new King();
            respondent.LoadFrom(dsr);
        }
        System.out.println("deserializing youstep///");
        youStep = dsr.readBool(3);
        playerArmy = new Vector();
        opponentArmy = new Vector();
        System.out.println("deserializing players army///");
        res = dsr.readMessage(4);
        while(res != -1){
            Unit unit = new Unit();
            unit.LoadFrom(dsr);
            playerArmy.addElement(unit);
            res = dsr.readMessage(4);
        }
        System.out.println("deserializing opponents army///");
        res = dsr.readMessage(5);
        while(res != -1){
            Unit unit = new Unit();
            unit.LoadFrom(dsr);
            opponentArmy.addElement(unit);
            res = dsr.readMessage(5);
        }
    }*/
        

}

