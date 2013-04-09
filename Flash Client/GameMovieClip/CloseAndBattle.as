package GameMovieClip
{
	import fl.controls.Button;
	
	import flash.display.MovieClip;
	import flash.display.Sprite;
	import flash.events.MouseEvent;
	
	import main.Main;
	
	public class CloseAndBattle extends MovieClip
	{
		public var btnClose:Button;
		public var btnBattle:Button;
		private var m:Main = null;
		private var g:Game = null;
		
		var rect:Sprite = new Sprite();	
		
		public function CloseAndBattle(m:Main, g:Game)
		{
			super();
			this.m = m;
			this.g = g;
			this.addChild(rect);
			btnClose.addEventListener(MouseEvent.CLICK,CloseClick);
			btnClose.addEventListener(MouseEvent.MOUSE_MOVE,CloseMouseMove);
			btnClose.addEventListener(MouseEvent.MOUSE_OUT,CloseMouseOut);
			
			btnBattle.addEventListener(MouseEvent.CLICK,BattleClick);
			btnBattle.addEventListener(MouseEvent.MOUSE_MOVE,BattleMouseMove);
			btnBattle.addEventListener(MouseEvent.MOUSE_OUT,BattleMouseOut);
		}
		private function CloseClick(e:MouseEvent):void{
			e.stopPropagation();
			this.m._menu.call();
			trace("close");
		}
		private function CloseMouseMove(e:MouseEvent):void{
			this.stage.focus = rect;
		}
		private function CloseMouseOut(e:MouseEvent):void{
			this.stage.focus = null;
		}
		
		private function BattleClick(e:MouseEvent):void{
			e.stopPropagation();
			this.g.showBattle();
		}
		private function BattleMouseMove(e:MouseEvent):void{
			this.stage.focus = rect;
		}
		private function BattleMouseOut(e:MouseEvent):void{
			this.stage.focus = null;
		}
	}
}