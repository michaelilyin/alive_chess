package GameMovieClip
{
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.MovieClip;
	import flash.display.Sprite;
	import flash.geom.*;
	
	public class PathController
	{
		private var path:MovieClip;
		private var cells:Sprite;
	//	var bmpData:BitmapData = null;
	//	var m:Matrix = null;
		
		public function PathController(path:MovieClip)
		{
			this.path = path;
		}
		public function SetPathX(posX:int):void{
			this.path.x += posX;
		}
		public function SetPathY(posY:int):void{
			this.path.y += posY;
		}
		public function setSize(sizeX:int,sizeY:int):void{
			path.width = sizeX;
			path.height = sizeY;
		}
		public function setPos(posX:int,posY:int):void{
			path.x = posX;
			path.y = posY;
		}
		public function CreatePath(obj:Sprite):void{
			cells = new Sprite();
			cells.graphics.lineStyle(1, 0xFFFF99);			
			cells.graphics.beginFill(0xFFFF99, 1);
			cells.graphics.drawRect(obj.x, obj.y, obj.width, obj.height);
			path.addChild(cells);
		}
	/*	private function rRastr():void{			
			var r:Rectangle;
			r = cells.getRect(cells);
			m.translate(-r.x, -r.y);
			bmpData.draw(cells, m);
			var _bmpPath = new Bitmap(bmpData);
			path.addChild(_bmpPath);
		}*/
	}
}