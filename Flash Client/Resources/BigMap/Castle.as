package Resources.BigMap
{
	import ACEvents.BigMapEvents.BigMapEvent;
	
	import GameMovieClip.CaptureCastle;
	import GameMovieClip.Game;
	
	import Net.Transport;
	
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.filters.BitmapFilterQuality;
	import flash.filters.GlowFilter;
	
	public class Castle extends MovieClip
	{
		private var castleId:int;		
		private var wayCost:int;
		private var flag:Boolean = false;
		private var filter:GlowFilter;
		private var captureBtn:CaptureCastle;
		private var game:Game = null; 
		var myFilters:Array ;
		
		public function Castle()
		{
			super();
					
			addEventListener(MouseEvent.MOUSE_MOVE,mouseMove);
			addEventListener(MouseEvent.MOUSE_OUT,mouseOut);
			addEventListener(MouseEvent.CLICK, clickListener);
		}
		private function clickListener(e:MouseEvent):void{
			captureBtn = new CaptureCastle(castleId,game);	
			trace(this.castleId);	
			captureBtn.x = 25;
			captureBtn.y = 3;
			addChild(captureBtn);
		}
		public function setGame(g:Game):void{
			this.game = g;
		}
		function mouseMove(e:MouseEvent):void{
			filter =  getBitmapFilter();
			myFilters = new Array();
			myFilters.push(filter);
			filters = myFilters;
		}
		private function getBitmapFilter():GlowFilter {
			var color:Number = 0xF4EC58;
			var alpha:Number = 0.6;
			var blurX:Number = 10;
			var blurY:Number = 10;
			var strength:Number = 2;
			var inner:Boolean = false;
			var knockout:Boolean = false;
			var quality:Number = BitmapFilterQuality.HIGH;
			
			return new GlowFilter(color,
				alpha,
				blurX,
				blurY,
				strength,
				quality,
				inner,
				knockout);
		}
		public function setMyCastle():void{
			flag = true;
			filter =  getBitmapFilterConst();
			myFilters = new Array();
			myFilters.push(filter);
			filters = myFilters;
		}
		function getBitmapFilterConst():GlowFilter {
			var color:Number = 0x009933;
			var alpha:Number = 0.6;
			var blurX:Number = 10;
			var blurY:Number = 10;
			var strength:Number = 2;
			var inner:Boolean = false;
			var knockout:Boolean = false;
			var quality:Number = BitmapFilterQuality.HIGH;
			
			return new GlowFilter(color,
				alpha,
				blurX,
				blurY,
				strength,
				quality,
				inner,
				knockout);
		}
		
		function mouseOut(e:MouseEvent):void{
			myFilters = null;
			filters = null;	
			if(flag)
				setMyCastle();
		}

		public function get CastleId():int
		{
			return castleId;
		}

		public function set CastleId(value:int):void
		{
			castleId = value;
		}

		public function get WayCost():int
		{
			return wayCost;
		}

		public function set WayCost(value:int):void
		{
			wayCost = value;
		}


	}
}