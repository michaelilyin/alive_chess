package vsu.alivechess.game;

import java.util.LinkedList;
import java.util.List;
import java.util.Queue;
import java.util.Timer;
import java.util.TimerTask;

import vsu.alivechess.R;
import vsu.alivechess.net.commands.AliveChessProtos.Position;
import android.app.Activity;
import android.graphics.Point;
import android.widget.AbsoluteLayout;
import android.widget.ImageView;

public class GameKing {
	public static abstract class OnKingMoved{
		public abstract void kingMoved(Point pos);
	}
	
	public final static int TIME_MOVE = 100;
	
	protected Activity cont;
	
	private OnKingMoved onKingMoved;
	
	private ImageView kingView;
	private GameMap map;
	private Point position;
	private boolean isMoving;
	private int id;
	private boolean playerKing;
	
	private Queue<Point> steps = new LinkedList<Point>();
	
	public GameKing(Activity cont, GameMap map, boolean playerKing){
		this.cont = cont;
		this.map = map;
		this.playerKing = playerKing;
		kingView = new ImageView(cont);
		kingView.setBackgroundResource(R.drawable.king);
		isMoving = false;
	}

	public void setPosition(Point position) {
		this.position = position;
		Cell cell = map.getCell(position.x, position.y);
		kingView.setLayoutParams(new AbsoluteLayout.LayoutParams(
				Cell.WIDTH, Cell.HEIGHT, cell.getCoordinate().x, cell.getCoordinate().y));
	}

	public void addStep(Point pos){
		synchronized (steps) {
			steps.add(pos);			
		}
		if(!isMoving())
			moveKing();
	}
	
	public Point getStep(){
		Point p;
		synchronized (steps) {
			p = steps.poll();
		}
		return p;
	}
	
	public int getStepCount(){
		int count = 0;
		synchronized (steps) {
			count = steps.size();
		}
		return count;
	}
	
	public void addSteps(List<Position> steps){
		for (Position pos : steps) {
			Point point = new Point(pos.getX(), pos.getY());
			addStep(point);
		}
	}
	
	public void moveKing(){
		isMoving = true;
		final Point pos = getStep();
		if(pos == null){
			isMoving = false;	
			return;
		}
		
		Timer timer = new Timer();
		timer.schedule(new TimerTask() {
			@Override
			public void run() {
				cont.runOnUiThread(new Runnable() {
					@Override
					public void run() {
						if(playerKing){
							map.setKingPosition(pos);
						} else {
							setPosition(pos);
						}
						if(getStepCount() == 0 && onKingMoved != null)
							onKingMoved.kingMoved(pos);
						moveKing();
					}
				});
			}
		}, TIME_MOVE);
	}	
	
	public Point getPosition() {
		return position;
	}
	
	public ImageView getKingView(){
		return kingView;
	}

	public void setMoving(boolean moving) {
		this.isMoving = moving;
	}

	public boolean isMoving() {
		return isMoving;
	}

	public void setId(int id) {
		this.id = id;
	}

	public int getId() {
		return id;
	}

	public void setPlayerKing(boolean playerKing) {
		this.playerKing = playerKing;
	}

	public boolean isPlayerKing() {
		return playerKing;
	}

	public void setOnKingMoved(OnKingMoved onKingMoved) {
		this.onKingMoved = onKingMoved;
	}

	public OnKingMoved getOnKingMoved() {
		return onKingMoved;
	}

}
