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
public class GetHelpFigureRequest implements IProtoSerializableRequest {

    private int com_id;
    private int figureCount;

    private int figureType;
        //Knight = 0,
        //Queen  = 1,
        //Rook   = 2,
        //Bishop = 3,
        //Pawn   = 10

    public GetHelpFigureRequest(){
        com_id = Commands.GET_HELP_FIGURE_REQUEST;
    }

    public void setFigureCount(int value){
        figureCount = value;
    }
    public void setFigureType(int value){
        figureType = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, figureCount)
               + ComputeSizeUtil.ComputeInt(2, figureType);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, figureCount);
        sr.SerializeInt(2, figureType);
        return result;
    }

}
