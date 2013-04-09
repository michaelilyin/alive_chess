package GameMovieClip
{
	import fl.controls.Button;
	
	import flash.display.MovieClip;
	import flash.events.MouseEvent;
	
	import main.Main;
	
	public class SMain extends MovieClip
	{
		public var btnLogin:Button;
		public var btnReg:Button;
		public var btnAbout:Button;
		public var btnGame:Button;
		private var ma:Main = null;
		
		public function SMain(m:Main)
		{
			super();
			this.ma = m;
			if (ma.IsLogin){
				btnReg.visible = false;
				btnLogin.label = "Exit";
			}
			btnLogin.addEventListener(MouseEvent.CLICK,clickLogin);
			btnGame.addEventListener(MouseEvent.CLICK,gameClick);
			btnAbout.addEventListener(MouseEvent.CLICK,aboutClick);
		}
		private function aboutClick(e:MouseEvent):void{
			var about:About = new About(this);
			addChild(about);
		}
		private function clickLogin(e:MouseEvent):void{
			if(btnLogin.label=="Login"){
				ma.createLogin();
			}
			else{/*шлём exit*/}
		}
		private function gameClick(e:MouseEvent):void{
			if (ma.IsLogin){
				ma.hideMenu.call();
			}
		}
	}
}