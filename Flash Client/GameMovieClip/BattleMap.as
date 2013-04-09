package GameMovieClip
{
	import ACEvents.BattleEvent.BattleEvent;
	
	import AliveChessLibrary.Commands.BattleCommand.Battle;
	import AliveChessLibrary.Commands.BattleCommand.DownloadBattlefieldRequest;
	import AliveChessLibrary.Commands.BattleCommand.DownloadBattlefildResponse;
	import AliveChessLibrary.Commands.BattleCommand.Unit;
	import AliveChessLibrary.Commands.BigMapCommand.Castle;
	import AliveChessLibrary.GameObjects.Buildings.Castle;
	
	import GameMovieClip.Chess.ChessCell;
	import GameMovieClip.Chess.KnightBlack;
	import GameMovieClip.Chess.KnightWhite;
	
	import Net.Transport;
	
	import fl.controls.Button;
	
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.MouseEvent;
	
	public class BattleMap extends MovieClip
	{
		public var btnRollUp:Button;
		public var btnOk:Button;
		
		private var startX:int = 31;
		private var startY:int = 34;//?
		private var sizeCell:int = 42;
		var id:int;
		private var grid:Array = new Array();
		private var battle:Battle = null;
		private var transport:Transport = null;
		private var g:Game = null;
		private var msg:DownloadBattlefildResponse = null;
		private var casle:AliveChessLibrary.GameObjects.Buildings.Castle = null;
		
		//ФИГУРЫ
		//public var knightBlack:KnightBlack;
		
		public function BattleMap(id:int, g:Game/*c: AliveChessLibrary.GameObjects.Buildings.Castle*/)
		{
			super();
			this.addEventListener(MouseEvent.CLICK,clickListener);
			this.id = id;
			this.g = g;
		//	this.casle = casle;
			CreateGrid();
			Test();
			btnRollUp.addEventListener(MouseEvent.CLICK,RollUpClick);
			transport = Transport.getInstance();
			var msg:DownloadBattlefieldRequest = new DownloadBattlefieldRequest();
			msg.opponent = 0;
			//transport.SendCommand(msg);
			transport.addEventListener(BattleEvent.DOWNLOAD_BATTLE, buttleListener);
		}
		private function Test():void{
			var knightBlack:KnightBlack = new KnightBlack("A",2,10);
			knightBlack.x = this.startX + sizeCell/2;
			knightBlack.y = this.startY + sizeCell - sizeCell/2;
			this.addChild(knightBlack);
			
			var knightWidth:KnightWhite = new KnightWhite("A",2,10);
			knightWidth.x = this.startX - sizeCell/2+8*sizeCell;
			knightWidth.y = this.startY - sizeCell/2 + sizeCell;
			this.addChild(knightWidth);
		}
		private function clickListener(e:MouseEvent):void{
			e.stopPropagation();
		}
		private function RollUpClick(e:MouseEvent):void{
			e.stopPropagation();
			g.hideBattle();
		}
		
		private function CreateGrid():void{
			
			for(var i:int = 0 ; i < 8; i++){
				grid[i] = new Array();
				for(var j:int = 0 ; j < 8; j++){
					var cell:ChessCell = new ChessCell(i,j);
					grid[i][j] = cell;
					cell.X = startX + sizeCell*i;
					cell.Y = startY + sizeCell*j;
					addChild(cell);
				}
			}
		}
		private function buttleListener(e:Event):void{
			msg = DownloadBattlefildResponse(transport.msgResponse);
			battle = msg.Battle;
			trace("msg.Battle, msg.Battle.id, msg.Battle.OpponentArmy, msg.Battle.PlayerArmy, msg.Battle.Respondent, msg.Battle.youStep");
			trace(msg.Battle,msg.Battle.id,msg.Battle.OpponentArmy,msg.Battle.PlayerArmy,msg.Battle.Respondent,msg.Battle.youStep);
			var f:Unit;
			for each(var o:Object in battle.PlayerArmy){
				//белые
				f = Unit(o);
				switch (f.UnitType){
					case 0:
						//Knight конь
					/*	var knignt:KnightBlack = new KnightBlack("A",2, f.UnitCount);
						knignt.x = this.startX+sizeCell/2;
						knignt.y = this.startY+sizeCell/2;
						this.addChild(knignt);*/
						break;
					case 1:
						//Queen "A", 5
						break;
					case 2:
						//Rook ладья "A",1
						break;
					case 3:
						//Bishop слон "A",3
						break;
					case 10:
						//Pawn пешка "B",2
						break;
					default:
						break;
				}
			}
		}
	}
}