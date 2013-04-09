package GameMovieClip
{
	import AliveChessLibrary.Commands.BigMapCommand.BasePoint;
	import AliveChessLibrary.Commands.BigMapCommand.Castle;
	import AliveChessLibrary.Commands.BigMapCommand.GetMapRequest;
	import AliveChessLibrary.Commands.BigMapCommand.GetMapResponse;
	import AliveChessLibrary.Commands.BigMapCommand.Mine;
	
	import Net.*;
	
	import Symbols.Area;
	
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.MouseEvent;
	
	public class BigMap extends MovieClip
	{
		private var m:MovieClip;
				
		public var sizeX:int = 0;
		public var sizeY:int = 0;
		
		public var speedX:Number = 0;
		public var speedY:Number = 0;
		public static const DECAY:Number = .8;
		
		//массивы элементов на карте
		private var bpoint:Array = null;
		private var castles:Array = null;
		private var mines:Array = null;		
		private var singleObjects:Array = null;
		private var multyObjects: Array = null;
		private var borders:Array = null;
		private var resources:Array = new Array();
		
		public var area:Area = new Area();
		
		public function BigMap(m:MovieClip)
		{
			super();			
			this.m = m;
			this.addEventListener(Event.ENTER_FRAME,enterFrameHandler);		
		}	
		public function hScroll(val:int, max:int = 10):void
		{			
			this.speedX += val; 
			if (max < 0) this.speedX = (this.speedX < max) ? max : this.speedX;
			if (max > 0) this.speedX = (this.speedX > max) ? max : this.speedX; 
		}
		
		public function vScroll(val:int, max:int = 10):void
		{
			this.speedY += val;
			if (max < 0) this.speedY = (this.speedY < max) ? max : this.speedY;
			if (max > 0) this.speedY = (this.speedY > max) ? max : this.speedY; 
		}	
		private function enterFrameHandler(e:Event):void
		{			
			/* Прибавляем скорость к положению карты */
			x += int(speedX);
			y += int(speedY);
	
			/* Горизонтальная прокрутка */
			if (x > 0) // Выезд за левый край
			{
				x = 0;
				speedX = 0;
			}
			else if (x < -1600 + 550) // Выезд за правый край
			{
				x = -1600 + 550;
				speedX = 0;
			}
			
			/* Вертикальная прокрутка */
			if (y > 0) // Выезд за верхний край
			{
				y = 0;
				speedY = 0;
			}
			else if (y < -1600 + 400) // Выезд за нижний край
			{
				y = -1600 + 400;
				speedY = 0;
			}			
			
			/* Применяем торможение к скорости */
			speedX *= DECAY;
			speedY *= DECAY;
		}
		
		public function setCenter():void{
			this.x = -(this.sizeX*16-m.width/2);
			this.y = -(this.sizeY*16-m.height/2);
		}
		public function setSize(sizeX:int,sizeY:int):void{
			this.sizeX = sizeX;
			this.sizeY = sizeX;	
		}
		public function setBasePoints(bpoint:Array):void{
			this.bpoint = bpoint;
		}
		public function setCastles(castles:Array):void{
			this.castles = castles;			
		}
		public function setMines(mines:Array):void{
			this.mines = mines;
		}
    public function setSingleObj(so:Array):void{
			singleObjects = so;
		}
		public function setMultyObj(mo:Array):void{
			multyObjects = mo;
		}
		public function setBorder(b:Array):void{
			borders = b;
		}
		public function setResources(r:Array):void{
			for each(var ro:Object in r){
				resources.push(ro);
			}
		}
		public function getCastles():Array{
			return castles;
		}
		public function getMines():Array{
			return mines;
		}
		public function getBPoint():Array{
			return bpoint;
		}
		public function getSingleObj():Array{
			return singleObjects;
		}
		public function getMultyObj():Array{
			return multyObjects;
		}
		public function getResources():Array{
			return resources;
		}
		public function getBorders():Array{
			return borders;
		}
		public function clearResources():void{
			resources = null;
		}
		public function clearMap():void{
			this.borders = null;
			this.bpoint = null;
			this.castles = null;
			this.mines = null;
			this.singleObjects = null;
			resources = new Array();
		/*	if(resources.length>0)
				for each(var r:Object in resources)
					resources.shift();*/
		}
	}
}