package vsu.alivechess.game;

import vsu.alivechess.R;
import vsu.alivechess.net.commands.AliveChessProtos.LandscapeTypes;
import vsu.alivechess.net.commands.AliveChessProtos.PointTypes;

public class CellType {
	
	public static int getLandscapeRes(int landscapeType){
		switch (landscapeType) {
		case LandscapeTypes.Grass_VALUE:
			return R.drawable.landscape_grass;
		default:
			return R.drawable.landscape_grass;
		}
	}
	
//	public static int getPointRes(int pointType){
//		switch (pointType) {
//		case PointTypes.pKing_VALUE:
//			return R.drawable.king;
//		default:
//			return R.drawable.landscape_grass;
//		}
//	}
	
	
}
