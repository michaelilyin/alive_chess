package GameMovieClip.Chess
{
	import flash.display.MovieClip;
	import flash.events.MouseEvent;

	public class ChessCell extends MovieClip
	{
		private var a:int;
		private var b:int;
		private var figure:MovieClip = null;
		
		public function ChessCell(a:int,b:int)
		{
			stop();
			this.a = a;
			this.b = b;
			this.addEventListener(MouseEvent.CLICK, clickListener);
		}
		private function clickListener(e:MouseEvent):void{
			trace(this.a,this.b);
			gotoAndStop(2);
		}
		public function get Y():int
		{
			return y;
		}

		public function set Y(value:int):void
		{
			y = value;
		}

		public function get A():int
		{
			return a;
		}

		public function set A(value:int):void
		{
			a = value;
		}

		public function get B():int
		{
			return b;
		}

		public function set B(value:int):void
		{
			b = value;
		}

		public function get X():int
		{
			return x;
		}

		public function set X(value:int):void
		{
			x = value;
		}

		public function get Figure():MovieClip
		{
			return figure;
		}

		public function set Figure(value:MovieClip):void
		{
			figure = value;
		}


	}
}