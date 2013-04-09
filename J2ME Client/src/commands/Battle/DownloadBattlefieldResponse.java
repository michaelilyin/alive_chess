/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Battle;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBattleCommandListener;
import Objects.Battle;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class DownloadBattlefieldResponse implements IProtoDeserializable, ICommand {

    private Battle battle;

    public DownloadBattlefieldResponse(){ }

    public Battle getBattle(){
        return battle;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);       
        int res = dsr.readMessage(1);
        if (res == -1) {
            battle=null;
            return;
        }
        battle = new Battle();
        battle.LoadFrom(dsr.getMessageBytes());
    }

    public void Execute(Object listener) {
        IBattleCommandListener l = (IBattleCommandListener)listener;
        l.DownloadBattlefieldResponseReceived(this);
    }



}
