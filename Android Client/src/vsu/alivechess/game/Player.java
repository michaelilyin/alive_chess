package vsu.alivechess.game;

import java.util.ArrayList;
import java.util.List;

import vsu.alivechess.activities.BigMapAct;
import vsu.alivechess.net.commands.AliveChessProtos.Castle;
import vsu.alivechess.net.commands.AliveChessProtos.Mine;
import vsu.alivechess.net.commands.AliveChessProtos.Resource;
import vsu.alivechess.net.commands.AliveChessProtos.ResourceTypes;

public class Player {
	private BigMapAct activity;
	
	private GameKing king;
	private List<Castle> castles;
	private List<Mine> mines;
	private List<GameResource> resources;
	
	public Player(BigMapAct activity){
		this.activity = activity;
		
		castles = new ArrayList<Castle>();
		mines = new ArrayList<Mine>();
		
		resources = new ArrayList<GameResource>();
		resources.add(new GameResource(ResourceTypes.rWood_VALUE));
		resources.add(new GameResource(ResourceTypes.rStone_VALUE));
		resources.add(new GameResource(ResourceTypes.rGold_VALUE));
		resources.add(new GameResource(ResourceTypes.rIron_VALUE));
	}

	public void setKing(GameKing king) {
		this.king = king;
	}

	public GameKing getKing() {
		return king;
	}

	public void setCastles(List<Castle> castles) {
		this.castles = castles;
		setCountCastleToAct(castles.size());
	}

	public void addCastle(Castle castle){
		castles.add(castle);
		setCountCastleToAct(castles.size());
	}
	
	public boolean isCastle(int castleId){
		for(Castle castle : castles){
			if(castle.getCastleId() == castleId)
				return true;
		}
		return false;
	}
	
	public boolean looseCastle(int id){
		for (Castle castle : castles) {
			if(castle.getCastleId() == id){
				castles.remove(castle);
				setCountCastleToAct(castles.size());
				return true;
			}
		}
		return false;
	}
	
	public List<Castle> getCastles() {
		return castles;
	}

	public void setMines(List<Mine> mines) {
		this.mines = mines;
		setCountMineToAct(mines.size());
	}

	public void addMine(Mine mine){
		mines.add(mine);
		setCountMineToAct(mines.size());
	}
	
	public boolean looseMine(int id){
		for (Mine mine : mines) {
			if(mine.getMineId() == id){
				castles.remove(mine);
				setCountMineToAct(mines.size());
				return true;
			}
		}
		return false;		
	}
	
	public boolean isMine(int mineId){
		for (Mine mine : mines) {
			if(mine.getMineId() == mineId)
				return true;
		}
		return false;
	}
	
	public List<Mine> getMines() {
		return mines;
	}

	public void setResources(List<Resource> resources) {
		for (Resource resource : resources) {
			addResource(resource);
		}
	}
	
	public void addResource(Resource res){
		GameResource resource = new GameResource(res.getResourceType().ordinal());
		resource.setCount(res.getResourceCount());
		addResource(resource);
	}
	
	public void addResource(GameResource res){
		for (GameResource resource : resources) {
			if(resource.getResType() == res.getResType()){
				int sum = resource.getCount()+res.getCount();
				resource.setCount(sum);
				setResToActivity(resource);
				break;
			}
		}
	}

	private void setResToActivity(final GameResource res){
		activity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				switch (res.getResType()) {
				case ResourceTypes.rWood_VALUE:
					activity.setTxtWood(res.getCount());
					break;
				case ResourceTypes.rStone_VALUE:
					activity.setTxtStone(res.getCount());
					break;
				case ResourceTypes.rGold_VALUE:
					activity.setTxtGold(res.getCount());
					break;
				case ResourceTypes.rIron_VALUE:
					activity.setTxtIron(res.getCount());
					break;			
			}
			}
		});
	}
	
	private void setCountCastleToAct(final int count){
		activity.runOnUiThread(new Runnable() {
			
			@Override
			public void run() {
				activity.setTxtCastles(count);
			}
		});
	}
	
	private void setCountMineToAct(final int count){
		activity.runOnUiThread(new Runnable() {
			
			@Override
			public void run() {
				activity.setTxtMines(count);
			}
		});		
	}
	
	public List<GameResource> getResources() {
		return resources;
	}
}
