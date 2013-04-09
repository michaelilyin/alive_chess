/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Serializer.Utils;

import Protobuf.Utils.*;

/**
 *
 * @author Admin
 */
public class ComputeSizeUtil {
    public static int ComputeInt(int field_num, int value){
        if (value != 0) return CodedOutputStream.computeInt32Size(field_num, value);
        return 0;
    }
    public static int ComputeBool(int field_num, boolean value){
        if (value) return CodedOutputStream.computeBoolSize(field_num, value);
        return 0;
    }
    public static int ComputeString(int field_num, String value){
        if (value != null) return CodedOutputStream.computeStringSize(field_num, value);
        return 0;
    }
    public static int ComputeFloat(int field_num, float value){
        if (value != 0) return CodedOutputStream.computeFloatSize(field_num, value);
        return 0;
    }
    public static int ComputeMessage(int field_num, int value){
        return CodedOutputStream.computeTagSize(field_num)
                + CodedOutputStream.computeRawVarint32Size(value);
    }
}
