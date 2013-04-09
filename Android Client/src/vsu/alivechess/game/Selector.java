package vsu.alivechess.game;

import vsu.alivechess.R;
import android.content.Context;
import android.graphics.Point;
import android.widget.AbsoluteLayout;
import android.widget.ImageView;

public class Selector extends ImageView{
	
	public static final int WIDTH = Cell.WIDTH*2;
	public static final int HEIGHT = Cell.HEIGHT*2;
	
	private GameMap map;
	public Point curPos = new Point(-1,-1);
	
	public Selector(Context cont, GameMap map){
		super(cont);
		this.map = map;
		setLayoutParams(new AbsoluteLayout.LayoutParams(WIDTH, HEIGHT, 0, 0));
		setBackgroundResource(R.drawable.selector);
		setVisibility(INVISIBLE);
	}
	
	public void drawSelector(Point position){
		Point pos = map.getMapCoordinate(position.x, position.y);
		if(pos==null || pos.equals(curPos.x, curPos.y))
			return;
		curPos.set(pos.x, pos.y);		
		
		Cell cell = map.getCell(pos);
		Point posCell = new Point(cell.getCoordinate().x, cell.getCoordinate().y);
		posCell.offset(-WIDTH/4, -HEIGHT/4);
		setVisibility(VISIBLE);
		setLayoutParams(new AbsoluteLayout.LayoutParams(WIDTH, HEIGHT, posCell.x, posCell.y));
		bringToFront();
	}
	
	public void removeSelector(){
		curPos.set(-1, -1);
		setVisibility(INVISIBLE);
	}
}
