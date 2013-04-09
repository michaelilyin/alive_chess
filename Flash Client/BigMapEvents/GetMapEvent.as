package BigMapEvents
{
	import flash.events.Event;
	
	public class GetMapEvent extends Event
	{
		public static const GET_MAP:String = "getMap"; 
		public var message:String;
		
		public function GetMapEvent(type:String = "GET_MAP", bubbles:Boolean=false, cancelable:Boolean=false)
		{
			super(type, bubbles, cancelable);
			this.message = type; 
		}
		public override function clone():Event 
		{ 
			return new GetMapEvent(message); 
		} 
		public override function toString():String 
		{ 
			return formatToString("GetMapEvent", "type", "bubbles", "cancelable", "eventPhase", "message"); 
		}
	}
}