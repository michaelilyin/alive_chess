package vsu.alivechess.activities;

import java.io.IOException;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;

import vsu.alivechess.R;
import vsu.alivechess.game.Cell;
import vsu.alivechess.game.GameKing;
import vsu.alivechess.game.GameMap;
import vsu.alivechess.game.Player;
import vsu.alivechess.net.ClientApp;
import vsu.alivechess.net.Receiver;
import vsu.alivechess.net.Sender;
import vsu.alivechess.net.commands.AliveChessProtos.CaptureCastleResponse;
import vsu.alivechess.net.commands.AliveChessProtos.CaptureMineResponse;
import vsu.alivechess.net.commands.AliveChessProtos.Castle;
import vsu.alivechess.net.commands.AliveChessProtos.GetGameStateResponse;
import vsu.alivechess.net.commands.AliveChessProtos.GetMapResponse;
import vsu.alivechess.net.commands.AliveChessProtos.GetObjectsResponse;
import vsu.alivechess.net.commands.AliveChessProtos.King;
import vsu.alivechess.net.commands.AliveChessProtos.Mine;
import vsu.alivechess.net.commands.AliveChessProtos.MoveKingResponse;
import vsu.alivechess.net.commands.AliveChessProtos.PointTypes;
import vsu.alivechess.net.commands.AliveChessProtos.Position;
import vsu.alivechess.net.executors.CaptureCastleExecutor;
import vsu.alivechess.net.executors.CaptureMineExecutor;
import vsu.alivechess.net.executors.ComeInCastleExecutor;
import vsu.alivechess.net.executors.Executors;
import vsu.alivechess.net.executors.ExitFromGameExecutor;
import vsu.alivechess.net.executors.GetGameStateExecutor;
import vsu.alivechess.net.executors.GetMapExecutor;
import vsu.alivechess.net.executors.GetObjectsExecutor;
import vsu.alivechess.net.executors.MoveKingExecutor;
import vsu.alivechess.utils.AppHelper;
import android.app.Activity;
import android.content.Context;
import android.graphics.Point;
import android.os.Bundle;
import android.view.MotionEvent;
import android.view.View;
import android.widget.AbsoluteLayout;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

public class BigMapAct extends Activity {
	BigMapAct cont;
	GameMap map;
	
	AbsoluteLayout mapLayout;
	
	TextView txtWood;
	TextView txtStone;
	TextView txtGold;
	TextView txtIron;
	
	TextView txtCastles;
	TextView txtMines;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        cont = this;
        setContentView(R.layout.bigmap);
        
        mapLayout = (AbsoluteLayout) findViewById(R.id.map_layout);
        
    	txtWood  = (TextView) findViewById(R.id.game_txt_wood);
    	txtStone = (TextView) findViewById(R.id.game_txt_stone);
    	txtGold  = (TextView) findViewById(R.id.game_txt_gold);
    	txtIron  = (TextView) findViewById(R.id.game_txt_iron);
    	
    	txtCastles = (TextView) findViewById(R.id.game_txt_castles);
    	txtMines   = (TextView) findViewById(R.id.game_txt_mines);        

		map = new GameMap(this);  
		map.createMap();
		mapLayout.addView(map.getSelector());
		
		Executors.createExecutors(map);
    	
        Sender.getInstance().sendGetMap(new GetMapExecutor() {
			@Override
			public void execute() {
				final GetMapResponse resp = (GetMapResponse) this.getResponce();
				runOnUiThread(new Runnable() {		
					@Override
					public void run() {
						map.setMapResp(resp);
						setMapLayout();
						if(map.getKing() != null)
							map.getKing().getKingView().bringToFront();
					}
				});
			}
		});

        Sender.getInstance().sendGetGameState(new GetGameStateExecutor() {
			@Override
			public void execute() {
				final GetGameStateResponse gsResp = (GetGameStateResponse) getResponce();
				runOnUiThread(new Runnable() {
					@Override
					public void run() {
						if(map == null)
							return;
						
						King k = gsResp.getKing();
						map.setKingPosition(new Point(k.getX(), k.getY()));
						map.getPlayer().setResources(gsResp.getStartResourcesList());
						map.getPlayer().addCastle(gsResp.getCastle());
						
						ImageView kingView = map.getKing().getKingView();
						kingView.bringToFront();
						mapLayout.addView(kingView);
				    	setMapLocation();
						
					}
				});
			}
		});
        
        Receiver.getInstance().addExecutor(new GetObjectsExecutor() {
			@Override
			public void execute() {
				final GetObjectsResponse resp = (GetObjectsResponse) getResponce();
				runOnUiThread(new Runnable() {
					@Override
					public void run() {
						if(map == null)		
							return;
					
						List<Cell> cells = map.setResources(resp.getResourcesList());
						for (Cell cell : cells) {
							mapLayout.addView(cell.getObjectView());
						}
						
						List<King> kings = resp.getKingsList();
						for (King king : kings) {
							Point pos = new Point(king.getX(), king.getY());
							map.addEnemyKing(king.getKingId(), pos);
						}
					}
				});
			}
		});

        
        Sender.getInstance().sendGetObjects(false, 0, PointTypes.pKing);
    }
    
    private void setMapLayout(){    
    	setMapLocation();
    	
    	Cell[][] cells = map.getCells();
    	for(int i=0; i<map.getMapX(); i++)
    		for(int j=0; j<map.getMapY(); j++)
    			mapLayout.addView(cells[i][j].getLandView());

    	for(int i=0; i<map.getMapX(); i++){
    		for(int j=0; j<map.getMapY(); j++){
    			Cell cell = cells[i][j];
    			ImageView obj = cell.getObjectView();
    			if(obj != null)
    				mapLayout.addView(obj);
    		}
    	}
    }
    
    public void setMapLocation(){
    	mapLayout.setLayoutParams(new AbsoluteLayout.LayoutParams(
    			map.getSizeXPix(), map.getSizeYPix(), 
    			-map.getFirstCell().x*Cell.WIDTH, 
    			-map.getFirstCell().y*Cell.HEIGHT));    	
    }
    
	public void moveKing(int x, int y){
		if(!map.getKing().isMoving()){
    		Point point  = map.getMapCoordinate(x, y);
    		if(point == null)
    			return;
    		Sender.getInstance().sendMoveKing(point.x, point.y, new MoveKingExecutor() {
				
				@Override
				public void execute() {
					MoveKingResponse resp = (MoveKingResponse) getResponce();
					map.getKing().addSteps(resp.getStepsList());
					map.getKing().setOnKingMoved(onKingMoved);
//					map.getKing().setAction(true);
//					mk(0, resp.getStepsList());
				}
			});
		}
	}
	
