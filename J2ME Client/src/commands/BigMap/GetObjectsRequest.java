/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.BigMap;

import commands.Util.Commands;
import Serializer.Utils.*;
//import Commands.Util.*;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class GetObjectsRequest implements IProtoSerializableRequest {
    private int com_id;
    private int observerId;
    private boolean forConcreteObserver;

    private int observerType;
    //    None         = 0,
    //    King         = 1, // король
    //    Castle       = 2, // замок
    //    Mine         = 3, // шахта
    //    Resource     = 4, // ресурс
    //    SingleObject = 5, // оьъект, занимающий одну ячейку
    //    MultyObject  = 6, // объект, занимающий несколько ячеек
    //    Landscape    = 7, // тип местности


    public GetObjectsRequest(){
        com_id = Commands.GET_OBJECTS_REQUEST;
    }

    public void setObserverId(int value){
        observerId = value;
    }
    public void setForConcreteObserver(boolean value){
        forConcreteObserver = value;
    }
    public void setObserverType(int value){
        observerType = value;
    }

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, observerId)+
               ComputeSizeUtil.ComputeBool(2, forConcreteObserver)+
               ComputeSizeUtil.ComputeInt(3, observerType);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer fs = new FieldSerializer(result);
        fs.WriteIntNonSerialized(com_id);
        fs.WriteIntNonSerialized(len);
        fs.SerializeInt(1, observerId);
        fs.SerializeBool(2, forConcreteObserver);
        fs.SerializeInt(3, observerType);
        return result;
    }
}
