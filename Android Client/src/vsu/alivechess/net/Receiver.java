/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package vsu.alivechess.net;

import com.google.protobuf.InvalidProtocolBufferException;
import com.google.protobuf.Message;
import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;
import java.util.logging.Level;
import java.util.logging.Logger;

import vsu.alivechess.net.commands.AliveChessProtos.AuthorizeResponse;
import vsu.alivechess.net.commands.AliveChessProtos.CaptureCastleResponse;
import vsu.alivechess.net.commands.AliveChessProtos.CaptureMineResponse;
import vsu.alivechess.net.commands.AliveChessProtos.ComeInCastleResponse;
import vsu.alivechess.net.commands.AliveChessProtos.ErrorMessage;
import vsu.alivechess.net.commands.AliveChessProtos.ExitFromGameResponse;
import vsu.alivechess.net.commands.AliveChessProtos.GetGameStateResponse;
import vsu.alivechess.net.commands.AliveChessProtos.GetMapResponse;
import vsu.alivechess.net.commands.AliveChessProtos.GetObjectsResponse;
import vsu.alivechess.net.commands.AliveChessProtos.GetResourceMessage;
import vsu.alivechess.net.commands.AliveChessProtos.LeaveCastleResponse;
import vsu.alivechess.net.commands.AliveChessProtos.LooseCastleMessage;
import vsu.alivechess.net.commands.AliveChessProtos.LooseMineMessage;
import vsu.alivechess.net.commands.AliveChessProtos.MoveKingResponse;
import vsu.alivechess.net.commands.AliveChessProtos.RegisterResponse;
import vsu.alivechess.net.commands.AliveChessProtos.UpdateWorldMessage;
import vsu.alivechess.net.commands.Commands;
import vsu.alivechess.net.executors.ErrorExecutor;
import vsu.alivechess.net.executors.IExecutor;
import vsu.alivechess.net.executors.UpdateWorldExecutor;
import vsu.alivechess.utils.ByteHelper;

/**
 *
 * @author Al-mal
 */
public class Receiver {
    public static Receiver instance;

    public static final String PACKAGE_EXECUTORS = "vsu.alivechess.net.executors";
    
    private List<IExecutor> executors;
    private Queue<IExecutor> finishedExecutors;

    private byte[] bytes = new byte[0];
    
    public Receiver(){
        executors = new ArrayList<IExecutor>();
        finishedExecutors = new LinkedList<IExecutor>();

        new Thread(new ExecutorRunnable()).start();
    }
    
    public static Receiver getInstance(){
        if(instance == null){
            instance = new Receiver();
        }
        return instance;
    }

    public void receiveMessage(byte[] msg)
            throws InvalidProtocolBufferException, ClassNotFoundException{
    	bytes = ByteHelper.concatBytes(bytes, msg);
    	
    	int msgId = ByteHelper.byteArrayToInt(
                ByteHelper.getPartByteArray(bytes, 0, 4));
        int msgSize = ByteHelper.byteArrayToInt(
                ByteHelper.getPartByteArray(bytes, 4, 4));
        
        if(msgSize > bytes.length - 8){
        	return;
        }

        byte[] rCmd = ByteHelper.getPartByteArray(bytes, 8, msgSize);
        
        bytes = ByteHelper.getPartByteArray(bytes, msgSize+8, 
        		bytes.length-msgSize-8);
        if(bytes.length != 0)
        	receiveMessage(new byte[0]);
        
        Message resp;
        
        switch(msgId){
            case Commands.RegisterResponse:
                receiveRegister(rCmd);
                break;
            case Commands.AuthorizeResponse:
                receiveAuthorize(rCmd);
                break;
            case Commands.GetMapResponse:
                receiveGetMap(rCmd);
                break;
            case Commands.GetGameStateResponse:
                receiveGetGameState(rCmd);
                break;
            case Commands.ErrorMessage:
                receiveError(rCmd);
                break;
            case Commands.MoveKingResponse:
            	receiveMoveKing(rCmd);
            	break;
            case Commands.GetObjectsResponse:
            	receiveGetObjects(rCmd);
            	break;
            case Commands.CaptureCastleResponse:
            	receiveCaptureCastle(rCmd);
            	break;
            case Commands.CaptureMineResponse:
            	receiveCaptureMine(rCmd);
            	break;
            case Commands.ComeInCastleResponse:
            	receiveComeInCastle(rCmd);
            	break;
            case Commands.GetResourceMessage:
        		resp = GetResourceMessage.parseFrom(rCmd);
        		setExecutor("GetResourceExecutor", resp);            	
            	break;
            case Commands.LooseCastleMessage:
        		resp = LooseCastleMessage.parseFrom(rCmd);
        		setExecutor("LooseCastleExecutor", resp);            	            	
            	break;
            case Commands.LooseMineMessage:
        		resp = LooseMineMessage.parseFrom(rCmd);
        		setExecutor("LooseMineExecutor", resp);            	            	
            	break;
            case Commands.LeaveCastleResponse:
            	resp = LeaveCastleResponse.parseFrom(rCmd);
            	setExecutorAndRemove("LeaveCastleExecutor", resp);
            	break;
            case Commands.ExitFromGameResponse:
            	resp = ExitFromGameResponse.parseFrom(rCmd);
            	setExecutorAndRemove("ExitFromGameExecutor", resp);
            	break;
            case Commands.UpdateWorldMessage:
            	resp = UpdateWorldMessage.parseFrom(rCmd);
            	setExecutor("UpdateWorldExecutor", resp);
            	break;
        }
    }

