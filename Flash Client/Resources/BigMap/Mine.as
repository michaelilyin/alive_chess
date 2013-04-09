package Resources.BigMap
{
	import flash.display.MovieClip;	
	import flash.events.MouseEvent;
	import flash.filters.BitmapFilterQuality;
	import flash.filters.GlowFilter;
	
	public class Mine extends MovieClip
	{
		private var filter:GlowFilter;
		var myFilters:Array ;
		
		public function Mine()
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
		function mouseOut(e:MouseEvent):void{
			myFilters = null;
			filters = null;	
			
		}
	}
}