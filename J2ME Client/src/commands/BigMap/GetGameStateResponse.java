/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.Castle;
import Objects.King;
import Objects.Resource;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class GetGameStateResponse implements IProtoDeserializable, ICommand {

        private King king;              //proto 1
        private Castle castle;          //proto 2
        private Vector resources;       //proto 3

        public GetGameStateResponse(){}

        public King getKing(){return king;}
        public Castle getCastle(){return castle;}
        public Vector gerResources(){return resources;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        if (res == -1) king = null;
        else{
            king = new King();
            king.LoadFrom(dsr.getMessageBytes());
        }
        res = dsr.readMessage(2);
        if (res == -1) castle = null;
        else{
            castle = new Castle();
            castle.LoadFrom(dsr.getMessageBytes());
        }
        resources = new Vector();
        res = dsr.readMessage(3);
        while(res != -1){
            Resource r = new Resource();
            r.LoadFrom(dsr.getMessageBytes());
            resources.addElement(r);
            res = dsr.readMessage(3);
        }
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener) listener;
        l.GetGameStateResponseReceived(this);
    }


}
