/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.Commands;
import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class EmbedTaxRateRequest implements IProtoSerializableRequest {

    private int com_id;
    private int rate;

    public EmbedTaxRateRequest(){
        com_id = Commands.EMBED_TAX_RATE_REQUEST;
    }

    public void setRate(int value){
        rate = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, rate);
    }

    public byte[] toByte() throws IOException{
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, rate);
        return result;
    }

}
