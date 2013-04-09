/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Dialog;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IDialogCommandListener;
import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.FieldSerializer;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class PayOffDialogMessage extends Message implements ICommand {

    private int resourceType;     //proto 3
        //Coal   = 0, // уголь
        //Gold   = 1, // золото
        //Iron   = 2, // железо
        //Stone  = 3, // камень
        //Wood   = 4  // дерево
    private int resourceCount;    //proto 4

    public PayOffDialogMessage(){
        super();
        com_id = Commands.PAY_OFF_DIALOG_MESSAGE;
    }

    public int getResourceType(){return resourceType;}
    public void setResourceType(int value){resourceType = value;}
    public int getResourceCount(){return resourceCount;}
    public void setResourceCount(int value){resourceCount = value;}

    public int ComputeSize() {
        return super.ComputeSize() +
               ComputeSizeUtil.ComputeInt(3, resourceType) +
               ComputeSizeUtil.ComputeInt(4, resourceCount);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, type);
        sr.SerializeInt(2, disputeId);
        sr.SerializeInt(3, resourceType);
        sr.SerializeInt(4, resourceCount);
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        type = dsr.readInt(1);
        disputeId = dsr.readInt(2);
        resourceType = dsr.readInt(3);
        resourceCount = dsr.readInt(4);
    }

    public void Execute(Object listener) {
        IDialogCommandListener l = (IDialogCommandListener)listener;
        l.PayOffDialogMessReceived(this);
    }


}
