package vsu.alivechess.game;

import java.util.ArrayList;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;

import android.app.Activity;
import android.content.Context;
import android.graphics.Point;
import android.widget.ImageView;
import vsu.alivechess.activities.BigMapAct;
import vsu.alivechess.net.commands.AliveChessProtos.Castle;
import vsu.alivechess.net.commands.AliveChessProtos.GetMapResponse;
import vsu.alivechess.net.commands.AliveChessProtos.King;
import vsu.alivechess.net.commands.AliveChessProtos.LandscapeTypes;
import vsu.alivechess.net.commands.AliveChessProtos.Mine;
import vsu.alivechess.net.commands.AliveChessProtos.Position;
import vsu.alivechess.net.commands.AliveChessProtos.Resource;
import vsu.alivechess.net.commands.AliveChessProtos.ResourceTypes;
import vsu.alivechess.net.commands.AliveChessProtos.SingleObject;
import vsu.alivechess.net.commands.AliveChessProtos.SingleObjectType;
import vsu.alivechess.utils.AppHelper;

public class GameMap {
	private BigMapAct cont;
	
	public final static int VISIBLE_MAP_X = 11;
	public final static int VISIBLE_MAP_Y = 10;
	
	private GetMapResponse resp;
	private Cell[][] cells;
	private Selector selector;
	private int mapX = 49;
	private int mapY = 49;
	private GameKing king;
	private Player player;
	private List<GameKing> enemyKings;
	private List<Resource> resources;
	
	private Point firstCell = new Point(0, 0);
	
	public GameMap(BigMapAct cont){
		this.cont = cont;
		king = new GameKing(cont, this, true);
		player = new Player(cont);
		enemyKings = new ArrayList<GameKing>();
		setSelector(new Selector(cont, this));
	}
	
	public void createMap(){
		setCells(new Cell[mapX][mapY]);

		int startX = 0;
		int startY = 0;
		for (int i=0; i<mapX; i++) {
			for(int j=0; j<mapY; j++) {
				Cell cell = new Cell(cont);
				cell.setMapCoord(new Point(j, i));
				cell.setCoordinate(new Point(startX, startY));
				startX += Cell.WIDTH;
				cell.setLandscapeType(LandscapeTypes.Grass_VALUE);
				cells[j][i] = cell;
			}
			startY += Cell.HEIGHT;
			startX = 0;
		}
	}
	
	public void setMapResp(GetMapResponse mapResp){
		this.resp = mapResp;
		mapX = mapResp.getSizeMapX()-1;
		mapY = mapResp.getSizeMapY()-1;
		
		setCastles();
		setMines();
		setSingleObjects();		
	}

	private void setCastles() {
		List<Castle> castles = resp.getCastlesList();
		for (Castle castle : castles) {
			Cell cell = getCell(castle.getLeftX(), castle.getTopY());
			cell.setObject(castle.getWidth(), castle.getHeight(), ObjectType.CASTLE, castle);
		}
	}

	private void setSingleObjects() {
		List<SingleObject> objects = resp.getSingleObjectsList();
		for (SingleObject singleObject : objects) {
			Cell cell = getCell(singleObject.getX(), singleObject.getY());
			if(singleObject.getSingleObjectType() == SingleObjectType.Tree){
				cell.setObject(1, 1, ObjectType.TREE, singleObject);
			} else if(singleObject.getSingleObjectType() == SingleObjectType.Obstacle){
				cell.setObject(1, 1, ObjectType.OBSTACLE, singleObject);
			}
		}
	}

	private void setMines() {
		List<Mine> mines = resp.getMinesList();
		for (Mine mine : mines) {
			Cell cell = getCell(mine.getLeftX(), mine.getTopY());
			Resource resource = mine.getGainingResource();
			if (resource.getResourceType() == ResourceTypes.rWood) {
				cell.setObject(mine.getWidth(), mine.getHeight(), ObjectType.MINE_WOOD, mine);				
			} else if (resource.getResourceType() == ResourceTypes.rStone) {
				cell.setObject(mine.getWidth(), mine.getHeight(), ObjectType.MINE_STONE, mine);				
			} else if (resource.getResourceType() == ResourceTypes.rIron) {
				cell.setObject(mine.getWidth(), mine.getHeight(), ObjectType.MINE_IRON, mine);				
			} else if (resource.getResourceType() == ResourceTypes.rGold) {
				cell.setObject(mine.getWidth(), mine.getHeight(), ObjectType.MINE_GOLD, mine);				
			}
		}
	}
	
