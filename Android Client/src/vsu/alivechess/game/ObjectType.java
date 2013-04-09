package vsu.alivechess.game;

import vsu.alivechess.R;

public class ObjectType {
	public static final int NONE = 0;
	public static final int MINE_WOOD = 1;
	public static final int MINE_STONE = 2;
	public static final int MINE_GOLD = 3;
	public static final int MINE_IRON = 4;
	public static final int TREE = 5;
	public static final int OBSTACLE = 6;
	public static final int RES_GOLD = 7;
	public static final int RES_TIMBER = 8;
	public static final int CASTLE = 9;
	
	
	public static int getResObject(int object){
		switch(object){
			case CASTLE:
				return R.drawable.castle;
			case MINE_WOOD:
				return R.drawable.mine_wood;
			case MINE_STONE:
				return R.drawable.mine_stones;
			case MINE_GOLD:
				return R.drawable.mine_gold;	
			case MINE_IRON:
				return R.drawable.mine_iron;
			case TREE:
				return R.drawable.tree_oak;
			case OBSTACLE:
				return R.drawable.stone;
			case RES_GOLD:
				return R.drawable.gold;
			case RES_TIMBER:
				return R.drawable.timber;	
		}
		return R.drawable.landscape_grass;
	}
}
