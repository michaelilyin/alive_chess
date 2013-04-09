/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Logic.Contexts.CommandListeners;

import commands.BigMap.CaptureMineResponse;
import commands.BigMap.LooseMineMessage;
import commands.BigMap.ComputePathResponse;
import commands.BigMap.CaptureCastleResponse;
import commands.BigMap.UpdateWorldMessage;
import commands.BigMap.GetUnityMapResponse;
import commands.BigMap.GetGameStateResponse;
import commands.BigMap.GetMapResponse;
import commands.BigMap.GetResourceMessage;
import commands.BigMap.GetKingResponse;
import commands.BigMap.ContactKingResponse;
import commands.BigMap.LooseCastleMessage;
import commands.BigMap.TakeResourceMessage;
import commands.BigMap.ComeInCastleResponse;
import commands.BigMap.BigMapResponse;
import commands.BigMap.MoveKingResponse;
import commands.BigMap.GetObjectsResponse;
import commands.BigMap.ContactCastleResponse;
import commands.BigMap.VerifyPathResponse;
//import Commands.BigMap.*;

/**
 *
 * @author Admin
 */
public interface IBigMapCommandListener {
    public void BigMapResponseReceived(BigMapResponse c);
    public void CaptureCastleResponseReceived(CaptureCastleResponse c);
    public void CaptureMineResponseReceived(CaptureMineResponse c);
    public void ComeInCastleResponseReceived(ComeInCastleResponse c);
    public void ComputePathResponseReceived(ComputePathResponse c);
    public void ContactCastleResponseReceived(ContactCastleResponse c);
    public void ContactKingResponseReceived(ContactKingResponse c);
    public void GetGameStateResponseReceived(GetGameStateResponse c);
    public void GetKingResponseReceived(GetKingResponse c);
    public void GetMapResponseReceived(GetMapResponse c);
    public void GetObjectsResponseReceived(GetObjectsResponse c);
    public void GetResourceMessReceived(GetResourceMessage c);
    public void GetUnityMapResponseReceived(GetUnityMapResponse c);
    public void LooseCastleMessReceived(LooseCastleMessage c);
    public void LooseMineMessReceived(LooseMineMessage c);
    public void MoveKingResponseReceived(MoveKingResponse c);
    public void TakeResourceMessReceived(TakeResourceMessage c);
    public void UpdateWorldMessReceived(UpdateWorldMessage c);
    public void VerifyPathResponseReceived(VerifyPathResponse c);
}
