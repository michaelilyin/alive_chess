package ACEvents.AuthorizeEvent
{
	import flash.events.Event;
	
	public class AuthorizeEvent extends Event
	{
		public static const AUTHORIZE:String = "authorize"; //1
		public static const REGISTER:String = "Register";//5
		
		public var message:String;
		
		public function AuthorizeEvent(type:String, bubbles:Boolean=false, cancelable:Boolean=false)
		{
			super(type, bubbles, cancelable);
			this.message = type; 
		}
		public override function clone():Event 
		{ 
			return new AuthorizeEvent(message, bubbles, cancelable); 
		} 
		public override function toString():String 
		{ 
			return formatToString("AuthorizeEvent", "type", "bubbles", "cancelable", "eventPhase", "message"); 
		}
	}
}