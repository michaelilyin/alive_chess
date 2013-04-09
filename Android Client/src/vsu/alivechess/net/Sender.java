/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package vsu.alivechess.net;

import java.util.LinkedList;
import java.util.Queue;

import vsu.alivechess.net.commands.AliveChessProtos.AuthorizeRequest;
import vsu.alivechess.net.commands.AliveChessProtos.CaptureCastleRequest;
import vsu.alivechess.net.commands.AliveChessProtos.CaptureMineRequest;
import vsu.alivechess.net.commands.AliveChessProtos.ComeInCastleRequest;
import vsu.alivechess.net.commands.AliveChessProtos.ExitFromGameRequest;
import vsu.alivechess.net.commands.AliveChessProtos.GetGameStateRequest;
import vsu.alivechess.net.commands.AliveChessProtos.GetMapRequest;
import vsu.alivechess.net.commands.AliveChessProtos.GetObjectsRequest;
import vsu.alivechess.net.commands.AliveChessProtos.LeaveCastleRequest;
import vsu.alivechess.net.commands.AliveChessProtos.MoveKingRequest;
import vsu.alivechess.net.commands.AliveChessProtos.MoveKingResponse;
import vsu.alivechess.net.commands.AliveChessProtos.PointTypes;
import vsu.alivechess.net.commands.AliveChessProtos.RegisterRequest;
import vsu.alivechess.net.commands.Commands;
import vsu.alivechess.net.executors.AuthorizeExecutor;
import vsu.alivechess.net.executors.CaptureCastleExecutor;
import vsu.alivechess.net.executors.CaptureMineExecutor;
import vsu.alivechess.net.executors.ComeInCastleExecutor;
import vsu.alivechess.net.executors.ExitFromGameExecutor;
import vsu.alivechess.net.executors.GetGameStateExecutor;
import vsu.alivechess.net.executors.GetMapExecutor;
import vsu.alivechess.net.executors.GetObjectsExecutor;
import vsu.alivechess.net.executors.LeaveCastleExecutor;
import vsu.alivechess.net.executors.MoveKingExecutor;
import vsu.alivechess.net.executors.RegisterExecutor;
import vsu.alivechess.utils.ByteHelper;

/**
 *
 * @author Al-mal
 */
public class Sender {
    private static Sender instance;
    private Queue<byte[]> messages;

    public Sender(){
        messages = new LinkedList<byte[]>();
    }

    public void addMessage(byte[] msg){
    	synchronized (messages) {
            messages.add(msg);			
		}
    }

    public void createMessage(int cmdId, byte[] msg){
        byte[] cmdName = ByteHelper.intToByteArray(cmdId);
        byte[] cmdSize = ByteHelper.intToByteArray(msg.length);
        byte[] sendMsg = ByteHelper.concatBytes(cmdName, cmdSize, msg);
        addMessage(sendMsg);
    }

    public byte[] getMessage(){
    	byte[] msg;
    	synchronized (messages) {
    		msg = messages.poll();
		}
        return msg;
    }

    public void sendRegister(String login, String pass, RegisterExecutor eReg){
        RegisterRequest regReq = RegisterRequest.newBuilder()
                .setLogin(login)
                .setPassword(pass)
                .build();

        Receiver.getInstance().addExecutor(eReg);
        createMessage(Commands.RegisterRequest, regReq.toByteArray());
    }

    public void sendAuthorize(String login, String pass, AuthorizeExecutor eAuth){
        AuthorizeRequest authReq = AuthorizeRequest.newBuilder()
                .setLogin(login)
                .setPassword(pass)
                .build();

        Receiver.getInstance().addExecutor(eAuth);
        createMessage(Commands.AuthorizeRequest, authReq.toByteArray());
    }

    public void sendGetMap(GetMapExecutor exec){
        GetMapRequest req = GetMapRequest.getDefaultInstance();
        Receiver.getInstance().addExecutor(exec);
        createMessage(Commands.GetMapRequest, req.toByteArray());
    }

    public void sendGetGameState(GetGameStateExecutor exec){
        GetGameStateRequest req = GetGameStateRequest.getDefaultInstance();
        Receiver.getInstance().addExecutor(exec);
        createMessage(Commands.GetGameStateRequest, req.toByteArray());
    }

    public void sendGetObjects(boolean conctretObserver, int observerId,
            PointTypes pType){
        GetObjectsRequest req = GetObjectsRequest.newBuilder()
                .setForConcreteObserver(conctretObserver)
                .setObserverId(observerId)
                .setObserverType(pType)
                .build();
        createMessage(Commands.GetObjectsRequest, req.toByteArray());
    }
    
    public void sendMoveKing(int x, int y, MoveKingExecutor exec){
    	MoveKingRequest req = MoveKingRequest.newBuilder()
    		.setX(x)
    		.setY(y)
    		.build();
        Receiver.getInstance().addExecutor(exec);
        createMessage(Commands.MoveKingRequest, req.toByteArray());
    }

    public void sendCaptureMine(int mineId, CaptureMineExecutor exec){
    	CaptureMineRequest req = CaptureMineRequest.newBuilder()
    		.setMineId(mineId)
    		.build();
        Receiver.getInstance().addExecutor(exec);
        createMessage(Commands.CaptureMineRequest, req.toByteArray());    	
    }
    
    public void sendCaptureCastle(int castleId, CaptureCastleExecutor exec){
    	CaptureCastleRequest req = CaptureCastleRequest.newBuilder()
    		.setCastleId(castleId)
    		.build();
        Receiver.getInstance().addExecutor(exec);
        createMessage(Commands.CaptureCastleRequest, req.toByteArray());    	
    }
    
    public void sendComeInCastle(int castleId, ComeInCastleExecutor exec){
    	ComeInCastleRequest req = ComeInCastleRequest.newBuilder()
    		.setCastleId(castleId)
    		.build();
    	Receiver.getInstance().addExecutor(exec);
    	createMessage(Commands.ComeInCastleRequest, req.toByteArray());
    }
    
    public void sendLeaveCastle(LeaveCastleExecutor exec){
    	LeaveCastleRequest req = LeaveCastleRequest.getDefaultInstance();
    	Receiver.getInstance().addExecutor(exec);
    	createMessage(Commands.LeaveCastleRequest, req.toByteArray());
    }
    
    public void sendExitFromGame(ExitFromGameExecutor exec){
    	ExitFromGameRequest req = ExitFromGameRequest.getDefaultInstance();
    	Receiver.getInstance().addExecutor(exec);
    	createMessage(Commands.ExitFromGameRequest, req.toByteArray());
    }
    
    public static Sender getInstance(){
        if(instance == null)
            instance = new Sender();
        return instance;
    }
}