	public List<Cell> setResources(List<Resource> resources){
		this.resources = resources;
		List<Cell> result = new ArrayList<Cell>();
		for (Resource resource : resources) {
			Cell cell = getCell(resource.getX(), resource.getY());
			if(cell.getObjectType()!=ObjectType.NONE)
				continue;
			
			if(resource.getResourceType() == ResourceTypes.rGold){
				cell.setObject(1, 1, ObjectType.RES_GOLD, resource);
			} else if(resource.getResourceType() == ResourceTypes.rWood){
				cell.setObject(1, 1, ObjectType.RES_TIMBER, resource);
			}
			result.add(cell);
		}
		return result;
	}

	public void setKingPosition(Point pos){
		king.setPosition(pos);
		setFirstCell(new Point(
				king.getPosition().x - VISIBLE_MAP_X/2, 
				king.getPosition().y - VISIBLE_MAP_Y/2));
		cont.setMapLocation();
	}
	
	public Point getMapCoordinate(int x, int y){
		int shiftX = x/Cell.WIDTH;
		int shiftY = y/Cell.HEIGHT;
		if(shiftX > VISIBLE_MAP_X-1 || shiftY > VISIBLE_MAP_Y-1)
			return null;
		return new Point(firstCell.x+shiftX, firstCell.y+shiftY);
	}
	
	public Mine getMine(Point pos){
		List<Mine> mines = resp.getMinesList();
		for (Mine mine : mines) {
			if(pos.x >= mine.getLeftX() && pos.x < mine.getLeftX()+mine.getWidth() &&
					pos.y >= mine.getTopY() && pos.y < mine.getTopY()+mine.getHeight()){
				return mine;
			}
		}
		return null;
	}
	
	public Castle getCastle(Point pos){
		List<Castle> castles = resp.getCastlesList();
		for (Castle castle : castles) {
			if(pos.x >= castle.getLeftX() && pos.x < castle.getLeftX()+castle.getWidth() &&
					pos.y >= castle.getTopY() && pos.y < castle.getTopY()+castle.getHeight()){
				return castle;
			}
		}
		return null;
	}
	
	public boolean removeResource(int id){
		for (Resource res : resources) {
			if(res.getResourceId() == id){
				//resources.remove(res);
				Cell cell = getCell(res.getX(), res.getY());
				cont.removeResource(cell);
				return true;
			}
		}
		return false;
	}
	
	public GameKing getKing(){
		return king;
	}
	
	public Cell getCell(int x, int y){
		return cells[x][y];
	}

	public Cell getCell(Point pos){
		return cells[pos.x][pos.y];
	}	
	
	public void setCells(Cell[][] cells) {
		this.cells = cells;
	}

	public Cell[][] getCells() {
		return cells;
	}
	
	public int getSizeXPix(){
		return mapX*Cell.WIDTH;
	}
	
	public int getSizeYPix(){
		return mapY*Cell.HEIGHT;
	}
	
	public int getMapX(){
		return mapX;
	}
	
	public int getMapY(){
		return mapY;
	}

	public void setFirstCell(Point firstCell) {
		int x = Math.min(Math.max(0, firstCell.x)+VISIBLE_MAP_X, mapX)-VISIBLE_MAP_X;
		int y = Math.min(Math.max(0, firstCell.y)+VISIBLE_MAP_Y, mapY)-VISIBLE_MAP_Y;
		this.firstCell = new Point(x, y);
	}

	public Point getFirstCell() {
		return firstCell;
	}

	public void setPlayer(Player player) {
		this.player = player;
	}

	public Player getPlayer() {
		return player;
	}
	
	public Activity getCont(){
		return cont;
	}

	public void addEnemyKing(int id, Point pos){
		GameKing king = findEnemyKing(id);
		if(king != null){
			king.setPosition(pos);
			return;					
		}
		
		king = new GameKing(cont, this, false);
		king.setId(id);
		king.setPosition(pos);
		enemyKings.add(king);
		cont.getLayout().addView(king.getKingView());
	}
	
	public GameKing findEnemyKing(int id){
		for(GameKing king : enemyKings){
			if(king.getId() == id)
				return king;
		}
		return null;
	}
	
	public void moveEnemyKing(int id, Point pos){
		GameKing king = findEnemyKing(id);
		if(king == null)
			return;
		king.addStep(pos);
	}
	
	public void removeEnemyKing(int id){
		GameKing king = findEnemyKing(id);
		if(king == null)
			return;
		enemyKings.remove(king);
		cont.getLayout().removeView(king.getKingView());
	}
	
	public void setEnemyKings(List<GameKing> enemyKings) {
		this.enemyKings = enemyKings;
	}

	public List<GameKing> getEnemyKings() {
		return enemyKings;
	}

	public void setSelector(Selector selector) {
		this.selector = selector;
	}

	public Selector getSelector() {
		return selector;
	}
	
}
