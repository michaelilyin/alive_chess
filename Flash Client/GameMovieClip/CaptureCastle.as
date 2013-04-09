package GameMovieClip
{
	import ACEvents.BigMapEvents.BigMapEvent;
	
	import AliveChessLibrary.Commands.BigMapCommand.CaptureCastleRequest;
	import AliveChessLibrary.Commands.BigMapCommand.CaptureCastleResponse;
	import AliveChessLibrary.Commands.BigMapCommand.Castle;
	
	import Net.Transport;
	
	import fl.controls.Button;
	
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.MouseEvent;
	
	public class CaptureCastle extends MovieClip
	{
		public var btnCapture:Button;
		var transport:Transport = null;
		var castleId :int;
		var game:Game = null;
		
		public function CaptureCastle(castleId:int,g:Game)
		{
			super();
			this.game = g;
			this.castleId = castleId;
			transport = Transport.getInstance();
			this.btnCapture.addEventListener(MouseEvent.CLICK, clickCapture);
			transport.addEventListener(BigMapEvent.CAPTURE_CASTLE,captureListener);
		}
		private function clickCapture(e:MouseEvent):void{
			e.stopPropagation();
			var msg:CaptureCastleRequest = new CaptureCastleRequest();
			msg.castleId = castleId;
			transport.SendCommand(msg);
		}
		private function captureListener(e:Event):void{
			var msg:CaptureCastleResponse = CaptureCastleResponse(transport.msgResponse);	
			trace(msg.castle);
			trace(msg.castle.CastleId)
			trace(this.game);
			if(msg.castle.CastleId > 0){
				try{
					this.game.createBattle(msg.castle.CastleId);
				}
				catch(er:Error){
					trace(er);
				}
			}
		}
	}
}