/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Logic.Contexts.CommandListeners;

import commands.Empire.SendFigureHelpMessage;
import commands.Empire.GrandLeaderPrivilegesMessage;
import commands.Empire.MessageNewsMessage;
import commands.Empire.ExcludeKingFromEmpireResponse;
import commands.Empire.EmbedTaxRateResponse;
import commands.Empire.JoinToAlianceResponse;
import commands.Empire.StartVoteResponse;
import commands.Empire.ExcludeFromEmpireMessage;
import commands.Empire.StartImpeachmentResponse;
import commands.Empire.GetHelpFigureResponse;
import commands.Empire.JoinRequestMessage;
import commands.Empire.SendResourceHelpMessage;
import commands.Empire.TakeAwayLeaderPrivilegesMessage;
import commands.Empire.VoteBallotMessage;
import commands.Empire.GetHelpResourceResponse;
import commands.Empire.StartNegotiateResponse;
import commands.Empire.GetAliancesInfoResponse;
import commands.Empire.GetAlianceInfoResponse;
import commands.Empire.ExitFromAlianceResponse;
import commands.Empire.IncludeKingInEmpireResponse;
//import Commands.Empire.*;

/**
 *
 * @author Admin
 */
public interface IEmpireCommandListener {

    public void EmbedTaxRateResponseReceived(EmbedTaxRateResponse c);
    public void ExcludeFromEmpireMessReceived(ExcludeFromEmpireMessage c);
    public void ExcludeKingFromEmpireResponseReceived(ExcludeKingFromEmpireResponse c);
    public void ExitFromAlianceResponseReceived(ExitFromAlianceResponse c);
    public void GetAlianceInfoResponseReceived(GetAlianceInfoResponse c);
    public void GetAliancesInfoResponseReceived(GetAliancesInfoResponse c);
    public void GetHelpFigureResponseReceived(GetHelpFigureResponse c);
    public void GetHelpResourceResponseReceived(GetHelpResourceResponse c);
    public void GrandLeaderPrivilegesMessReceived(GrandLeaderPrivilegesMessage c);
    public void IncludeKingInEmpireResponseReceived(IncludeKingInEmpireResponse c);
    public void JoinRequestMessReceived(JoinRequestMessage c);
    public void JoinToAlianceResponseReceived(JoinToAlianceResponse c);
    public void MessageNewsMessReceived(MessageNewsMessage c);
    public void SendFigureHelpMessReceived(SendFigureHelpMessage c);
    public void SendResourceHelpMessReceived(SendResourceHelpMessage c);
    public void StartImpeachmentResponseReceived(StartImpeachmentResponse c);
    public void StartNegotiateResponseReceived(StartNegotiateResponse c);
    public void StartVoteResponseReceived(StartVoteResponse c);
    public void TakeAwayLeaderPrivilegesMessReceived(TakeAwayLeaderPrivilegesMessage c);
    public void VoteBallotMessReceived(VoteBallotMessage c);
}
