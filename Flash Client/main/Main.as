package main {
	
	import GameMovieClip.Authorize;
	import GameMovieClip.LogIn;
	import GameMovieClip.SMain;
	
	import Net.Transport;
	
	import Symbols.LoadGame;
	
	import Test.TestProto;
	
	import fl.transitions.Fade;
	
	import flash.display.*;
	import flash.events.*;
	import flash.net.URLRequest;
	
	public class Main extends MovieClip {
		
		//public var ta:TextArea;
		//	private var fa:Authorize;
		private var log:LogIn = null;
		private var isLogin:Boolean = false;
		private var transport:Transport = Transport.getInstance();
		private var sMain:SMain = null;
		public function Main() {
			stop();
			_menu();
				//форма авторизации
			/*	fa=new Authorize(this);
				addChild(fa);*/
		}			
		public function createLogin():void{
			log = new LogIn(this);
			addChild(log);
			/*	fa=new Authorize(this);
			addChild(fa);*/
			removeChild(sMain);
		}
		public function _menu():Boolean{
			sMain = new SMain(this);
			addChild(sMain);
			if(log!=null){
			//	removeChild(log);
			}
			return true;
		}
		public function removeFa():void{
			//	fa = null;
		}		
		public function hideMenu():void{
			this.removeChild(sMain);
		}
		public function get IsLogin():Boolean
		{
			return isLogin;
		}

		public function set IsLogin(value:Boolean):void
		{
			isLogin = value;
		}

	}	
}
