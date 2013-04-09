package GameMovieClip
{
	import flash.display.MovieClip;
	import flash.display.Sprite;
	import flash.events.MouseEvent;
	
	import main.Main;
	
	public class Close extends MovieClip
	{
		private var m:Main = null;
		var rect:Sprite = new Sprite();	
		public function Close(m:Main)
		{
			super();
			this.m = m;
			this.addChild(rect);
			addEventListener(MouseEvent.CLICK,clickListener);
			addEventListener(MouseEvent.MOUSE_MOVE,mouseMove);
			addEventListener(MouseEvent.MOUSE_OUT,mouseOut);
		}
		private function clickListener(e:MouseEvent):void{
			e.stopPropagation();
			this.m._menu.call();
			trace("close");
		}
		private function mouseMove(e:MouseEvent):void{
			this.stage.focus = rect;
		}
		private function mouseOut(e:MouseEvent):void{
			this.stage.focus = null;
		}
	}
}