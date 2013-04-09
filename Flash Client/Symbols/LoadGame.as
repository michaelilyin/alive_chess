package Symbols
{
	import flash.display.MovieClip;
	
	public class LoadGame extends MovieClip
	{
		var m:MovieClip = null;
		public function LoadGame(m:MovieClip)
		{
			this.m = m;
			super();
		}
		public function Destroy():void
		{
			m.removeChild(this);
		}
	}
}