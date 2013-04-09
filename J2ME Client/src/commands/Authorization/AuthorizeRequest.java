/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Authorization;

import commands.Util.Commands;
import Serializer.Utils.*;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class AuthorizeRequest implements IProtoSerializableRequest {
    private int com_id;        //id number of the command
    private String login;      //first field to protobuf serialization
    private String password;   //second field to protobuf serialization

    public AuthorizeRequest(){
        com_id = Commands.AUTHORISE_REQUEST;
    }

    public void setLogin(String val){
        login = val;
    }

    public void setPassword(String val){
        password = val;
    }

    public int ComputeSize() {
        int len = 0;
        len += ComputeSizeUtil.ComputeString(1, login);
        len += ComputeSizeUtil.ComputeString(2, password);
        return len;
    }

    public byte[] toByte() throws IOException{
        int len = ComputeSize();
        byte[] buf = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(buf);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeString(1, login);
        sr.SerializeString(2, password);
        return buf;
    }

}
