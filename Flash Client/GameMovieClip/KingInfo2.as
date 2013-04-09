package GameMovieClip
{
	import AliveChessLibrary.GameObjects.Resources.Resource;
	
	import Resources.BigMap.Castle;
	
	import fl.controls.Label;
	
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.MouseEvent;
	
	public class KingInfo2 extends MovieClip
	{
		public var lKingName:fl.controls.Label;
		public var lKingId:fl.controls.Label;
		public var lRank:fl.controls.Label;
		public var lExperience:fl.controls.Label;
		public var lCastle:fl.controls.Label;
		public var lCastleCount:fl.controls.Label;
		public var lMineCount:fl.controls.Label;
		//resources
		public var lGold:fl.controls.Label;
		public var lIron:fl.controls.Label;
		public var lStone:fl.controls.Label;
		public var lWood:fl.controls.Label;
		public var lCoal:fl.controls.Label;
		
		private var imgCastle:Castle;
		private var king:King;
		private var castle:Castle;
		
		public function KingInfo2()
		{
			super();
			this.king = king;
			this.castle = castle;
			/*imgCastle = new Castle();
			imgCastle.setMyCastle();			
			imgCastle.x = 250;
			imgCastle.y = 1;
			imgCastle.width = 40;
			imgCastle.height = 40;
			addChild(imgCastle);*/
			addEventListener(Event.ENTER_FRAME, enterFrameListener);
		}
		private function clickHandler(e:MouseEvent):void{
			this.x = -this.width+10;
			this.y = -this.height+10;
		}
		private function enterFrameListener(e:Event):void{
			lKingName.text = king.KingName;
			lKingId.text = king.KingId.toString();
			lRank.text = king.KingMilitaryRank.toString();
			lExperience.text = king.KingExperience.toString();
			lCastle.text = castle.CastleId.toString();
			for each(var r:Resource in king.Resources){
				switch(r.ResourceType){
					case 0:
						lCoal.text = r.ResourceCount.toString();
						break;
					case 1:
						lGold.text = r.ResourceCount.toString();
						break;
					case 2:
						lIron.text = r.ResourceCount.toString();
						break;
					case 3:
						lStone.text = r.ResourceCount.toString();
						break;
					case 4:
						lWood.text = r.ResourceCount.toString();
						break;					
				}
			}
		}
	}
}