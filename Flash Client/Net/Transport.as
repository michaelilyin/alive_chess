package Net
{
	
	import ACEvents.AuthorizeEvent.AuthorizeEvent;
	import ACEvents.BattleEvent.BattleEvent;
	import ACEvents.BigMapEvents.BigMapEvent;
	import ACEvents.ErrorMessageEvent;
	
	import GameMovieClip.Authorize;
	import GameMovieClip.BigMap;
	
	import Symbols.connectionLog;
	
	import com.google.protobuf.CodedInputStream;
	import com.google.protobuf.CodedOutputStream;
	import com.google.protobuf.Message;
	import com.hurlant.math.BigInteger;
	
	import fl.controls.Button;
	import fl.controls.TextArea;
	
	import flash.display.*;
	import flash.errors.IOError;
	import flash.events.*;
	import flash.geom.Transform;
	import flash.net.*;
	import flash.net.URLLoader;
	import flash.net.URLRequest;
	import flash.system.*;
	import flash.text.*;
	import flash.utils.*;
	
	import main.Main;
	
	
	public class Transport extends EventDispatcher
	{
		public static const RESPONSE_COMPLITE:String = "responseComplite";
		static private var codec:ProtoBufferCodec;
		private var conn:Connect;
		public var IdCmd:int;
		private var msg:Message;
		public var msgResponse:Message;
		static private var xmlCmd:XML;
		private var urlloader:URLLoader;
		private var load:Boolean = false;
		static private var _instance:Transport=null;
		static private var _allowCallConstructor:Boolean = false;

		static public function getInstance():Transport
		{
			if( _instance == null )
			{
				_allowCallConstructor = true;
				_instance = new Transport();
				_allowCallConstructor = false;
			}
			return _instance;
		}
		public function Transport()
		{
			if( !_allowCallConstructor )
				throw new Error("Use getInstance() method instead");
			else
			{
				LoadCmdXML();
				codec = ProtoBufferCodec.getInstance();
			}			
		}
		
		private function compliteListener(e:Event):void
		{
			xmlCmd = new XML (urlloader.data);			
			load = true;
		}
		public function SendCommand(m:Message):void
		{
			msg = m;
			var id:int = getId(msg);
			Send(EnCode(id,msg));
		}
		private function getId(msg:Message):int
		{
			var _id:int;
			var s:String="";
			var snew:String="";
			s = getQualifiedClassName(msg);
			var ind:int = s.indexOf("::")+2;
			snew = s.substring(ind,s.length);
			_id=xmlCmd.cmd.(name==snew).@id;
			return _id;
		}
		private function EnCode(id:int,msg:Message):ByteArray
		{
			return codec.Encode(id,msg);
		}
		public function Decode(buf:ByteArray):void
		{
			msgResponse = codec.Decode(buf,IdCmd);
			IdCmd = codec.getId();
			if (msgResponse!=null)
			{
				
				switch(IdCmd){
					case 1:
						dispatchEvent( new AuthorizeEvent(AuthorizeEvent.AUTHORIZE));
						break;
					case 5:
						dispatchEvent( new AuthorizeEvent(AuthorizeEvent.REGISTER));
						break;
					case 38:
						dispatchEvent(new BigMapEvent(BigMapEvent.GET_KING));
						break;
					case 7:
						dispatchEvent(new BigMapEvent(BigMapEvent.BIG_MAP));
						break;
					case 22:
						dispatchEvent(new BigMapEvent(BigMapEvent.GET_MAP));
						break;
					case 9:
						dispatchEvent(new BigMapEvent(BigMapEvent.CAPTURE_CASTLE));
						break;
					case 13:
						dispatchEvent(new BigMapEvent(BigMapEvent.COME_IN_CASTLE));
						break;
					case 15:
						dispatchEvent(new BigMapEvent(BigMapEvent.CONTACT_KING));
						break;
					case 18:
						dispatchEvent(new BigMapEvent(BigMapEvent.MOVE_KING));
						break;
					case 20:
						dispatchEvent(new BigMapEvent(BigMapEvent.CONTACT_CASTLE));
						break;
					case 32:
						dispatchEvent(new BigMapEvent(BigMapEvent.GET_GAME_STATE));
						break;
					case 34:
						dispatchEvent(new BigMapEvent(BigMapEvent.COMPUTE_PATH));
						break;
					case 36:
						dispatchEvent(new BigMapEvent(BigMapEvent.VERIFY_PATH));
						break;
          			case 24:
						dispatchEvent(new BigMapEvent(BigMapEvent.GET_OBJECT));
						break;
					case 28:
						dispatchEvent(new BigMapEvent(BigMapEvent.GET_RESOURCE));
						break;
					case 21:
						dispatchEvent(new BigMapEvent(BigMapEvent.CONTACT_CASTLE));
						break;
					case 28:
						dispatchEvent(new BigMapEvent(BigMapEvent.GET_RESOURCE));
						break;
					case 666:
						dispatchEvent(new ErrorMessageEvent(ErrorMessageEvent.ERROR));
						break;
					case 129:
						dispatchEvent(new BattleEvent(BattleEvent.DOWNLOAD_BATTLE));
						break;
					default:
						dispatchEvent(new Event(Transport.RESPONSE_COMPLITE));						
				}
			}
		}
		private function Send(b:ByteArray):void
		{
			conn = Connect.getInstance();
			conn.Send(b);
		}
		public function SConnect()
		{
			conn = Connect.getInstance();
			conn.Con();
		}
		private function LoadCmdXML():void
		{
			var urlRequest:URLRequest = new URLRequest("Commands.xml");
			urlloader = new URLLoader();
			urlloader.addEventListener(Event.COMPLETE,compliteListener);
			urlloader.load(urlRequest);
		}
	}
}