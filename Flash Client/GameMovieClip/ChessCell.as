package GameMovieClip
{
	import flash.display.MovieClip;

	public class ChessCell
	{
		private var a:int;
		private var b:int;
		private var x:int;
		private var y:int;
		private var figure:MovieClip = null;
		
		public function ChessCell(a:int,b:int)
		{
			this.a = a;
			this.b = b;
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