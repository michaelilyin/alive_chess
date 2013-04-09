package Net
{
	import AliveChessLibrary.Commands.BattleCommand.DownloadBattlefildResponse;
	import AliveChessLibrary.Commands.BigMapCommand.BigMapResponse;
	import AliveChessLibrary.Commands.BigMapCommand.CaptureCastleResponse;
	import AliveChessLibrary.Commands.BigMapCommand.ComeInCastleResponse;
	import AliveChessLibrary.Commands.BigMapCommand.ContactCastleResponse;
	import AliveChessLibrary.Commands.BigMapCommand.GetGameStateResponse;
	import AliveChessLibrary.Commands.BigMapCommand.GetKingResponse;
	import AliveChessLibrary.Commands.BigMapCommand.GetMapResponse;
	import AliveChessLibrary.Commands.BigMapCommand.GetObjectsResponse;
	import AliveChessLibrary.Commands.BigMapCommand.GetResourceMessage;
	import AliveChessLibrary.Commands.BigMapCommand.MoveKingResponse;
	import AliveChessLibrary.Commands.ErrorCommand.ErrorMessage;
	import AliveChessLibrary.Commands.RegisterCommands.AuthorizeResponse;
	import AliveChessLibrary.Commands.RegisterCommands.ExitFromGameResponse;
	import AliveChessLibrary.Commands.RegisterCommands.RegisterResponse;
	
	import com.google.protobuf.CodedInputStream;
	import com.google.protobuf.CodedOutputStream;
	import com.google.protobuf.Message;
	import com.hurlant.math.BigInteger;
	
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.utils.ByteArray;
	import flash.utils.IDataInput;
	import flash.utils.IDataOutput;
	
	public class ProtoBufferCodec extends EventDispatcher
	{
		public static const BIGMAPRESPONSE_COMPLITE:String = "responseComplite";
		public static const AUTHORIZERESPONSE_COMPLITE:String = "authorizeresponseComplite";
		private var id:int;
		static private var _instance:ProtoBufferCodec=null;
		static private var _allowCallConstructor:Boolean = false;
		
		static public function getInstance():ProtoBufferCodec
		{
			if( _instance == null )
			{
				_allowCallConstructor = true;
				_instance = new ProtoBufferCodec();
				_allowCallConstructor = false;
			}
			return _instance;
		}
		public function ProtoBufferCodec()
		{
			if( !_allowCallConstructor )
				throw new Error("Use getInstance() method instead");			
		}
		public function Encode(id:int,msg:Message):ByteArray
		{
			var ba:ByteArray = new  ByteArray();
			var ido:IDataOutput = ba;
			var cos:CodedOutputStream = new CodedOutputStream(ido);
			msg.writeToCodedStream(cos);		
			var b2:ByteArray=new ByteArray();
			//b2.writeInt(id);
			var bas:ByteArray = new ByteArray();
			bas.writeInt(id);
			for(var i:int = bas.length - 1;i >= 0; i--){
				b2.writeByte(bas[i]);
			}	
			var bas1:ByteArray = new ByteArray();
			//b2.writeInt(ba.length);
			bas1.writeInt(ba.length);
			for(var i:int = bas1.length - 1; i >= 0; i--){
				b2.writeByte(bas1[i]);
			}
			b2.writeBytes(ba,0,ba.length);
			var s:String="";
			for(var i:int = 0; i < b2.length; i++)
			{
				s+=b2[i].toString()+" ";
			}
			//trace("send "+s+"\n");
			b2.position=0;
			return b2;
		}
		public function getId():int
		{
			return id;
		}
		public function Decode(buf:ByteArray,_id:int):Message
		{
			var msg:Message = null; 
			if(buf!=null)
			{
				var j:int = 3;
				var idArray:ByteArray = new ByteArray();
				for(var i:int = 0 ; i<4; i++)
				{
					idArray.writeByte(buf[j]);
					j--;				
				}
				var sizeArray:ByteArray = new ByteArray();
				j=7;
				for(var i:int = 0 ; i<4; i++)
				{
					sizeArray.writeByte(buf[j]);
					j--;				
				}
				idArray.position = 0;
				sizeArray.position = 0;
				id = idArray.readInt();
				var size:int = sizeArray.readInt();
				buf.position = 8;
				
				switch (id){
					case 1:
						msg = new AuthorizeResponse();
					//	dispatchEvent(new Event(ProtoBufferCodec.AUTHORIZERESPONSE_COMPLITE));
						break;
					case 5:
						msg = new RegisterResponse();
						break;
					case 3:
						msg = new ExitFromGameResponse();
						break;
					case 7:
						msg = new BigMapResponse();
					//	dispatchEvent(new Event(ProtoBufferCodec.BIGMAPRESPONSE_COMPLITE));
						break;
					case 9:
						msg= new CaptureCastleResponse();
						break;
					case 22:
						msg = new GetMapResponse();
						break;
					case 38:
						msg = new GetKingResponse();
						break;
					case 32:
						msg = new GetGameStateResponse();
						break;
					case 24:
						msg = new GetObjectsResponse();
						break;
					case 18:
						msg = new MoveKingResponse();
						break;
					case 28:
						msg = new GetResourceMessage();
						break;
					case 13:
						msg = new ComeInCastleResponse();
						break;
					case 21:
						msg = new ContactCastleResponse()
						break;
					case 28:
						msg = new GetResourceMessage()
						break;
					case 666:
						msg = new ErrorMessage();
						break;
					case 129:
						msg = new DownloadBattlefildResponse();
						break;
					default:
						msg = null;
						id = -1;
				}
				var idi:IDataInput = buf;
				var cis:CodedInputStream = new CodedInputStream(idi);	
				msg.readFromCodedStream(cis);
			}
			else {
				id = -1;
			}
			return msg;			
			
			/*	if(id == 1){
					msg = new AuthorizeResponse();
					dispatchEvent(new Event(ProtoBufferCodec.AUTHORIZERESPONSE_COMPLITE));					
				}
				if(id == 5)
				{
					msg = new RegisterResponse();
				}
				if(id == 3)
				{
					msg = new ExitFromGameResponse();
				}
				if(id==7)
				{
					msg = new BigMapResponse();
					dispatchEvent(new Event(ProtoBufferCodec.BIGMAPRESPONSE_COMPLITE));
				}
				if(id==9)
				{
					msg= new CaptureCastleResponse();
				}
				if(id==22)
				{
					msg = new GetMapResponse();
				}
				if(id==38)
				{
					msg = new GetKingResponse();
				}
				var idi:IDataInput = buf;
				var cis:CodedInputStream = new CodedInputStream(idi);	
				msg.readFromCodedStream(cis);
			}
			else {id=-1;}
			return msg;*/
		}
		public function getConcreteCommand(_id:int):Message
		{
			var msg:Message;	
			if(_id==1){
				msg = new AuthorizeResponse();
			}
			return msg;
		}
	}
}