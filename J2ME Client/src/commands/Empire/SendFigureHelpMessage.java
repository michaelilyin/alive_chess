/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IEmpireCommandListener;
import Objects.Unit;
import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoDeserializable;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class SendFigureHelpMessage implements IProtoSerializableRequest, IProtoDeserializable, ICommand {
    private int com_id;

    private int receiverId;     //proto 1
    private Vector units;       //proto 2   //array of <Unit> objects
    private int fromCastle;     //proto 3

    public SendFigureHelpMessage(){com_id = Commands.SEND_FIGURE_HELP_MESSAGE;}

    public int getReceiverId(){return receiverId;}
    public void setReceiverId(int value){receiverId = value;}
    public Vector getUnits(){return units;}
    public void setUnits(Vector value){units = value;}
    public int getFromCastle(){return fromCastle;}
    public void setFromCastle(int value){fromCastle = value;}

    public int ComputeSize() {
        int result = 0;
        result += ComputeSizeUtil.ComputeInt(1, receiverId);
        for (int i=0; i<units.size(); i++){
            Unit u = (Unit)units.elementAt(i);
            int size = u.ComputeSize();
            result += ComputeSizeUtil.ComputeMessage(2, size) + size;
        }
        result += ComputeSizeUtil.ComputeInt(3, fromCastle);
        return result;
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, receiverId);
        for (int i=0; i<units.size(); i++){
            Unit u = (Unit)units.elementAt(i);
            sr.SerializeMessage(2, u.ComputeSize());
            u.WriteFields(sr);
        }
        sr.SerializeInt(3, fromCastle);
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        receiverId = dsr.readInt(1);
        units = new Vector();
        int res = dsr.readMessage(2);
        while(res != -1){
            Unit u = new Unit();
            u.LoadFrom(dsr.getMessageBytes());
            units.addElement(u);
            res = dsr.readMessage(2);
        }
        fromCastle = dsr.readInt(3);
    }

    public void Execute(Object listener) {
        IEmpireCommandListener l = (IEmpireCommandListener)listener;
        l.SendFigureHelpMessReceived(this);
    }


}
