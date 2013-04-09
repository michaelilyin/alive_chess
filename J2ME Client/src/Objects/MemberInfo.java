/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Objects;

import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class MemberInfo implements IProtoDeserializable {

    private int memberId;          //proto 3
    private String memberName;     //proto 4

    public MemberInfo(){}

    public int getMemerId(){return memberId;}
    public String getMemberName(){return memberName;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        memberId = dsr.readInt(3);
        memberName = dsr.readString(4);
    }


}
