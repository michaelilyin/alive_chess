package Resources.BigMap.Landscape
{
	import flash.display.MovieClip;
	import flash.events.MouseEvent;
	import flash.filters.BitmapFilterQuality;
	import flash.filters.GlowFilter;
	
	public class Grass extends MovieClip
	{
		private var filter:GlowFilter;
		var myFilters:Array ;
		public var wayCost:int = 0;
		public var id:int = 0;
		
		public function Grass()
		{
			super();
			addEventListener(MouseEvent.MOUSE_MOVE,mouseMove);
			addEventListener(MouseEvent.MOUSE_OUT,mouseOut);
		}
		function mouseMove(e:MouseEvent):void{
			filter =  getBitmapFilter();
			myFilters = new Array();
			myFilters.push(filter);
			filters = myFilters;
		}
		private function getBitmapFilter():GlowFilter {
			var color:Number = 0xFFFF00;
			var alpha:Number = 1;
			var blurX:Number = 4;
			var blurY:Number = 4;
			var strength:Number = 2;
			var inner:Boolean = true;
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
			
		}
	}
}