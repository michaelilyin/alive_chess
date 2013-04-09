package Test
{
	import AliveChessLibrary.Commands.*;
	import AliveChessLibrary.Commands.BigMapCommand.BigMapRequest;
	import AliveChessLibrary.Commands.BigMapCommand.BigMapResponse;
	import AliveChessLibrary.Commands.BigMapCommand.CaptureCastleRequest;
	import AliveChessLibrary.Commands.BigMapCommand.CaptureCastleResponse;
	import AliveChessLibrary.Commands.BigMapCommand.GetMapRequest;
	import AliveChessLibrary.Commands.BigMapCommand.GetMapResponse;
	import AliveChessLibrary.Commands.DialogCommand.BattleDialogMessage;
	import AliveChessLibrary.Commands.RegisterCommands.*;
	
	import Net.ProtoBufferCodec;
	import Net.Transport;
	
	import fl.controls.*;
	
	import flash.display.Shape;
	import flash.events.Event;
	import flash.events.MouseEvent;
	
	import GameMovieClip.Authorize;
	
	import main.Main;
	
	public class TestProto
	{
		private var taTest:TextArea;
		private var m:Main;
		private var taCon:TextArea;
		private var transport:Transport;
		private var ar:AuthorizeResponse;
		private var count:int=0;
		public var btnTestC:Button;
		public var btnAuth:Button;
		public var fAuth:GameMovieClip.Authorize;
		public var btnTest:Button;
		public var o:Shape;
		
		public function TestProto(m:Main,taCon:TextArea)
		{
			this.m = m;
			this.taCon = taCon;
			TestGraphicsBuild();
			transport = Transport.getInstance(this.m, this.btnTestC);
			transport.addEventListener(Transport.RESPONSE_COMPLITE,responseExecute);
			btnTestC.addEventListener(Event.ENTER_FRAME,testEvent);
		}
		private function testEvent(e:Event):void
		{
			//trace("Попали");
		}
		private function TestGraphicsBuild():void
		{
			var t:title1=new title1();
			t.x=5;
			t.y=10;
			m.addChild(t);
			
			taTest = new TextArea();
			taTest.move(5,93);
			taTest.setSize(538,275);
			m.addChild(taTest);
			
			o=new Shape(); 
			o.graphics.beginFill(0xFFFFFF,1);
			o.graphics.drawRect(6,70,536,20);
			o.graphics.endFill();
			m.addChild(o);
			
			btnTest = new Button();
			btnTest.height = 22;
			btnTest.width = 484;
			btnTest.label = "Test Proto Buffers library";				
			btnTest.move(59,69);
			btnTest.addEventListener(MouseEvent.CLICK,clickHandler);
			//btnTest.enabled = false;
			m.addChild(btnTest);
			
			btnTestC = new Button();
			btnTestC.height = 22;
			btnTestC.width = 54;
			btnTestC.label = "Connect";			
			btnTestC.move(5,69);
			btnTestC.addEventListener(MouseEvent.CLICK,clickHandler1);
			m.addChild(btnTestC);
			
			btnAuth = new Button();
			btnAuth.height = 22;
			btnAuth.width = 54;
			btnAuth.label = "Authorize";
			btnAuth.move(320,373);
			btnAuth.addEventListener(MouseEvent.CLICK,clickAuth);
			m.addChild(btnAuth);
		}
		private function clickAuth(e:MouseEvent):void
		{
			m.removeChild(btnTestC);
			m.removeChild(btnTest);
			m.removeChild(o);
			m.removeChild(taCon);
			m.removeChild(taTest);
			m.removeChild(btnAuth);
			fAuth = new GameMovieClip.Authorize(m);
			m.addChild(fAuth);
		}
		private function clickHandler1(e:MouseEvent):void
		{
			transport.SConnect();
		}
		
		private function clickHandler(e:MouseEvent):void
		{
			switch(count)
			{
			case 0:
				var reg:RegisterRequest = new RegisterRequest();
				reg.login = "newPlayer";
				reg.password = "newpw";
				//taTest.text +="> Send RegisterRequest: login: newPlayer, password: newpw \n"; 
				//transport.SendCommand(reg);
				var ar1:AuthorizeRequest = new AuthorizeRequest();
				ar1.login="player";
				ar1.password="pw";
				taTest.text +="> Send AuthorizeRequest: login: player, password: pw \n"; 
				transport.SendCommand(ar1);
				break;
			case 1:
				var MR:BigMapRequest=new BigMapRequest();
				taTest.text +="> Send BigMapRequest\n";
				transport.SendCommand(MR);
				break;
			//case 2:
			//	var ex:ExitFromGameRequest = new ExitFromGameRequest();
			//	taTest.text +="> Spasce Send ExitFromGameRequest\n";
			//	transport.SendCommand(ex);
			//	break;
			case 2:
				var map:GetMapRequest = new GetMapRequest();
				taTest.text +="> Send GetMapRequest\n";
				transport.SendCommand(map);
				break;
			}
			count++;
		
		}
		private function responseExecute(e:Event):void
		{
			switch(transport.IdCmd)
			{
				case 1:
					ar = AuthorizeResponse (transport.msgResponse);
					taTest.text += ">> Accepted: AuthorizeResponse IsAuthorized: "+ar.IsAuthorized.toString()+", IsNewPlayer: "+ar.IsNewPlayer.toString()+ar.ErrorMessage+"\n";
					break;
				case 3:
					var ar2 = ExitFromGameResponse (transport.msgResponse);
					taTest.text +=">> Accepted: ExitFromGameResponse \n";
					break;
				case 5:
					var r = RegisterResponse(transport.msgResponse);
					taTest.text +=">> Accepted: RegisterResponse \n";
					break;
				case 7:
					var bm:BigMapResponse = BigMapResponse (transport.msgResponse);
					taTest.text +=">> Accepted: BigMapResponse "+bm.isAllowed.toString()+" \n";	
					break;
				case 9:
					var car:CaptureCastleResponse = CaptureCastleResponse (transport.msgResponse);
					taTest.text +=">> Accepted: CaptureCastleResponse " + car.castle.CastleId.toString()+"\n";
					break;
				case 22:
					var map:GetMapResponse = GetMapResponse(transport.msgResponse);
					taTest.text +=">> Accepted: GetMapResponse\n";
			}			
		}
	}
}