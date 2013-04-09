/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IEmpireCommandListener;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoDeserializable;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class ExcludeFromEmpireMessage implements IProtoSerializableRequest, IProtoDeserializable, ICommand {
    private int com_id;

    public ExcludeFromEmpireMessage(){
        com_id = Commands.EXCLUDE_FROM_EMPIRE_MESSAGE;
    }

    public int ComputeSize() {
        return 0;
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        
    }

    public void Execute(Object listener) {
        IEmpireCommandListener l = (IEmpireCommandListener)listener;
        l.ExcludeFromEmpireMessReceived(this);
    }


}
