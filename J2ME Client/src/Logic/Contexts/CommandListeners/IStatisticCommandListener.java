/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Logic.Contexts.CommandListeners;

import commands.Statistic.GetStatisticResponse;
import commands.Statistic.GetAvailableMapsResponse;
//import Commands.Statistic.*;

/**
 *
 * @author Admin
 */
public interface IStatisticCommandListener {
    public void GetAvailableMapsResponseReceived(GetAvailableMapsResponse c);
    public void GetStatisticResponseReceived(GetStatisticResponse c);
}
