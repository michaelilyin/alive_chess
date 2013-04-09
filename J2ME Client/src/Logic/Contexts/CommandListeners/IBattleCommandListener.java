/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Logic.Contexts.CommandListeners;

import commands.Battle.MoveUnitResponse;
import commands.Battle.DownloadBattlefieldResponse;
//import Commands.Battle.*;

/**
 *
 * @author Admin
 */
public interface IBattleCommandListener {
    public void DownloadBattlefieldResponseReceived(DownloadBattlefieldResponse c);
    public void MoveUnitResponseReceived(MoveUnitResponse c);
}
