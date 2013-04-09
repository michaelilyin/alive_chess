package GameMovieClip
{
	import fl.controls.Button;
	
	import flash.display.MovieClip;
	import flash.events.MouseEvent;
	
	public class About extends MovieClip
	{
		public var btnM:Button;
		private var m:SMain = null;
		public function About(m:SMain)
		{
			super();
			this.m = m;
			btnM.addEventListener(MouseEvent.CLICK, clickListener);
		}
		
		private function clickListener(e:MouseEvent):void{
			m.removeChild(this);
		}
	}
}