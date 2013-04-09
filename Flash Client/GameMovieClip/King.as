package GameMovieClip
{
	import AliveChessLibrary.GameObjects.Resources.Resource;
	
	import flash.debugger.enterDebugger;
	import flash.display.MovieClip;
	import flash.display.Sprite;
	import flash.events.*;
	import flash.events.KeyboardEvent;
	import flash.events.MouseEvent;
	import flash.ui.Keyboard;
	
	public class King extends MovieClip
	{
		private var kingId:int = 0;
		private var kingName:String = "";
		private var kingExperience:int = 0;		
		private var kingMilitaryRank:int = 0;
		var resources:Vector.<Resource> = new Vector.<Resource>();		 
		private var castle:int = 0;
		
		private var m:MovieClip = null;
		private var map:BigMap = null;
		
		var rect:Sprite = new Sprite();		
		private var _border:int = 100;// Размер границы
		private var tempX:int;
		private var tempY:int;
		//для перемещения
		public var kode:int = -1;
		public var delayX:int = 0;
		public var delayY:int = 0;
		private var count:int = 0;
		var step = 2;		
		private var _speedX:int = 2;
		private var _speedY:int = 2;
				
		public function King(m:MovieClip,map:BigMap)
		{
			super();
			this.m = m;	
			this.map = map;	
			/*var r1:Resource = new Resource();
			r1.ResourceCount = 0;
			r1.ResourceType = 0;
			resources.push(r1);
			var r2:Resource = new Resource();
			r2.ResourceCount = 0;
			r2.ResourceType = 1;
			resources.push(r2);
			var r3:Resource = new Resource();
			r3.ResourceCount = 0;
			r3.ResourceType = 2;
			resources.push(r3);
			var r4:Resource = new Resource();
			r4.ResourceCount = 0;
			r4.ResourceType = 3;
			resources.push(r4);
			var r5:Resource = new Resource();
			r5.ResourceCount = 0;
			r5.ResourceType = 4;
			resources.push(r5);*/
			addEventListener(Event.ENTER_FRAME, EnterFrameListener);			
		}
		private function EnterFrameListener(e:Event):void{
			moveMoveKing();			
		}
		public function removeListener():void{
			removeEventListener(Event.ENTER_FRAME, EnterFrameListener);	
		}
		public function addHandler():void{
			this.addChild(rect);			
			this.addEventListener(Event.ADDED_TO_STAGE,AddedKingToStage);			
		}
		private function AddedKingToStage(e:Event):void{
			this.addEventListener(Event.ENTER_FRAME,CreateFocus);
			this.stage.addEventListener(KeyboardEvent.KEY_DOWN,KeyDownHandler);
		}
		private function CreateFocus(e:Event):void{			
			this.stage.focus = rect;
		}
		//движение короля по клеткам
		public function moveMoveKing():void{
			tempX = x; tempY = y;
			if(count<16){
				if(delayX > 0){
					tempX -=  _speedX;
					delayX = Math.abs(delayX)-_speedX;
				}
				else if(delayX < 0){
					tempX +=  _speedX;
					delayX = -( Math.abs(delayX)-_speedX );
				}				
				count++;
			}
			if(count>=16 && count<32){
				if(delayY > 0){
					tempY -=  _speedY;
					delayY = Math.abs(delayY) - _speedY;
				}
				else if(delayY < 0){
					tempY +=  _speedY;	
					delayY = -( Math.abs(delayY) - _speedY );
				}
				count++;
			} 
			if(count == 32)
				count = 0;
			/* Проверка столкновения с границами карты */
			if (x > 1600-32)
			{
				tempX -=  _speedX;
			}
			else if (x < 32)
			{
				tempX +=  _speedX;
			}
			if (y > 1600-32)
			{
				tempY -=  _speedY;
			}
			else if (y < 32)
			{
				tempY +=  _speedY;
				
			}
			x = tempX; y = tempY;
			map.area.x = x+width - map.area.width/2;
			map.area.y = y+height - map.area.height/2;
			updMapPos();
		}
		private function KeyDownHandler(e:KeyboardEvent):void
		{
			tempX = x; tempY = y;
			switch (e.keyCode)
			{
				case 39 :
					tempX +=  _speedX;
					break;// ->
				case 37 :
					tempX -=  _speedX;
					break;// <-
				case 40 :					
					tempY +=  _speedY;					
					break;// Вниз
				case 38 :
					tempY -=  _speedY;
					break;// Вверх
			}
			/* Проверка столкновения с границами карты */
			if (x > 1600-32)
			{
				tempX -=  _speedX;
			}
			else if (x < 32)
			{
				tempX +=  _speedX;
			}
			if (y > 1600-32)
			{
				tempY -=  _speedY;
			}
			else if (y < 32)
			{
				tempY +=  _speedY;
				
			}
			x = tempX; y = tempY;
			map.area.x = x+width - map.area.width/2;
			map.area.y = y+height - map.area.height/2;
			updMapPos();
		}
		
		// Protected Methods:
		
		private function updMapPos():void
		{
			//trace(map.x,map.y, x, y);
			if (x < Math.abs(map.x) + _border)
			{
				map.hScroll(1,_speedX);
				// Вправо;
			}
			else if (x > Math.abs(map.x) + 550 - _border)
			{
				map.hScroll(-1,_speedX);
			}
			// Влево;
			
			if (y < Math.abs(map.y) + _border)
			{
				map.vScroll(1,_speedY);
				// Вниз;
			}
			else if (y > Math.abs(map.y) + 400 - _border)
			{
				map.vScroll(-1,_speedY);
			}
		}
		
		
		public function setSize(sizeX:int,sizeY:int):void{
			this.width = sizeX;
			this.height = sizeY;
			
		}
		public function setPosition(posX:int,posY:int):void{
			this.x = posX;
			this.y = posY;		
		}
		public function setCenter():void{
			this.x = 200;
			this.y = 200;
		}
		public function AddToStage():void{
			this.x = 0; this.y = 0;
			m.addChild(this);
		}				

		public function get Castle():int
		{
			return castle;
		}

		public function set Castle(value:int):void
		{
			castle = value;
		}

		
		public function AddResource(r:Resource):void{
		/*	if(resources.length == 0)
				resources.push(r);
			else{*/
			var f:Boolean = false;
			for each(var o:Resource in resources){					
				if(o.ResourceType == r.ResourceType){
					o.ResourceCount+=r.ResourceCount;
					f = true;	
				}
			}
			if(!f){
				resources.push(r);
			}	
			
		}

		public function get Resources():Vector.<Resource>
		{
			return resources;
		}

		public function set Resources(value:Vector.<Resource>):void
		{
			resources = value;
		}

		public function get KingId():int
		{
			return kingId;
		}

		public function set KingId(value:int):void
		{
			kingId = value;
		}

		public function get KingName():String
		{
			return kingName;
		}

		public function set KingName(value:String):void
		{
			kingName = value;
		}

		public function get KingExperience():int
		{
			return kingExperience;
		}

		public function set KingExperience(value:int):void
		{
			kingExperience = value;
		}

		public function get KingMilitaryRank():int
		{
			return kingMilitaryRank;
		}

		public function set KingMilitaryRank(value:int):void
		{
			kingMilitaryRank = value;
		}


	}
}