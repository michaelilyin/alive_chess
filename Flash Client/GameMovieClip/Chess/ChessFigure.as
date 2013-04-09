package GameMovieClip.Chess
{
	import flash.display.MovieClip;
	import flash.events.MouseEvent;
	
	public class ChessFigure extends MovieClip
	{
		private var letter:String = "";
		private var number:int;
		private var count:int;
		
		public function ChessFigure(let:String, num:int, count:int)
		{
			super();
			letter = let;
			number = num;
			this.count = count;
			addEventListener(MouseEvent.CLICK,clickListener);
		}
		private function clickListener(e:MouseEvent):void{
			
		}
		public function get Letter():String
		{
			return letter;
		}

		public function set Letter(value:String):void
		{
			letter = value;
		}

		public function get Number():int
		{
			return number;
		}

		public function set Number(value:int):void
		{
			number = value;
		}

		public function get Count():int
		{
			return count;
		}

		public function set Count(value:int):void
		{
			count = value;
		}
		public function moveFigure():void{
			
		}

	}
}