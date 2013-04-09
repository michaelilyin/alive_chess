/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.BasePoint;
import Objects.Border;
import Objects.Castle;
import Objects.Mine;
import Objects.MultyObject;
import Objects.SingleObject;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class GetMapResponse implements IProtoDeserializable, ICommand {

    private int mapId;                             //proto 1
    private int sizeMapX;                          //proto 2
    private int sizeMapY;                          //proto 3
    private Vector castles;                        //proto 4    //array of <Castle> objects
    private Vector mines;                          //proto 5    //array of <Mine> objects
    private Vector basePoints;                     //proto 6    //array of <BasePoint> objects
    private Vector singleObjects;                  //proto 7    //array of <SingleObject> objects
    private Vector multyObjects;                   //proto 8    //array of <MultyObject> objects
    private Vector borders;                        //proto 9    //array of <Border> objects

    public GetMapResponse(){}

    public int getId(){return mapId;}
    public int getWidth(){return sizeMapX;}
    public int getHeight(){return sizeMapY;}
    public Vector getCastles(){return castles;}                 
    public Vector getMines(){return mines;}                     
    public Vector getBasePoints(){return basePoints;}           
    public Vector getSingleObjects(){return singleObjects;}     
    public Vector getMultyObjects(){return multyObjects;}       
    public Vector getBorders(){return borders;}                 

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        mapId = dsr.readInt(1);
        sizeMapX = dsr.readInt(2);
        sizeMapY = dsr.readInt(3);
        castles = new Vector();
        int res = dsr.readMessage(4);
        while(res != -1){
            Castle c = new Castle();
            c.LoadFrom(dsr.getMessageBytes());
            castles.addElement(c);
            res = dsr.readMessage(4);
        }
        mines = new Vector();
        res = dsr.readMessage(5);
        while(res != -1){
            Mine m = new Mine();
            m.LoadFrom(dsr.getMessageBytes());
            mines.addElement(m);
            res = dsr.readMessage(5);
        }
        basePoints = new Vector();
        res = dsr.readMessage(6);
        while(res != -1){
            BasePoint bp = new BasePoint();
            bp.LoadFrom(dsr.getMessageBytes());
            basePoints.addElement(bp);
            res = dsr.readMessage(6);
        }
        singleObjects = new Vector();
        res = dsr.readMessage(7);
        while(res != -1){
            SingleObject so = new SingleObject();
            so.LoadFrom(dsr.getMessageBytes());
            singleObjects.addElement(so);
            res = dsr.readMessage(7);
        }
        multyObjects = new Vector();
        res = dsr.readMessage(8);
        while(res != -1){
            MultyObject mo = new MultyObject();
            mo.LoadFrom(dsr.getMessageBytes());
            multyObjects.addElement(mo);
            res = dsr.readMessage(8);
        }
        borders = new Vector();
        res = dsr.readMessage(9);
        while(res != -1){
            Border b = new Border();
            b.LoadFrom(dsr.getMessageBytes());
            borders.addElement(b);
            res = dsr.readMessage(9);
        }
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener) listener;
        l.GetMapResponseReceived(this);
    }


}
