/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Battle;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBattleCommandListener;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class MoveUnitResponse implements IProtoDeserializable, ICommand {

    private boolean isSucceed;

    public boolean getSucceedStatus(){
        return isSucceed;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        isSucceed = dsr.readBool(1);
    }

    public void Execute(Object listener) {
        IBattleCommandListener l = (IBattleCommandListener)listener;
        l.MoveUnitResponseReceived(this);
    }


}
