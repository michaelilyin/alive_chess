package GameMovieClip
{
	import fl.controls.Button;
	
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.MouseEvent;
	
	public class CastleInfo extends MovieClip
	{
		var castleController:CastleController = null;
		
	//	public var CastleResources:MovieClip;
		public var btnExit:Button;
		public var btnContact:Button;
		//public var btnResource:Button;
		public var btnArmy:Button;
		public var btnBuilding:Button;
		
		public function CastleInfo(id:int,cc:CastleController)
		{
			super();
			castleController = cc;
			btnExit.addEventListener(MouseEvent.CLICK, ExitHandler);
			btnContact.addEventListener(MouseEvent.CLICK, ContactHandler);
			//btnResource.addEventListener(MouseEvent.CLICK, ResourceHandler);
			btnArmy.addEventListener(MouseEvent.CLICK, ArmyHandler);
			btnBuilding.addEventListener(MouseEvent.CLICK, BuildingHandler);
		}
		private function ExitHandler(e:MouseEvent):void{
			castleController.bigMapRequest();
		}
		private function ContactHandler(e:MouseEvent):void{
			
		}
		/*private function ResourceHandler(e:MouseEvent):void{
			
		}*/
		private function ArmyHandler(e:MouseEvent):void{
			
		}
		private function BuildingHandler(e:MouseEvent):void{
			
		}
	}
}