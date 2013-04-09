/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IEmpireCommandListener;
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
public class VoteBallotMessage implements IProtoSerializableRequest, IProtoDeserializable, ICommand {
    private int com_id;
    private boolean support;    //proto 1

    public VoteBallotMessage(){com_id = Commands.VOTE_BALLOT_MESSAGE;}

    public boolean getSupport(){return support;}
    public void setSupport(boolean value){support = value;}

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeBool(1, support);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeBool(1, support);
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        support = dsr.readBool(1);
    }

    public void Execute(Object listener) {
        IEmpireCommandListener l = (IEmpireCommandListener)listener;
        l.VoteBallotMessReceived(this);
    }


}
