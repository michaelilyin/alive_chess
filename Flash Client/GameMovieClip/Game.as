package GameMovieClip {
		
	import ACEvents.BigMapEvents.BigMapEvent;
	
	import AliveChessLibrary.Commands.BigMapCommand.BigMapRequest;
	import AliveChessLibrary.Commands.BigMapCommand.GetMapRequest;
	import AliveChessLibrary.GameObjects.Buildings.Castle;
	
	import Net.Transport;
	
	import Symbols.LoadGame;
	
	import flash.display.*;
	import flash.events.Event;
	import flash.events.KeyboardEvent;
	import flash.events.MouseEvent;
	import flash.text.TextField;
	import flash.ui.Keyboard;
	
	import main.Main;
	
	public class Game extends MovieClip {
	
		private var lg:LoadGame = null;;
		private var m:Main = null;
		
		private var bigMap:GameMovieClip.BigMap = null;
		public var battleMap:BattleMap = null;
		private var gameController:GameController = null;
		var rect:Sprite = new Sprite();
		
		private var king:GameMovieClip.King = null;
		private var castleOfKing:int;
		private var resourcesOfKing:Array = null;
		
		public var kingInfo:KingInfo = null;
		public var castleInfo:CastleInfo = null;
		
		public function Game(m:Main) {		
			this.m = m;
			sLoader();						
			gameController = new GameController(m,this, bigMap, king);
			king.addEventListener(Event.ADDED_TO_STAGE,AddedKingToStage);
			castleInfo.addEventListener(Event.ADDED_TO_STAGE,AddedCastleInfo);
			castleInfo.addEventListener(Event.REMOVED_FROM_STAGE, removeCastleInfo);
		}
		public function createBattle(id:int/*c: AliveChessLibrary.GameObjects.Buildings.Castle*/):void{
			battleMap = new BattleMap(id, this);
			addChild(battleMap);
		}
		public function hideBattle():void{			
			removeChild(battleMap);
		}
		public function showBattle():void{	
			if(battleMap!=null)
				addChild(battleMap);
			//для теста
			else{
				battleMap = new BattleMap(0, this);
				addChild(battleMap);
			}
		}
		private function removeCastleInfo(e:Event):void{
			castleInfo = null;
		}
		private function AddedKingToStage(e:Event):void{
			king.addChild(rect);
			this.addEventListener(Event.ENTER_FRAME,CreateFocus);
			/*if(castleInfo != null)
				removeChild(castleInfo);*/
		}
		private function CreateFocus(e:Event):void{			
			this.stage.focus = rect;
		}
		public function goNewFrame():void{
			gotoAndStop(4);
		}
		private function AddedCastleInfo(e:Event):void{
			king.removeChild(rect);		
		}
		public function addHungler():void{
		}
		
		public function ClearStage():void{
			lg.Destroy();
		}
		
		private function sLoader():void{
			lg = new LoadGame(m);
			lg.x = 0;
			lg.y = 0;
			m.addChild(lg);
		}		

		public function getResourcesOfKing():Array
		{
			return resourcesOfKing;
		}

		public function setResourcesOfKing(value:Array):void
		{
			resourcesOfKing = value;
		}

		public function getCastleOfKing():int
		{
			return castleOfKing;
		}

		public function setCastleOfKing(value:int):void
		{
			castleOfKing = value;
		}

		


	}
}