//	private void mk(final int step, final List<Position> steps){
//		if(step == steps.size()){
//			map.getKing().setAction(false);
//			checkWhereKing(steps.get(steps.size()-1));
//			return;
//		}
//		
//		Timer timer = new Timer();
//		timer.schedule(new TimerTask() {
//			@Override
//			public void run() {				
//				runOnUiThread(new Runnable() {					
//					@Override
//					public void run() {
//						Position pos = steps.get(step);
//			    		map.setKingPosition(new Point(pos.getX(), pos.getY()));
//			    		int nextStep = step+1;
//			    		mk(nextStep, steps);
//					}
//				});
//			}
//		}, map.getKing().TIME_MOVE);		
//	}
    
	GameKing.OnKingMoved onKingMoved = new GameKing.OnKingMoved() {
		@Override
		public void kingMoved(Point pos) {
			checkWhereKing(pos);
		}
	};
	
	public void checkWhereKing(Point pos){
		final Mine mine = map.getMine(pos);
		if(mine != null && !map.getPlayer().isMine(mine.getMineId())){
			Sender.getInstance().sendCaptureMine(mine.getMineId(), 
					new CaptureMineExecutor() {
				
				@Override
				public void execute() {
					CaptureMineResponse resp = (CaptureMineResponse) getResponce();
					Mine respMine = resp.getMine();
					if(respMine == null)
						return;
					if(mine.getMineId() == resp.getMine().getMineId()){
						map.getPlayer().addMine(respMine);
						AppHelper.toast(cont, "Шахта захвачена");
					}
				}
			});
		}
		
		final Castle castle = map.getCastle(pos);
		if(castle != null && !map.getPlayer().isCastle(castle.getCastleId())){
			Sender.getInstance().sendCaptureCastle(castle.getCastleId(), 
					new CaptureCastleExecutor() {
				
				@Override
				public void execute() {
					CaptureCastleResponse resp = (CaptureCastleResponse) getResponce();
					Castle respCastle = resp.getCastle();
					if(respCastle == null)
						return;
					if(castle.getCastleId() == resp.getCastle().getCastleId()){
						map.getPlayer().addCastle(castle);
						AppHelper.toast(cont, "Замок захвачен");
					}
				}
			});
		}
		
		if(castle != null && map.getPlayer().isCastle(castle.getCastleId())){
			Sender.getInstance().sendComeInCastle(castle.getCastleId(), 
					new ComeInCastleExecutor(this, map, castle.getCastleId()));
		}
	}
	
	public void removeResource(final Cell resCell){
		runOnUiThread(new Runnable() {			
			@Override
			public void run() {
				mapLayout.removeView(resCell.getObjectView());
			}
		});
	}
	
	public AbsoluteLayout getLayout(){
		return mapLayout;
	}
	
    @Override
    public boolean onTouchEvent(MotionEvent event) {
    	int action = event.getAction();
    	int x = (int)event.getX();
    	int y = (int)event.getY();
    	Point pos = new Point(x,y);
    	
    	if(action == MotionEvent.ACTION_UP){
    		map.getSelector().removeSelector();
    		moveKing(x, y);
		} else if (action == MotionEvent.ACTION_DOWN) {
			map.getSelector().drawSelector(pos);
		} else if (action == MotionEvent.ACTION_MOVE) {
			map.getSelector().drawSelector(pos);
		}
    	return super.onTouchEvent(event);
    }
    
    public void click_btn_exit(View view){
    	finish();
    }
    
    
    public void setTxtWood(int count){
    	txtWood.setText(String.valueOf(count));
    }
    
    public void setTxtStone(int count){
    	txtStone.setText(String.valueOf(count));
    }    
    
    public void setTxtGold(int count){
    	txtGold.setText(String.valueOf(count));
    }
    
    public void setTxtIron(int count){
    	txtIron.setText(String.valueOf(count));
    }
    
    
    public void setTxtCastles(int count){
    	txtCastles.setText(String.valueOf(count));
    }
    
    public void setTxtMines(int count){
    	txtMines.setText(String.valueOf(count));
    }
    
    @Override
    public void finish() {
    	Sender.getInstance().sendExitFromGame(new ExitFromGameExecutor() {			
			@Override
			public void execute() {
				try {
					ClientApp.getInstance().close();
					closeAct();
				} catch (IOException e) {}
			}
		});
    }
    
    private void closeAct(){
    	super.finish();    	
    }
}
