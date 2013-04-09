package vsu.alivechess.game;

import vsu.alivechess.net.commands.AliveChessProtos.ResourceTypes;

public class GameResource {
	private int resType;
	private int count;
	
	public GameResource(int type){
		setResType(type);
		setCount(0);
	}

	public void setResType(int resType) {
		this.resType = resType;
	}

	public int getResType() {
		return resType;
	}

	public void setCount(int count) {
		this.count = count;
	}

	public int getCount() {
		return count;
	}
	
	
}
