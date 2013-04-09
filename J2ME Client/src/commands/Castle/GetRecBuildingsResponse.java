/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Castle;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.ICastleCommandListener;
import Objects.ResBuild;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class GetRecBuildingsResponse implements IProtoDeserializable, ICommand {
    private ResBuild resBuild;  //proto 1

    public GetRecBuildingsResponse(){}

    public ResBuild getResBuild(){return resBuild;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        if (res == -1){
            resBuild = null;
            return;
        }
        resBuild = new ResBuild();
        resBuild.LoadFrom(dsr.getMessageBytes());
    }

    public void Execute(Object listener) {
        ICastleCommandListener l = (ICastleCommandListener)listener;
        l.GetRecBuildingsResponseReceived(this);
    }


}
