/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Decoders;

import commands.Castle.LeaveCastleResponse;
import commands.Castle.BuildingInCastleResponse;
import commands.Castle.GetRecBuildingsResponse;
import commands.Castle.GetArmyCastleToKingResponse;
import commands.Castle.GetListBuildingsInCastleResponse;
import commands.Castle.BuyFigureResponse;
//import Commands.Castle.*;
import commands.Util.Commands;

/**
 *
 * @author Admin
 */
public class CastleDecoder extends DefaultDecoder{

    private static CastleDecoder instance;
    private CastleDecoder(){}

    public static CastleDecoder getInstance(){
        if (instance == null) instance = new CastleDecoder();
        return instance;
    }

    public void recognizeCommand(int c) {
        switch (c){
            case Commands.BUILDING_IN_CASTLE_RESPONSE: command = new BuildingInCastleResponse(); break;
            case Commands.BUY_FIGURE_RESPONSE: command = new BuyFigureResponse(); break;
            case Commands.GET_ARMY_CASTLE_TO_KING_RESPONSE: command = new GetArmyCastleToKingResponse(); break;
            case Commands.GET_LIST_BUILDINGS_IN_CASTLE_RESPONSE: command = new GetListBuildingsInCastleResponse(); break;
            case Commands.GET_REC_BUILDINGS_RESPONSE: command = new GetRecBuildingsResponse(); break;
            case Commands.LEAVE_CASTLE_RESPONSE: command = new LeaveCastleResponse(); break;
        }
    }


}
