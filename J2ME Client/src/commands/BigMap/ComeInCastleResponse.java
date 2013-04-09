/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IBigMapCommandListener;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class ComeInCastleResponse implements IProtoDeserializable, ICommand {

    private int castleId;

    public ComeInCastleResponse(){}

    public int getCastleId(){return castleId;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        castleId = dsr.readInt(1);
    }

    public void Execute(Object listener) {
        IBigMapCommandListener l = (IBigMapCommandListener) listener;
        l.ComeInCastleResponseReceived(this);
    }


}
