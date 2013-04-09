package vsu.alivechess.net.executors;

import vsu.alivechess.activities.CastleAct;
import vsu.alivechess.game.GameMap;
import android.app.Activity;
import android.content.Intent;

public class ComeInCastleExecutor extends AbstractExecutor{
	Activity cont;
	GameMap map;
	int castleId;
	
	public ComeInCastleExecutor(Activity cont, GameMap map, int castleId) {
		this.cont = cont;
		this.map = map;
		this.castleId = castleId;
	}

	@Override
	public void execute() {
		cont.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				Intent intent = new Intent(cont, CastleAct.class);
				intent.putExtra("castleId", castleId);
				cont.startActivity(intent);
			}
		});
	}
}
