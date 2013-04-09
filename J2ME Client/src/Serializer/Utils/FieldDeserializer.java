/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Serializer.Utils;

import Protobuf.Utils.CodedInputStream;
import Protobuf.Utils.CodedOutputStream;
import Protobuf.Utils.InvalidProtocolBufferException;
import Protobuf.Utils.WireFormat;
import java.io.IOException;
import java.io.InputStream;
import java.util.Stack;
import java.util.Vector;

/**
 *
 * @author Admin
 */
public class FieldDeserializer {
    private int msg_size;
    CodedInputStream is;
    public FieldDeserializer(byte[] buf){
        is = CodedInputStream.newInstance(buf);
    }
    public FieldDeserializer(InputStream _is){
        is =CodedInputStream.newInstance(_is);
    }

    public int readIntNonSerialized() throws IOException{
        return is.readRawLittleEndian32();
    }
    public String readString(int field_n) throws IOException{
        int field_num = getNextFieldNumber();
        if ((field_num == -1)|(field_num != field_n)){
            if (field_num != -1) is.StepBack();
            return null;
        }
        return is.readString();
    }
    public int readInt(int field_n) throws IOException{
        int field_num = getNextFieldNumber();
        if ((field_num == -1)|(field_num != field_n)){
            if (field_num != -1) is.StepBack();
            return 0;
        }
        return is.readInt32();
    }
    public float readFloat(int field_n) throws IOException{
        int field_num = getNextFieldNumber();
        if ((field_num == -1)|(field_num != field_n)){
            if (field_num != -1) is.StepBack();
            return 0;
        }
        return is.readFloat();
    }
    public boolean readBool(int field_n) throws IOException{
        int field_num = getNextFieldNumber();
        if ((field_num == -1)|(field_num != field_n)){
            if (field_num != -1) is.StepBack();
            return false;
        }
        return is.readBool();
    }
    public int readMessage(int field_n) throws IOException{
        int field_num = getNextFieldNumber();
        if ((field_num == -1)|(field_num != field_n)){
            if (field_num != -1) is.StepBack();
            return -1;
        }
        //System.out.println("Message size before changed to " + msg_size);
        msg_size = is.readMessageStart();
        //System.out.println("Message size after changed to " + msg_size);
        return msg_size;
    }
    public int getNextFieldNumber() throws IOException  {
        if (is.DoesntHaveMore()) return -1;
        int result = 0;
        try{
            int tag = is.readTag();
            result = WireFormat.getTagFieldNumber(tag);
        }catch (Exception e){
            if (e.getMessage().compareTo("Protocol message contained an invalid tag (zero).") == 0)
                result = 0;
        }
        return result;
    }
    public void StepBack() throws IOException{
        is.StepBack();
    }
    public byte[] getMessageBytes(){
        byte[] result = is.getMessageInBytes(msg_size);
        return result;
    }
}
