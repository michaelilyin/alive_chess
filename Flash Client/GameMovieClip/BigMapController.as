package GameMovieClip
{
	import ACEvents.BigMapEvents.BigMapEvent;
	
	import AliveChessLibrary.Commands.BigMapCommand.BasePoint;
	import AliveChessLibrary.Commands.BigMapCommand.Castle;
	import AliveChessLibrary.Commands.BigMapCommand.GetMapRequest;
	import AliveChessLibrary.Commands.BigMapCommand.GetMapResponse;
	import AliveChessLibrary.Commands.BigMapCommand.GetObjectsResponse;
	import AliveChessLibrary.Commands.BigMapCommand.King;
	import AliveChessLibrary.Commands.BigMapCommand.Mine;
	import AliveChessLibrary.GameObjects.Objects.SingleObject;
	import AliveChessLibrary.GameObjects.Resources.Resource;
	
	import Net.Transport;
	
	import Resources.BigMap.Castle;
	import Resources.BigMap.Landscape.Grass;
	import Resources.BigMap.Landscape.Ground;
	import Resources.BigMap.Landscape.Snow;
	import Resources.BigMap.Mine;
	import Resources.BigMap.Mine.MineCoal;
	import Resources.BigMap.Mine.MineGold;
	import Resources.BigMap.Mine.MineIron;
	import Resources.BigMap.Mine.MineStones;
	import Resources.BigMap.Mine.MineWood;
	
	import Symbols.Obstacle;
	import Symbols.Tree;
	
	import com.netease.protobuf.ReadUtils;
	
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.MouseEvent;
	
	import main.Main;
	
	public class BigMapController
	{
		private var bigMap:BigMap = null;
		var transport:Transport = null;
		var mapResponse:GetMapResponse = null;
		var king:GameMovieClip.King = null;
		var otherKings:Array;
		//двумерные массивы для быстрого поиска объекта по расположению на карте
		var grid:Array = new Array();
		var castles:Array = new Array();
		var resources:Array = new Array();
		var game:Game = null;
		var m:Main = null;
		var gettingRes:Array = new Array();
		public function BigMapController(bigMap:BigMap,king:GameMovieClip.King, g:Game,m:Main)
		{
			this.bigMap = bigMap;
			this.king = king;
			this.game = g;
			this.m = m;
			ToServer();			
		}
		public function setCenter():void{
			var tempX:int = 550/2 - king.x;
			var tempY:int = 400/2 - king.y;
			if(tempX<(550-50*32))
				tempX = 550-50*32;
			else if(tempX>0)
				tempX = 0;
			bigMap.x = tempX;
			if(tempY <(400-50*32))
				tempY = 400-50*32;
			else if(tempY>0) 
				tempY = 0;
			bigMap.y = tempY;
		}
		
		private function ToServer():void{
			transport = Transport.getInstance();
			transport.addEventListener(BigMapEvent.GET_MAP,GetMap);
			transport.SendCommand(new GetMapRequest());						
		}
		
		private function GetMap(e:Event):void{
			try{	
				trace("GetMapResponse");
				mapResponse = GetMapResponse(transport.msgResponse);		
				//создаём пустую двумерную сетку, аналог карты, для хранений объектов
				bigMap.setSize(mapResponse.sizeMapX,mapResponse.sizeMapY);
				clearMap();
				// stage width / grid size = номер колонки
				for(var i:int = 0; i < mapResponse.sizeMapX; i++)
				{
					grid[i] = new Array();
					// stage height / grid size = номер строки
					for(var j:int = 0; j < mapResponse.sizeMapY; j++)
					{
						grid[i][j] = new Array();
					}
				}
				//bigMap.setCenter();
				bigMap.setBasePoints(mapResponse.BasePoints);
				bigMap.setCastles(mapResponse.Castles);
				bigMap.setMines(mapResponse.Mines);	
       			bigMap.setSingleObj(mapResponse.SingleObjects);		
				bigMap.setBorder(mapResponse.Borders); 
				
				buildLandscape();
				buildCastles();
				//buildMines();
				bigMap.addChild(king);				
				//transport.removeEventListener(BigMapEvent.GET_MAP,GetMap);
				transport.addEventListener(BigMapEvent.GET_OBJECT,getObj);
			}
			catch(er:Error){trace(er.toString());}
		}
		public function clearMap():void{			
			while(bigMap.numChildren>0){
				bigMap.removeChildAt(0);
			}
			grid = new Array();
			castles = new Array();
			this.bigMap.clearMap();
		}
		public function getObjectOfGrid(_x:int,_y:int):Object{
			return grid[_x][_y];
		}
		public function getResource(_x:int,_y:int):MovieClip{
			var result:MovieClip = null;
			try{
				result = resources[_x][_y] as MovieClip;
			}
			catch(er:Error){/* */}			
			return result;
		}
		public function getCastle(_x:int,_y:int):Resources.BigMap.Castle{	
			var result:Resources.BigMap.Castle = null;
			try{
				result = castles[_x][_y] as Resources.BigMap.Castle;
			}
			catch(er:Error){/* */}			
			return result;
		}
		public function getCastleofId(id:int):Resources.BigMap.Castle{
			var result:Resources.BigMap.Castle = null;		
			var obj:Resources.BigMap.Castle = null;
			for(var i:int = 0; i < castles.length; i++){
				var a:Array = castles[i] as Array;
				for(var j:int = 0; j < a.length; j++){
					obj = castles[i][j] as Resources.BigMap.Castle;
					if(obj != null){
						if(id == obj.CastleId)
							result = obj;			
					}
				}
			}			
			return result;
		}
	
		private function getObj(e:Event):void{
			trace("getObj");
			var msg:GetObjectsResponse = GetObjectsResponse(transport.msgResponse);					
			if (msg.Kings != null){
				if(otherKings != null)
				{
					for each(var kk:Object in otherKings){
						var kr:GameMovieClip.King = GameMovieClip.King(kk);
						bigMap.removeChild(kr);
					}
					otherKings = null;
				}
				otherKings = new Array();
				trace("msg.Kings",msg.Kings.toString());
				for each(var o:Object in msg.Kings){
					var k:AliveChessLibrary.Commands.BigMapCommand.King = AliveChessLibrary.Commands.BigMapCommand.King(o);
					var otherKing:GameMovieClip.King = new GameMovieClip.King(this.m,this.bigMap);
					otherKing.x = k.X*32;
					otherKing.y = k.Y*32;
					otherKing.KingId = k.KingId;
					otherKing.KingName = k.KingName;
					otherKing.removeListener();
					//...
					otherKings.push(otherKing);
					bigMap.addChild(otherKing);
				}
				
			}
			if(msg.Resources != null){	
				setResources(msg.Resources);
			}		
		}
		private function createCoal(r:Resource):MovieClip{			
			trace("createCoal");
			var coal:MineCoal = new MineCoal();
			coal.x = r.X*32;
			coal.y = r.Y*32;
			coal.setWayCost(r.WayCost);
			coal.setResourceId(r.ResourceId);
			coal.setResourceCount(r.ResourceCount);
			return coal;
		}
		private function createGold(r:Resource):MovieClip{
			var gold:MineGold = new MineGold();
			gold.x = r.X*32;
			gold.y = r.Y*32;
			gold.setWayCost(r.WayCost);
			gold.setResourceId(r.ResourceId);
			gold.setResourceCount(r.ResourceCount);
			return gold;
		}
		private function createIron(r:Resource):MovieClip{
			var iron:MineIron = new MineIron();
			iron.x = r.X*32;
			iron.y = r.Y*32;
			iron.setWayCost(r.WayCost);
			iron.setResourceId(r.ResourceId);
			iron.setResourceCount(r.ResourceCount);
			return iron;
		}
		private function createStones(r:Resource):MovieClip{
			var stone:MineStones= new MineStones();
			stone.x = r.X*32;
			stone.y = r.Y*32;
			stone.setWayCost(r.WayCost);
			stone.setResourceId(r.ResourceId);
			stone.setResourceCount(r.ResourceCount);
			return stone;
		}
		private function createWood(r:Resource):MovieClip{
			var wood:MineWood= new MineWood();
			wood.x = r.X*32;
			wood.y = r.Y*32;
			wood.setWayCost(r.WayCost);
			wood.setResourceId(r.ResourceId);
			wood.setResourceCount(r.ResourceCount);
			return wood;
		}		
		public function setResources(r:Array):void{
			for(var i:int = 0; i < mapResponse.sizeMapX; i++)
			{
				resources[i] = new Array();				
			}
			var resource:MovieClip = null;		
			bigMap.setResources(r);
			for each(var res:Object in r){
				var rs:Resource = res as Resource;		
				switch (rs.ResourceType){
					case 0:
						resource = createCoal(rs);
						break;
					case 1:
						resource = createGold(rs);
						break;
					case 2:
						resource = createIron(rs);
						break;
					case 3:
						resource = createStones(rs);
						break;
					case 4:
						resource = createWood(rs);
						break;
					default:
						trace("Out of range");
						break;					
				}
				if(resource!=null){
					resource.width = 25;
					resource.height = 25;
					resources[resource.x/32][resource.y/32] = resource;
					bigMap.addChild(resource);
				}
			}
		}
		public function delRes(i:int,j:int):void{
			var r:MovieClip = getResource(i,j);
			if(r!=null)
				bigMap.removeChild(r);			
		}
		private function buildLandscape():void{	
			buildLandscapeDefault();
			var so:SingleObject = null;
			for each(var obj:Object in bigMap.getSingleObj()){
				so = obj as SingleObject;	
				switch (so.SingleObjectType){
					case 0:
						var point:MovieClip= new Tree();
						point.x = so.X*32;
						point.y = so.Y*32;
						point.wayCost = so.WayCost;
						point.id = so.SingleObjectId;
						bigMap.addChild(point);
						grid[so.X][so.Y].push(point);
						break;
					case 1:
						var point:MovieClip = new Obstacle();
						point.x = so.X*32;
						point.y = so.Y*32;
						point.wayCost = so.WayCost;
						point.id = so.SingleObjectId;
						bigMap.addChild(point);
						grid[so.X][so.Y].push(point);
						break;				
					default:
						//buildLandscapeDefault();
						break;
				}
			}		
		}		
		private function buildLandscapeDefault():void{
			var _x:int = 0;
			var _y:int = 0;
			var it:int;
			var j:int;
			for (it = 0; it<bigMap.sizeX; it++){
				for(j = 0; j<bigMap.sizeY; j++){
					var point:MovieClip = new Grass();
					point.x = _x;
					point.y = _y;								
					bigMap.addChild(point);	
					if(_x>0 && _y>0)
						grid[_x/32][_y/32].push(point);		
						else if(_x ==0)
							grid[0][_y/32].push(point);	
							else if(_y==0)
								grid[_x/32][0].push(point);	
					_y+=32;
				}	
				_y = 0;
				_x = _x + 32;
			}
		}
		
		private function buildCastles():void{
			//создаём пустой двумерный массив
			for(var i:int = 0; i < mapResponse.sizeMapX; i++)
			{
				castles[i] = new Array();
			/*	for(var j:int = 0; j < mapResponse.sizeMapY; j++)
				{
					castles[i][j] = new Array();
				}*/
			}
			for each (var k:Object in bigMap.getCastles()) 
			{
				var myCastle:Resources.BigMap.Castle = new Resources.BigMap.Castle();
				myCastle.CastleId = AliveChessLibrary.Commands.BigMapCommand.Castle(k).CastleId;
				myCastle.WayCost = AliveChessLibrary.Commands.BigMapCommand.Castle(k).WayCost;
				myCastle.x = AliveChessLibrary.Commands.BigMapCommand.Castle(k).LeftX*32;
				myCastle.y = AliveChessLibrary.Commands.BigMapCommand.Castle(k).TopY*32;
				myCastle.height = AliveChessLibrary.Commands.BigMapCommand.Castle(k).Height*32;
				myCastle.width = AliveChessLibrary.Commands.BigMapCommand.Castle(k).Width*32;	
				myCastle.setGame(this.game);
				if(myCastle.CastleId == king.Castle){
					trace("Нашли свой замок ");
					myCastle.setMyCastle();
				}
				bigMap.addChild(myCastle);
				castles[myCastle.x/32][myCastle.y/32] = myCastle;
			}			
		}
	}
}