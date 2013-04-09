package ACEvents.BigMapEvents
{
	import flash.events.Event;
	
	public class BigMapEvent extends Event
	{
		public static const BIG_MAP:String = "bigMap"; //7
		public static const GET_MAP:String = "getMap"; //22
		public static const GET_KING:String = "getKing"; //38
		public static const	CAPTURE_CASTLE:String="captureCastle";// 9
		public static const	COME_IN_CASTLE:String="comeInCastle";// 13
		public static const	CONTACT_KING:String="contactKing"; // 15
		public static const	MOVE_KING:String="moveKing;" // 18
		public static const	CONTACT_CASTLE:String="contactCastle"; // 20			 
		public static const	GET_GAME_STATE:String="getGameState"; // 32
		public static const	COMPUTE_PATH:String="computePath"; // 34
		public static const	VERIFY_PATH:String="verifyPath"; // 36
		public static const	GET_OBJECT:String="GetObjects"; // 24
		public static const	GET_RESOURCE:String="GetResourceM"; // 28

		public var message:String;
		
		public function BigMapEvent(type:String, bubbles:Boolean=false, cancelable:Boolean=false)
		{
			super(type, bubbles, cancelable);
			this.message = type; 
		}
		public override function clone():Event 
		{ 
			return new BigMapEvent(message, bubbles, cancelable); 
		} 
		public override function toString():String 
		{ 
			return formatToString("BigMapEvent", "type", "bubbles", "cancelable", "eventPhase", "message"); 
		}
	}
}