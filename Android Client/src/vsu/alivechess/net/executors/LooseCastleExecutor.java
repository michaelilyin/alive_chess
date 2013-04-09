package vsu.alivechess.net.executors;

import vsu.alivechess.game.GameMap;
import vsu.alivechess.net.commands.AliveChessProtos.LooseCastleMessage;
import vsu.alivechess.utils.AppHelper;

public class LooseCastleExecutor extends AbstractExecutor{
	GameMap map;
	
	public LooseCastleExecutor(GameMap map) {
		this.map = map;
	}
	
	@Override
	public void execute() {
		LooseCastleMessage resp = (LooseCastleMessage) getResponce();
		boolean result = map.getPlayer().looseCastle(resp.getCastleId());
		if(result)
			AppHelper.toast(map.getCont(), "Один из ваших замков захвачен");
	}
	
}
