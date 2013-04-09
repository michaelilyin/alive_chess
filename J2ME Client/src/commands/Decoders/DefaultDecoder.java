/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Decoders;

import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public abstract class DefaultDecoder {

    protected IProtoDeserializable command;

    public IProtoDeserializable DeserializeCommand(byte[] buff) throws IOException{
        command.LoadFrom(buff);
        return command;
    }
    public abstract void recognizeCommand(int c);
}
