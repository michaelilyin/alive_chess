/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Serializer.Utils;

import java.io.IOException;

/**
 *
 * @author Admin
 */
public interface IProtoSerializableRequest {
    public int ComputeSize();
    public byte[] toByte() throws IOException;
}
