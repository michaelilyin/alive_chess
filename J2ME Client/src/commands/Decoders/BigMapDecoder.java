/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Decoders;

import commands.BigMap.CaptureMineResponse;
import commands.BigMap.LooseMineMessage;
import commands.BigMap.CaptureCastleResponse;
import commands.BigMap.ComputePathResponse;
import commands.BigMap.UpdateWorldMessage;
import commands.BigMap.GetUnityMapResponse;
import commands.BigMap.GetGameStateResponse;
import commands.BigMap.GetMapResponse;
import commands.BigMap.GetResourceMessage;
import commands.BigMap.GetKingResponse;
import commands.BigMap.LooseCastleMessage;
import commands.BigMap.ContactKingResponse;
import commands.BigMap.TakeResourceMessage;
import commands.BigMap.MoveKingResponse;
import commands.BigMap.ComeInCastleResponse;
import commands.BigMap.BigMapResponse;
import commands.BigMap.GetObjectsResponse;
import commands.BigMap.ContactCastleResponse;
import commands.BigMap.VerifyPathResponse;
//import Commands.BigMap.*;
import commands.Util.Commands;

/**
 *
 * @author Admin
 */
public class BigMapDecoder extends DefaultDecoder {
    
    private static BigMapDecoder instance;

    private BigMapDecoder(){}

    public static BigMapDecoder getInstance(){
        if (instance == null)  instance = new BigMapDecoder();
        return instance;
    }


    public void recognizeCommand(int c){
        
        switch(c){
            case Commands.BIG_MAP_RESPONSE: command = new BigMapResponse(); break;
            case Commands.CAPTURE_CASTLE_RESPONSE: command = new CaptureCastleResponse(); break;
            case Commands.CAPTURE_MINE_RESPONSE: command = new CaptureMineResponse(); break;
            case Commands.COME_IN_CASTLE_RESPONSE: command = new ComeInCastleResponse(); break;
            case Commands.CONTACT_KING_RESPONSE: command = new ContactKingResponse(); break;
            case Commands.TAKE_RESOURCE_MESSAGE: command = new TakeResourceMessage(); break;
            case Commands.MOVE_KING_RESPONSE: command = new MoveKingResponse(); break;
            case Commands.CONTACT_CASTLE_RESPONSE: command = new ContactCastleResponse(); break;
            case Commands.GET_MAP_RESPONSE: command = new GetMapResponse(); break;
            case Commands.GET_OBJECTS_RESPONSE: command = new GetObjectsResponse(); break;
            case Commands.LOOSE_CASTLE_MESSAGE: command = new LooseCastleMessage(); break;
            case Commands.LOOSE_MINE_MESSAGE: command = new LooseMineMessage(); break;
            case Commands.UPDATE_WORLD_MESSAGE: command = new UpdateWorldMessage(); break;
            case Commands.GET_RESOURCE_MESSAGE: command = new GetResourceMessage(); break;
            case Commands.GET_UNITY_MAP_RESPONSE: command = new GetUnityMapResponse(); break;
            case Commands.GET_GAME_STATE_RESPONSE: command = new GetGameStateResponse(); break;
            case Commands.COMPUTE_PATH_RESPONSE: command = new ComputePathResponse(); break;
            case Commands.VERIFY_PATH_RESPONSE: command = new VerifyPathResponse(); break;
            case Commands.GET_KING_RESPONSE: command = new GetKingResponse(); break;
        }
    }
}
