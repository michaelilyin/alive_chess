/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Logic.Contexts.CommandListeners;

import commands.Castle.LeaveCastleResponse;
import commands.Castle.BuildingInCastleResponse;
import commands.Castle.GetRecBuildingsResponse;
import commands.Castle.GetArmyCastleToKingResponse;
import commands.Castle.GetListBuildingsInCastleResponse;
import commands.Castle.BuyFigureResponse;
//import Commands.Castle.*;

/**
 *
 * @author Admin
 */
public interface ICastleCommandListener {
    public void BuildingInCastleResponseReceived(BuildingInCastleResponse c);
    public void BuyFigureResponseReceived(BuyFigureResponse c);
    public void GetArmyCastleToKingResponseReceived(GetArmyCastleToKingResponse c);
    public void GetListBuildingsInCastleResponseReceived(GetListBuildingsInCastleResponse c);
    public void GetRecBuildingsResponseReceived(GetRecBuildingsResponse c);
    public void LeaveCastleResponseReceived(LeaveCastleResponse c);
}
