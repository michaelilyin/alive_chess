/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Serializer.Utils;

import Protobuf.Utils.*;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class FieldSerializer  {
    CodedOutputStream os;

    public FieldSerializer(byte[] buf){
        os = CodedOutputStream.newInstance(buf);
    }
    public void SerializeInt(int field_num, int value) throws IOException{
        if (value != 0) os.writeInt32(field_num, value);
    }
    public void SerializeBool(int field_num, boolean value) throws IOException{
        if (value) os.writeBool(field_num, value);
    }
    public void SerializeString(int field_num, String value) throws IOException{
        if (value != null) os.writeString(field_num, value);
    }
    public void SerializeFloat(int field_num, float value) throws IOException{
        if (value != 0)os.writeFloat(field_num, value);
    }
    public void SerializeMessage(int field_num, int value) throws IOException{
        os.writeMessage(field_num, value);
    }
    public void WriteIntNonSerialized(int value) throws IOException {
        os.writeRawLittleEndian32(value);
    }

    public static byte[] IntToByte(int val){
        byte[] bytes = new byte[4];
        bytes[3] =(byte)( val >> 24 );
        bytes[2] =(byte)( (val << 8) >> 24 );
        bytes[1] =(byte)( (val << 16) >> 24 );
        bytes[0] =(byte)( (val << 24) >> 24 );
        return bytes;
    }
}
