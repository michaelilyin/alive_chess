package GameMovieClip
{
	import ACEvents.BigMapEvents.BigMapEvent;
	
	import AliveChessLibrary.Commands.BigMapCommand.BasePoint;
	import AliveChessLibrary.Commands.BigMapCommand.Castle;
	import AliveChessLibrary.Commands.BigMapCommand.ComeInCastleRequest;
	import AliveChessLibrary.Commands.BigMapCommand.ComeInCastleResponse;
	import AliveChessLibrary.Commands.BigMapCommand.ContactCastleRequest;
	import AliveChessLibrary.Commands.BigMapCommand.ContactCastleResponse;
	import AliveChessLibrary.Commands.BigMapCommand.Dialog;
	import AliveChessLibrary.Commands.BigMapCommand.GetKingRequest;
	import AliveChessLibrary.Commands.BigMapCommand.GetKingResponse;
	import AliveChessLibrary.Commands.BigMapCommand.GetObjectsResponse;
	import AliveChessLibrary.Commands.BigMapCommand.GetResourceMessage;
	import AliveChessLibrary.Commands.BigMapCommand.King;
	import AliveChessLibrary.Commands.BigMapCommand.MoveKingRequest;
	import AliveChessLibrary.Commands.BigMapCommand.MoveKingResponse;
	import AliveChessLibrary.Commands.BigMapCommand.Position;
	import AliveChessLibrary.GameObjects.Buildings.Castle;
	import AliveChessLibrary.GameObjects.Resources.Resource;
	
	import BigMapEvents.GetMapEvent;
	
	import Net.Transport;
	
	import Resources.BigMap.Castle;
	
	import flash.display.*;
	import flash.events.*;

	public class KingController 
	{
		var transport:Transport = null;
		private var kingCommand:AliveChessLibrary.Commands.BigMapCommand.King=null;
		private var king:GameMovieClip.King = null;
		private var selectPoint:BasePoint = null;
		private var bigMapController:BigMapController;
		
		public function KingController(king:GameMovieClip.King, bigMapController:BigMapController)
		{
			this.king = king;	
			this.bigMapController = bigMapController;
			ToServer();
			transport.addEventListener(BigMapEvent.GET_RESOURCE,getResourceHandler);
		}
		private function getResourceHandler(e:Event):void{
			var msgRes:GetResourceMessage = GetResourceMessage(transport.msgResponse);
			//var r:Resource = msgRes.resource;
			trace("Получен ресурс тип",msgRes.resource.ResourceType,"count",msgRes.resource.ResourceCount);			
			king.AddResource(msgRes.resource);
		}
		public function setBigMapController(bmc:BigMapController):void{
			this.bigMapController = bmc;
		}
		public function setSelectPoint(point:BasePoint):void{
			selectPoint = point;
			if (point != null)
				trace(point.X, point.Y);
		}
		public function moveKing(posX:int,posY:int):void{
			transport = Transport.getInstance();
			transport.addEventListener(BigMapEvent.MOVE_KING,msgMoveKing);
			var mKing:MoveKingRequest = new MoveKingRequest();
			mKing.x = posX;
			mKing.y = posY;	
			transport.SendCommand(mKing);
			
		}
	/*	private function getObj(e:Event):void{
			var msg:GetObjectsResponse = GetObjectsResponse(transport.msgResponse);
			trace("GetObjectsResponse ",msg.Kings.length);
			
		}*/
		private function EnterFrameListener(e:Event):void{
			king.moveMoveKing();			
		}
		private function msgMoveKing(e:Event):void{
			transport.removeEventListener(BigMapEvent.MOVE_KING,msgMoveKing);
			try{			
				var pathKing:MoveKingResponse = MoveKingResponse(transport.msgResponse);			
				var s:Position = null;
				for each(s in pathKing.steps){	
					bigMapController.delRes(s.X,s.Y);
					/*var r:Resource = bigMapController.getResource(s.X,s.Y);
					if(r!=null){
						Res(r);
					}*/
					var c:Resources.BigMap.Castle = bigMapController.getCastle(s.X,s.Y);
					if(c != null){		
						var msgComeInCastle:ComeInCastleRequest = new ComeInCastleRequest();
						msgComeInCastle.castleId = c.CastleId;
						transport.SendCommand(msgComeInCastle);
					}
					else{
					//trace(bigMapController.getObjectOfGrid(s.X,s.Y));
						king.delayX = king.x - s.X * 32;
						king.delayY = king.y - s.Y * 32;
					}
				}
			}
			catch(er:Error){
				trace(er);
			}
		}
		private function Res(res:Resource):void{
		}
		private function ToServer():void{
			transport = Transport.getInstance();
			transport.addEventListener(BigMapEvent.GET_KING,msgKing);
			sendKing();
		}
		private function sendKing():void{			
			var getKing:GetKingRequest = new GetKingRequest();
			//getKing.KingId = 0;		
			transport.SendCommand(getKing);
		}
	
		private function msgKing(e:Event):void{
			try{			
				var k:GetKingResponse = GetKingResponse(transport.msgResponse);
				king.setSize(32,32);	
				transport.removeEventListener(BigMapEvent.GET_KING,msgKing);
			}
			catch(er:Error){
				trace(er);
			}
		}
	}
}