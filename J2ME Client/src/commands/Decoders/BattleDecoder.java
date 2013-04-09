/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Decoders;

import commands.Battle.MoveUnitResponse;
import commands.Battle.DownloadBattlefieldResponse;
//import Commands.Battle.*;
import commands.Util.Commands;

/**
 *
 * @author Admin
 */
public class BattleDecoder extends DefaultDecoder {

    private static BattleDecoder instance;

    private BattleDecoder(){}

    public static BattleDecoder getInstance(){
        if (instance == null)  instance = new BattleDecoder();
        return instance;
    }

    public void recognizeCommand(int c){
        switch(c){
            case Commands.BATTLE_MESSAGE: break;
            case Commands.DOWNLOAD_BATTLEFIELD_RESPONSE: command = new DownloadBattlefieldResponse(); break;
            case Commands.MOVE_UNIT_RESPONSE: command = new MoveUnitResponse(); break;
        }
    }
}
