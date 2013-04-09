package GameMovieClip
{
	import ACEvents.AuthorizeEvent.AuthorizeEvent;
	import ACEvents.ErrorMessageEvent;
	
	import AliveChessLibrary.Commands.RegisterCommands.*;
	
	import Message.MsgNotAuthorize;
	
	import Net.Transport;
	
	import fl.controls.Button;
	import fl.controls.TextInput;
	
	import flash.display.DisplayObject;
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.MouseEvent;
	
	import main.Main;
		
	public class LogIn extends MovieClip
	{
		public var btnConnect:Button;
		public var btnOk:Button;
		public var btnMenu:Button;
		public var txtLogin:TextInput;
		public var txtPas:TextInput;
		public var ma:main.Main = null;
		private var transport:Transport;
		private var  ar:AuthorizeResponse = null;
		
		public function LogIn(ma:Main)
		{
			super();
			this.ma = ma;
			btnOk.enabled = false;
			transport = Transport.getInstance();
			btnMenu.addEventListener(MouseEvent.CLICK,clickMenu);
			btnConnect.addEventListener(MouseEvent.CLICK,conHandler);
			btnOk.addEventListener(MouseEvent.CLICK,clickHandler);
		}
		private function conHandler(e:MouseEvent):void
		{
			transport.SConnect();
			removeChild(btnConnect);			
			btnOk.enabled = true;
		}
		
		public function clickHandler(e:MouseEvent):void
		{			
			trace("Login: " + txtLogin.text + ", Password: " + txtPas.text);
			var ar:AuthorizeRequest = new AuthorizeRequest();
			ar.login = txtLogin.text;
			ar.password = txtPas.text;	
			transport.addEventListener(AuthorizeEvent.AUTHORIZE,msgExecute);
			transport.SendCommand(ar);		
			transport.addEventListener(ErrorMessageEvent.ERROR,errorHandler);
		}
		private function msgExecute(e:Event):void
		{
			try{
				ar = AuthorizeResponse (transport.msgResponse);
				trace (ar.IsAuthorized.toString()+" "+ar.IsNewPlayer.toString()+ar.ErrorMessage);
				if(ar.IsAuthorized){
					trace("Можно загружать игру");
					ma.IsLogin = true;
					ma.removeChild(this);
					gotoAndStop(3);
					var g:Game = new Game(ma);				
				}
				transport.removeEventListener(AuthorizeEvent.AUTHORIZE,msgExecute);
			}
			catch(er:Error){
				/*trace(er.toString());*/
			}
		}
		private function errorHandler(e:Event):void{
			var msg:MsgNotAuthorize = new MsgNotAuthorize(this);
			addChild(msg);
		}
		public function clickMenu(e:Event):void{
			trace(this.ma._menu.call());
			var v:Boolean = this.ma._menu.call();
		}
	}
}