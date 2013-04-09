package ACEvents.BattleEvent
{
	import flash.events.Event;
	
	public class BattleEvent extends Event
	{
		public static const DOWNLOAD_BATTLE:String = "downloadBattle"; //129
		public var message:String;
		
		public function BattleEvent(type:String, bubbles:Boolean=false, cancelable:Boolean=false)
		{
			super(type, bubbles, cancelable);
			this.message = type; 
		}
		public override function clone():Event 
		{ 
			return new BattleEvent(message, bubbles, cancelable); 
		} 
		public override function toString():String 
		{ 
			return formatToString("BattleEvent", "type", "bubbles", "cancelable", "eventPhase", "message"); 
		}
	}
}