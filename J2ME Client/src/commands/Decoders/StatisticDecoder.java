/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Decoders;

import commands.Statistic.GetStatisticResponse;
import commands.Statistic.GetAvailableMapsResponse;
//import Commands.Statistic.*;
import commands.Util.Commands;

/**
 *
 * @author Admin
 */
public class StatisticDecoder extends DefaultDecoder {
    private static StatisticDecoder instance;
    private StatisticDecoder(){}

    public static StatisticDecoder getInstance(){
        if (instance == null) instance = new StatisticDecoder();
        return instance;
    }

    public void recognizeCommand(int c) {
        switch (c){
            case Commands.GET_AVAILABLE_MAPS_RESPONSE: command = new GetAvailableMapsResponse(); break;
            case Commands.GET_STATISTIC_RESPONSE: command = new GetStatisticResponse(); break;
        }
    }


}
