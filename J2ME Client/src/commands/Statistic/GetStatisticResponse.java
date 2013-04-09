/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Statistic;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IStatisticCommandListener;
import Objects.Statistic;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class GetStatisticResponse implements IProtoDeserializable, ICommand {
    private Statistic statistic;    //proto 1

    public GetStatisticResponse(){}

    public Statistic getStatistic(){return statistic;}

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        int res = dsr.readMessage(1);
        if (res == -1){
            statistic = null;
            return;
        }
        statistic = new Statistic();
        statistic.LoadFrom(dsr.getMessageBytes());
    }

    public void Execute(Object listener) {
        IStatisticCommandListener l = (IStatisticCommandListener)listener;
        l.GetStatisticResponseReceived(this);
    }


}
