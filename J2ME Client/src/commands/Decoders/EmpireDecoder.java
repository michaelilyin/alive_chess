/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Decoders;

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
import commands.Util.Commands;

/**
 *
 * @author Admin
 */
public class EmpireDecoder extends DefaultDecoder {

    private static EmpireDecoder instance;

    private EmpireDecoder(){}

    public static EmpireDecoder getInstance(){
        if (instance == null) instance = new EmpireDecoder();
        return instance;
    }

    public void recognizeCommand(int c) {
        switch (c){
            case Commands.EMBED_TAX_RATE_RESPONSE: command = new EmbedTaxRateResponse(); break;
            case Commands.EXCLUDE_FROM_EMPIRE_MESSAGE: command = new ExcludeFromEmpireMessage(); break;
            case Commands.EXCLUDE_KING_FROM_EMPIRE_RESPONSE: command = new ExcludeKingFromEmpireResponse(); break;
            case Commands.EXIT_FROM_ALIANCE_RESPONSE: command = new ExitFromAlianceResponse(); break;
            case Commands.GET_ALIANCE_INFO_RESPONSE: command = new GetAlianceInfoResponse(); break;
            case Commands.GET_ALIANCES_INFO_RESPONSE: command = new GetAliancesInfoResponse(); break;
            case Commands.GET_HELP_FIGURE_RESPONSE: command = new GetHelpFigureResponse(); break;
            case Commands.GET_HELP_RESOURCE_RESPONSE: command = new GetHelpResourceResponse(); break;
            case Commands.GRAND_LEADER_PRIVILEGES_MESSAGE: command = new GrandLeaderPrivilegesMessage(); break;
            case Commands.INCLUDE_KING_IN_EMPIRE_RESPONSE: command = new IncludeKingInEmpireResponse(); break;
            case Commands.JOIN_REQUEST_MESSAGE: command = new JoinRequestMessage(); break;
            case Commands.JOIN_TO_ALIANCE_RESPONSE: command = new JoinToAlianceResponse(); break;
            case Commands.MESSAGE_NEWS_MESSAGE: command = new MessageNewsMessage(); break;
            case Commands.SEND_FIGURE_HELP_MESSAGE: command = new SendFigureHelpMessage(); break;
            case Commands.SEND_RESOURCE_HELP_MESSAGE: command = new SendResourceHelpMessage(); break;
            case Commands.START_IMPEACHMENT_RESPONSE: command = new StartImpeachmentResponse(); break;
            case Commands.START_NEGOTIATE_RESPONSE: command = new StartNegotiateResponse(); break;
            case Commands.START_VOTE_RESPONSE: command = new StartVoteResponse(); break;
            case Commands.TAKE_AWAY_LEADER_PRIVILEGES_MESSAGE: command = new TakeAwayLeaderPrivilegesMessage(); break;
            case Commands.VOTE_BALLOT_MESSAGE: command = new VoteBallotMessage();
        }
    }


}
