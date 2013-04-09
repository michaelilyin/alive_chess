/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Error;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IErrorCommandListener;
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
public class ErrorMessage implements IProtoSerializableRequest, IProtoDeserializable, ICommand {
    private int com_id;
    private String message;     //proto 1

    public ErrorMessage(){com_id = Commands.ERROR_MESSAGE;}

    public String getMessage(){return message;}
    public void setMessage(String value){message = value;}

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeString(1, message);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeString(1, message);
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        message = dsr.readString(1);
    }

    public void Execute(Object listener) {
        IErrorCommandListener l = (IErrorCommandListener)listener;
        l.ErrorMessReceived(this);
    }


}
