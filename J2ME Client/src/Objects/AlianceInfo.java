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
public class AlianceInfo implements IProtoDeserializable {

    private int id;             //proto 2
    private String name;        //proto 3
    private MemberInfo leader;  //proto 4

    public AlianceInfo(){}

    public int getId(){return id;}
    public String getName(){return name;}
    public MemberInfo getLeader(){return leader;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        id = dsr.readInt(2);
        name = dsr.readString(3);
        int res = dsr.readMessage(4);
        if (res == -1){
            leader = null;
            return;
        }
        leader = new MemberInfo();
        leader.LoadFrom(dsr.getMessageBytes());
    }


}
