package ACEvents
{
	import flash.events.Event;
	
	public class ErrorMessageEvent extends Event
	{
		public static const ERROR:String = "ErrorMessage";//666
		public var message:String;
		
		public function ErrorMessageEvent(type:String, bubbles:Boolean=false, cancelable:Boolean=false)
		{
			super(type, bubbles, cancelable);
			this.message = type; 
		}
		public override function clone():Event 
		{ 
			return new ErrorMessageEvent(message, bubbles, cancelable); 
		} 
		public override function toString():String 
		{ 
			return formatToString("ErrorMessageEvent", "type", "bubbles", "cancelable", "eventPhase", "message"); 
		}
	}
}