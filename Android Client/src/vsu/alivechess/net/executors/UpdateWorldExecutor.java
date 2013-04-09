package vsu.alivechess.net.executors;

import android.graphics.Point;
import vsu.alivechess.game.GameMap;
import vsu.alivechess.net.commands.AliveChessProtos.UpdateType;
import vsu.alivechess.net.commands.AliveChessProtos.UpdateWorldMessage;

public class UpdateWorldExecutor extends AbstractExecutor{
	GameMap map;
	
	public UpdateWorldExecutor(GameMap map) {
		this.map = map;
	}
	
	@Override
	public void execute() {
		final UpdateWorldMessage msg = (UpdateWorldMessage) getResponce();
		
		map.getCont().runOnUiThread(new Runnable() {
			
			@Override
			public void run() {
				Point pos = new Point(Math.round(msg.getLocation().getX()), 
						Math.round(msg.getLocation().getY()));
				UpdateType type = msg.getUpdateType();
				if (type == null)
					type = UpdateType.KingMove;
				
				if(type == UpdateType.KingAppear){
					map.addEnemyKing(msg.getObjectId(), pos);
				} else if(type == UpdateType.KingDisappear){
					map.removeEnemyKing(msg.getObjectId());
				} else if(type == UpdateType.KingMove){
					map.moveEnemyKing(msg.getObjectId(), pos);
				} else if(type == UpdateType.ResourceDisappear){
					map.removeResource(msg.getObjectId());
				}
			}
		});
	}
}
