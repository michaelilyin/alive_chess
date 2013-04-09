package vsu.alivechess.net.executors;

import vsu.alivechess.game.GameMap;
import vsu.alivechess.net.Receiver;

public class Executors {
	public static void createExecutors(GameMap map){
		Receiver.getInstance().addExecutor(new LooseCastleExecutor(map));
		Receiver.getInstance().addExecutor(new LooseMineExecutor(map));
		Receiver.getInstance().addExecutor(new GetResourceExecutor(map));
		Receiver.getInstance().addExecutor(new UpdateWorldExecutor(map));
	}
}
