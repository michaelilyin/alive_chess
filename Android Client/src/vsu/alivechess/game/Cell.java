package vsu.alivechess.game;

import vsu.alivechess.net.commands.AliveChessProtos.LandscapeTypes;
import android.content.Context;
import android.graphics.Point;
import android.widget.AbsoluteLayout;
import android.widget.ImageView;

public class Cell{
	private Context cont;
	private ImageView landscape;
	private ImageView objectView;
	
	public static final int WIDTH = 32;
	public static final int HEIGHT = 32;
	
	private Point coordinate;
	private Point mapCoord;
	private int landscapeType;
	private int objectType = 0;
	private Object object;
	
	public Cell(Context cont){
		this.cont = cont;
		landscape = new ImageView(cont);
	}

	public void setCoordinate(Point coordinate) {
		this.coordinate = coordinate;
		landscape.setLayoutParams(new AbsoluteLayout.LayoutParams(WIDTH, HEIGHT, 
				coordinate.x, coordinate.y));
	}

	public Point getCoordinate() {
		return coordinate;
	}

	public void setLandscapeType(int landscapeType) {
		this.landscapeType = landscapeType;
		int res = CellType.getLandscapeRes(landscapeType);
		landscape.setBackgroundResource(res);
	}

	public int getLandscapeType() {
		return landscapeType;
	}

	public int getObjectType() {
		return objectType;
	}

	public void setMapCoord(Point mapCoord) {
		this.mapCoord = mapCoord;
	}

	public Point getMapCoord() {
		return mapCoord;
	}
	
	public ImageView getLandView(){
		return landscape;
	}

	public void setObject(int countCellX, int countCellY, int typeObj, Object object) {
		objectType = typeObj;
		this.object = object;
		objectView = new ImageView(cont);
		objectView.setLayoutParams(new AbsoluteLayout.LayoutParams(
				countCellX*WIDTH, countCellY*HEIGHT, coordinate.x, coordinate.y));
		objectView.setBackgroundResource(ObjectType.getResObject(typeObj));
	}

	public boolean isMine(){
		if(objectType == ObjectType.MINE_WOOD ||
			objectType == ObjectType.MINE_STONE ||
			objectType == ObjectType.MINE_GOLD ||
			objectType == ObjectType.MINE_IRON){
				return true;
		}
		return false;
	}
	
	public boolean isCastle(){
		if(objectType == ObjectType.CASTLE)
			return true;
		return false;
	}
	
	public ImageView getObjectView() {
		return objectView;
	}
}