	private IExecutor setExecutor(String nameExecutor, Message responce)
            throws ClassNotFoundException{

        String fullNameExecutor = PACKAGE_EXECUTORS + "." + nameExecutor;
        Class execClass = Class.forName(fullNameExecutor);
        for (IExecutor executor : executors) {
            if(execClass.isInstance(executor)){
                executor.setResponce(responce);
                synchronized (finishedExecutors) {
                	finishedExecutors.add(executor);
				}
                return executor;
            }
        }
        return null;
    }
	
	private void setExecutorAndRemove(String nameExecutor, Message responce) 
			throws ClassNotFoundException{
		
		IExecutor executor = setExecutor(nameExecutor, responce);
		executors.remove(executor);
	}

    private void receiveRegister(byte[] rCmd) 
            throws InvalidProtocolBufferException, ClassNotFoundException{

        RegisterResponse regRes = RegisterResponse.parseFrom(rCmd);
        setExecutorAndRemove("RegisterExecutor", regRes);
    }

    private void receiveAuthorize(byte[] rCmd) 
            throws InvalidProtocolBufferException, ClassNotFoundException{

        AuthorizeResponse authRes = AuthorizeResponse.parseFrom(rCmd);
        setExecutorAndRemove("AuthorizeExecutor", authRes);
    }

    private void receiveGetMap(byte[] rCmd) 
            throws InvalidProtocolBufferException, ClassNotFoundException{

        GetMapResponse getMapRes = GetMapResponse.parseFrom(rCmd);
        setExecutorAndRemove("GetMapExecutor", getMapRes);
    }

    private void receiveGetGameState(byte[] rCmd)
            throws InvalidProtocolBufferException, ClassNotFoundException{

        GetGameStateResponse resp = GetGameStateResponse.parseFrom(rCmd);
        setExecutorAndRemove("GetGameStateExecutor", resp);
    }

    private void receiveMoveKing(byte[] rCmd) 
    		throws InvalidProtocolBufferException, ClassNotFoundException {
		
    	MoveKingResponse resp = MoveKingResponse.parseFrom(rCmd);
    	setExecutorAndRemove("MoveKingExecutor", resp);
	}
    
	private void receiveGetObjects(byte[] rCmd) 
			throws InvalidProtocolBufferException, ClassNotFoundException {
		
		GetObjectsResponse resp = GetObjectsResponse.parseFrom(rCmd);
		setExecutor("GetObjectsExecutor", resp);
	}
    
	private void receiveCaptureCastle(byte[] rCmd)
			throws InvalidProtocolBufferException, ClassNotFoundException {
		
		CaptureCastleResponse resp = CaptureCastleResponse.parseFrom(rCmd);
		setExecutorAndRemove("CaptureCastleExecutor", resp);
	}

	private void receiveCaptureMine(byte[] rCmd)
		throws InvalidProtocolBufferException, ClassNotFoundException {
		
		CaptureMineResponse resp = CaptureMineResponse.parseFrom(rCmd);
		setExecutorAndRemove("CaptureMineExecutor", resp);
	}	
	
	private void receiveComeInCastle(byte[] rCmd) 
			throws InvalidProtocolBufferException, ClassNotFoundException {
		
		ComeInCastleResponse resp = ComeInCastleResponse.parseFrom(rCmd);
		setExecutorAndRemove("ComeInCastleExecutor", resp);
	}
	
    private void receiveError(byte[] rCmd) 
    		throws InvalidProtocolBufferException, ClassNotFoundException{
        
    	ErrorMessage msg = ErrorMessage.parseFrom(rCmd);
        setExecutor("ErrorExecutor", msg);
    }

    public void addExecutor(IExecutor executor){
        executors.add(executor);
    }

    public List<IExecutor> getExecutors() {
        return executors;
    }

    public void setExecutors(List<IExecutor> executors) {
        this.executors = executors;
    }


    private class ExecutorRunnable implements Runnable {

        public void run() {
            while (true) {
                try {
                    Thread.sleep(10);
                    IExecutor executor;
                    synchronized (finishedExecutors) {
                    	executor = finishedExecutors.poll();
					}
                    if(executor != null)
                        executor.execute();
                } catch (InterruptedException ex) {
                    Logger.getLogger(Receiver.class.getName()).log(Level.SEVERE, null, ex);
                }
            }
        }
    }
}
