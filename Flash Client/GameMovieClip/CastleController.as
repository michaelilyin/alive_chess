package GameMovieClip
{
	import ACEvents.BigMapEvents.BigMapEvent;
	
	import AliveChessLibrary.Commands.BigMapCommand.BigMapRequest;
	import AliveChessLibrary.Commands.BigMapCommand.ContactCastleRequest;
	import AliveChessLibrary.Commands.BigMapCommand.ContactCastleResponse;
	import AliveChessLibrary.Commands.BigMapCommand.GetMapRequest;
	import AliveChessLibrary.Commands.CastleCommand.*;
	
	import BigMapEvents.GetMapEvent;
	
	import Net.Transport;
	
	import flash.display.MovieClip;
	import flash.events.Event;
	
	public class CastleController
	{
		private var castleId:int;
		var castleInfo:MovieClip;
		var transport:Transport = null;
		var gc:GameController = null;
		
		public function CastleController(/*castleId:int, castleInfo:MovieClip,*/ gameContr:GameController)
		{
			//this.castleId = castleId;
		//	this.castleInfo = castleInfo;			
			transport = Transport.getInstance();
			gc = gameContr;
		}
		public function bigMapRequest():void{
			transport.SendCommand(new BigMapRequest());
			gc.delCastleInfo();
		}
		
		public function ContactCastle():void{
			var contact:ContactCastleRequest = new ContactCastleRequest();
			contact.castleId = castleId;
			transport.addEventListener(BigMapEvent.CONTACT_CASTLE,ContactHandler);
			transport.SendCommand(contact);
		}
		public function Resource():void{
			
		}
		private function ContactHandler(e:Event):void{
			var contact:ContactCastleResponse = ContactCastleResponse(transport.msgResponse);			
		}

		public function get CastleId():int
		{
			return castleId;
		}

		public function set CastleId(value:int):void
		{
			castleId = value;
		}

	}
}