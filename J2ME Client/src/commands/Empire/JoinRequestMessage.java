/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IEmpireCommandListener;
import Objects.King;
import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoDeserializable;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class JoinRequestMessage implements IProtoDeserializable, IProtoSerializableRequest, ICommand {

    private int com_id;
    private King candidate;     //proto 1

    public JoinRequestMessage(){com_id = Commands.JOIN_REQUEST_MESSAGE;}

    public King getCandidate(){return candidate;}
    public void setCandidate(King value){candidate = value;}

    public int ComputeSize() {
        int result = 0;
        if (candidate != null){
            int size = candidate.ComputeSize();
            result += ComputeSizeUtil.ComputeMessage(1, size) + size;
        }
        return result;
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        if (candidate != null){
            sr.SerializeMessage(1, candidate.ComputeSize());
            candidate.WriteFields(sr);
        }
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        if (res == -1){
            candidate = null;
            return;
        }
        candidate = new King();
        candidate.LoadFrom(dsr.getMessageBytes());
    }

    public void Execute(Object listener) {
        IEmpireCommandListener l = (IEmpireCommandListener)listener;
        l.JoinRequestMessReceived(this);
    }


}
