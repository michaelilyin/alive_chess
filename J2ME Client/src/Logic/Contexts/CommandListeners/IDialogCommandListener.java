/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Logic.Contexts.CommandListeners;

import commands.Dialog.BattleDialogMessage;
import commands.Dialog.CaptureCastleDialogMessage;
import commands.Dialog.PeaceDialogMessage;
import commands.Dialog.CreateUnionDialogMessage;
import commands.Dialog.MarketDialogMessage;
import commands.Dialog.PayOffDialogMessage;
import commands.Dialog.WarDialogMessage;
import commands.Dialog.DeactivateDialogMessage;
import commands.Dialog.JoinEmperiesDialogMessage;
import commands.Dialog.CapitulateDialogMessage;
//import Commands.Dialog.*;

/**
 *
 * @author Admin
 */
public interface IDialogCommandListener {
    public void BattleDialogMessReceived(BattleDialogMessage c);
    public void CapitulateDialogMessReceived(CapitulateDialogMessage c);
    public void CaptureCastleDialogMessReceived(CaptureCastleDialogMessage c);
    public void CreateUnionDialogMessReceived(CreateUnionDialogMessage c);
    public void DeactivateDialogMessReceived(DeactivateDialogMessage c);
    public void JoinEmperiesDialogMessReceived(JoinEmperiesDialogMessage c);
    public void MarketDialogMessReceived(MarketDialogMessage c);
    public void PayOffDialogMessReceived(PayOffDialogMessage c);
    public void PeaceDialogMessReceived(PeaceDialogMessage c);
    public void WarDialogMessReceived(WarDialogMessage c);
}
