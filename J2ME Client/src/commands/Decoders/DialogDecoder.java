/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Decoders;

import commands.Dialog.BattleDialogMessage;
import commands.Dialog.CaptureCastleDialogMessage;
import commands.Dialog.PeaceDialogMessage;
import commands.Dialog.CreateUnionDialogMessage;
import commands.Dialog.PayOffDialogMessage;
import commands.Dialog.MarketDialogMessage;
import commands.Dialog.WarDialogMessage;
import commands.Dialog.DeactivateDialogMessage;
import commands.Dialog.JoinEmperiesDialogMessage;
import commands.Dialog.CapitulateDialogMessage;
//import Commands.Dialog.*;
import commands.Util.Commands;

/**
 *
 * @author Admin
 */
public class DialogDecoder extends DefaultDecoder {

    private static DialogDecoder instance;

    private DialogDecoder(){}

    public static DialogDecoder getInstance(){
        if (instance == null)  instance = new DialogDecoder();
        return instance;
    }

    public void recognizeCommand(int c) {
        switch (c){
            case Commands.BATTLE_DIALOG_MESSAGE: command = new BattleDialogMessage(); break;
            case Commands.PAY_OFF_DIALOG_MESSAGE: command = new PayOffDialogMessage(); break;
            case Commands.MARKET_DIALOG_MESSAGE: command = new MarketDialogMessage(); break;
            case Commands.CAPITULATE_DIALOG_MESSAGE: command = new CapitulateDialogMessage(); break;
            case Commands.CAPTURE_CASTLE_DIALOG_MESSAGE: command = new CaptureCastleDialogMessage(); break;
            case Commands.DEACTIVATE_DIALOG_MESSAGE: command = new DeactivateDialogMessage(); break;
            case Commands.CREATE_UNION_DIALOG_MESSAGE: command = new CreateUnionDialogMessage(); break;
            case Commands.WAR_DIALOG_MESSAGE: command = new WarDialogMessage(); break;
            case Commands.PEACE_DIALOG_MESSAGE: command = new PeaceDialogMessage(); break;
            case Commands.JOIN_EMPERIES_DIALOG_MESSAGE: command = new JoinEmperiesDialogMessage(); break;

        }
    }

    
}
