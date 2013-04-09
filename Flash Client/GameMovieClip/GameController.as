package GameMovieClip
{
	import ACEvents.BigMapEvents.BigMapEvent;
	
	import AliveChessLibrary.Commands.BigMapCommand.BigMapRequest;
	import AliveChessLibrary.Commands.BigMapCommand.BigMapResponse;
	import AliveChessLibrary.Commands.BigMapCommand.Castle;
	import AliveChessLibrary.Commands.BigMapCommand.ComeInCastleRequest;
	import AliveChessLibrary.Commands.BigMapCommand.ComeInCastleResponse;
	import AliveChessLibrary.Commands.BigMapCommand.ContactCastleRequest;
	import AliveChessLibrary.Commands.BigMapCommand.ContactCastleResponse;
	import AliveChessLibrary.Commands.BigMapCommand.Dialog;
	import AliveChessLibrary.Commands.BigMapCommand.GetGameStateRequest;
	import AliveChessLibrary.Commands.BigMapCommand.GetGameStateResponse;
	import AliveChessLibrary.Commands.BigMapCommand.King;
	import AliveChessLibrary.GameObjects.Buildings.Castle;
	import AliveChessLibrary.GameObjects.Resources.Resource;
	
	import Net.Transport;
	
	import flash.display.MovieClip;
	import flash.display.Sprite;
	import flash.events.*;
	import flash.text.engine.EastAsianJustifier;
	
	import main.Main;
		
	public class GameController
	{
		private var bm:BigMapResponse = null;
		private var stateGame:GetGameStateResponse = null;
		
		private var transport:Transport = null;
		private var m:Main = null;
		
		private var game:Game = null;
		
		private var bigMap:GameMovieClip.BigMap = null;
		private var bigMapController:BigMapController = null;
		
		private var king:GameMovieClip.King = null;
		private var kingController:KingController = null;
		
		private var castleController:CastleController = null;
		
		//private var close:Close = null;
		private var menu:CloseAndBattle = null;
		public function GameController(m:Main, game:Game, bigMap:BigMap, k:GameMovieClip.King)
		{
			this.m = m;
			this.game = game;
			this.bigMap = bigMap;
			this.king = k;
			transport = Transport.getInstance();	
			//close = new Close(m);
			GetMap();
			castleController = new CastleController(this);
			game.addEventListener(MouseEvent.CLICK, clickHandler);
			transport.addEventListener(BigMapEvent.COME_IN_CASTLE,comeInCastle);
		}
		private function comeInCastle(e:Event):void{
			//transport.removeEventListener(BigMapEvent.COME_IN_CASTLE,comeInCastle);
			var msgComeInCastle:ComeInCastleResponse = ComeInCastleResponse(transport.msgResponse);
			var castleId:int = msgComeInCastle.castleId;
			castleController.CastleId = castleId;
			game.castleInfo = new CastleInfo(castleId,castleController);
			//bigMapController.clearMap();
			game.removeEventListener(MouseEvent.CLICK, clickHandler);
			game.addChild(game.castleInfo);
			//var contactCastleRequest:ContactCastleRequest = new ContactCastleRequest();
			//contactCastleRequest.castleId = castleId;
			//transport.addEventListener(BigMapEvent.CONTACT_CASTLE,contactCastleHandler);
			//transport.SendCommand(contactCastleRequest);
		}
		public function delCastleInfo():void{
			game.removeChild(game.castleInfo);
			game.addEventListener(MouseEvent.CLICK, clickHandler);
		}
		private function contactCastleHandler(e:Event):void{
			transport.removeEventListener(BigMapEvent.CONTACT_CASTLE,contactCastleHandler);
			var msgContactCastle:ContactCastleResponse = ContactCastleResponse(transport.msgResponse);
			var contCastle:AliveChessLibrary.GameObjects.Buildings.Castle = msgContactCastle.Castle;
			var disput:Dialog = msgContactCastle.Dispute;
			
		}
		function clickHandler(e:MouseEvent):void{				
			kingController.moveKing(Math.floor(e.target.x / 32),Math.floor(e.target.y / 32));
		}
		private function GetGameState():void{
			transport.addEventListener(BigMapEvent.GET_GAME_STATE,getGameStateHandler);
			transport.SendCommand(new GetGameStateRequest());
		}
		private function getGameStateHandler(e:Event):void{
			try{
				stateGame = GetGameStateResponse(transport.msgResponse);
				king.KingId = stateGame.king.KingId;
				king.KingMilitaryRank = stateGame.king.KingMilitaryRank;
				king.KingExperience = stateGame.king.KingExperience;
				king.KingName = stateGame.king.KingName;
				king.Castle = stateGame.castle.CastleId;
				bigMapController.getCastleofId(stateGame.castle.CastleId).setMyCastle();
				game.kingInfo = new KingInfo(king,bigMapController.getCastleofId(king.Castle));
				game.kingInfo.x = 0;
				game.kingInfo.y = 2;
				game.addChild(game.kingInfo);
				trace(stateGame.king.X*32,stateGame.king.Y*32);
				king.setPosition(stateGame.king.X*32,stateGame.king.Y*32);
				bigMapController.setCenter();
				trace("GetGameStateResponse","CastleId",stateGame.castle.CastleId);
			//	game.setCastleOfKing(stateGame.castle.CastleId);
			//	game.setResourcesOfKing(stateGame.startResources);
				trace("startResources");				
				bigMapController.setResources(stateGame.startResources);
				setStartRes(stateGame.startResources);
				bigMap.area.x = king.x+king.width - bigMap.area.width/2;
				bigMap.area.y = king.y+king.height - bigMap.area.height/2;	
				//bigMap.addChild(bigMap.area);				
			}
			catch(er:Error){/*trace(er.toString());*/}
		}
		private function setStartRes(res:Array):void{
			for each(var r:Object in res){
				king.AddResource(r as Resource);
			}
		}
		private function GetMap():void{			
			transport.addEventListener(BigMapEvent.BIG_MAP,getBigMap)
			transport.SendCommand(new BigMapRequest());		
		}
		public function getBigMap(e:Event):void{
			try{		
				trace("BigMapResponse");
				bm = BigMapResponse (transport.msgResponse);
				if(bm.isAllowed){
					game.x = 0; game.y = 0;
					m.addChild(game);
					StartBigMap();		
					game.ClearStage();
				/*	close.x = 515;
					close.y = 5;
					game.addChild(close);*/
					menu = new CloseAndBattle(m,this.game);
					menu.x=490;
					menu.y=2;
					game.addChild(menu);
				}
				//transport.removeEventListener(BigMapEvent.BIG_MAP,getBigMap);
			}
			catch(er:Error){/*trace(er.toString());*/}
		}
		private function StartBigMap():void{
			game.goNewFrame();
			while(game.numChildren>0){
				game.removeChildAt(0);
			}
			bigMap = new GameMovieClip.BigMap(m);
			bigMapController = new BigMapController(bigMap,CreateKing(),this.game, this.m);
			kingController.setBigMapController(bigMapController);
			game.addChild(bigMap);				
			GetGameState();
			game.addHungler();
			menu = new CloseAndBattle(m,this.game);
			menu.x=490;
			menu.y=2;
			game.addChild(menu);
		}
		private function CreateKing():King{
			king = new King(m, bigMap);
			kingController = new KingController(king,bigMapController);
			var rect:Sprite = new Sprite();
			rect.graphics.lineStyle(1);
		//	king.addHandler();
			return king;
		}
		/*public function createOtherKing(k:AliveChessLibrary.Commands.BigMapCommand.King):void{
			var otherKing:GameMovieClip.King = new GameMovieClip.King(this.m,this.bigMap);
			otherKing.x = k.X*32;
			otherKing.y = k.Y*32;
			otherKing.KingId = k.KingId;
			otherKing.KingName = k.KingName;
			//...
			bigMap.addChild(otherKing);
		}*/
		public function createBattleMap(/*c: AliveChessLibrary.GameObjects.Buildings.Castle*/ id:int):void{
			game.battleMap = new BattleMap(id,this.game);
			game.addChild(game.battleMap);
		}
	}
}