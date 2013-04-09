package vsu.alivechess.net.executors;

import vsu.alivechess.game.GameMap;
import vsu.alivechess.net.commands.AliveChessProtos.GetResourceMessage;
import vsu.alivechess.net.commands.AliveChessProtos.Resource;

public class GetResourceExecutor extends AbstractExecutor{
	GameMap map;
	
	public GetResourceExecutor(GameMap map){
		this.map = map;
	}
	
	@Override
	public void execute() {
		GetResourceMessage msg = (GetResourceMessage) getResponce();
		if(!msg.getFromMine()){
			map.removeResource(msg.getResource().getResourceId());
		} 
		Resource res;
		if(msg.getResource().getResourceCount() == 0){
			res = Resource.newBuilder()
				.setResourceType(msg.getResource().getResourceType())
				.setResourceCount(100).build();
		}else{
			res = msg.getResource();
		}
		map.getPlayer().addResource(res);
	}
	
}
