package vsu.alivechess.net.executors;

import vsu.alivechess.game.GameMap;
import vsu.alivechess.net.commands.AliveChessProtos.LooseMineMessage;
import vsu.alivechess.utils.AppHelper;

public class LooseMineExecutor extends AbstractExecutor{
	GameMap map;
	
	public LooseMineExecutor(GameMap map) {
		this.map = map;
	}
	
	@Override
	public void execute() {
		LooseMineMessage resp = (LooseMineMessage) getResponce();
		boolean result = map.getPlayer().looseMine(resp.getMineId());
		if(result)
			AppHelper.toast(map.getCont(), "Одна из ваших шахт захвачена");
	}
}
