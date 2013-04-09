/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Objects.Position;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;
import java.util.Vector;


/**
 *
 * @author Admin
 */
public class MoveKingResponse implements IProtoDeserializable, ICommand {
    
    private Vector steps;      //proto 1   //array of <Position> objects

    public MoveKingResponse(){}

    public Vector getSteps(){return steps;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        steps = new Vector();
        int res = dsr.readMessage(1);
        while(res != -1){
            Position p = new Position();
            p.LoadFrom(dsr.getMessageBytes());
            steps.addElement(p);
            res = dsr.readMessage(1);
        }
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener) listener;
        l.MoveKingResponseReceived(this);
    }


}
