package Net
{
	import AliveChessLibrary.Commands.RegisterCommands.*;
	
	import GameMovieClip.Authorize;
	
	import Net.Transport;
	
	import Symbols.connectionLog;
	
	import com.google.protobuf.CodedInputStream;
	import com.google.protobuf.CodedOutputStream;
	import com.google.protobuf.Message;
	import com.hurlant.math.BigInteger;
	import com.hurlant.util.der.Sequence;
	
	import fl.controls.Button;
	import fl.controls.Label;
	import fl.controls.TextArea;
	
	import flash.display.*;
	import flash.errors.IOError;
	import flash.events.*;
	import flash.net.*;
	import flash.system.*;
	import flash.text.*;
	import flash.utils.*;
	
	import google.protobuf.FieldDescriptorProto.Label;
	
	import main.Main;

	public class Connect extends MovieClip
	{
		
		public static const EOT:int = 4;
		private  var mv:MovieClip=new MovieClip();
		private  var statusField:TextField;
		private  var socket:Socket;
		private  var buffer:ByteArray;
		private  var loader:Loader;
		private  var id:int;
		private  var ba:ByteArray=new ByteArray();
		private  var ba2:ByteArray=new ByteArray();
		private  var taCon:connectionLog;
		private  var m:Main;
		private var btn:Button;
		private var login:String;
		private var pas:String;
		private var type:Boolean;
		private  var _connect:Boolean = false;
		private  var bufSend:ByteArray;
		private static var __instance:Connect;
		private static var __allowInstantiation:Boolean = false;
		public var flag:Boolean = false;
		
		static public function getInstance():Connect
		{
			if(!__instance)
			{
				// Разрешаем создание экземпляра класса.
				__allowInstantiation = true;
				// Создаем экземпляр.
				__instance = new Connect();
				// Запрещаем создание экземпляров.
				__allowInstantiation = false;
			}
			return __instance;
		
		}
		public function Connect()
		{
			if(!__allowInstantiation)
				throw new Error("Вы не можете создавать экземпляры класса при помощи конструктора. Для доступа к экземпляру используйте Singleton.instance.");
		}
		/*private function onData(event:DataEvent):void
		{
			trace("[" + event.type + "] " + event.data);
		}*/
		public function Con()
		{			
			socket = new Socket();
			socket.addEventListener(Event.CONNECT, connectListener);
			socket.addEventListener(Event.CLOSE, closeListener);
			socket.addEventListener(ProgressEvent.SOCKET_DATA, 
				socketDataListener);
			socket.addEventListener(IOErrorEvent.IO_ERROR, ioErrorListener);
			try {
				Security.loadPolicyFile("xmlsocket://localhost:11000");							
				socket.connect("localhost", 22000);				
			} catch (e:Error) {
				trace(e.message);
				trace("Connection problem!");
				//out(e.message);
			}
		}
		 public function isConnect():Boolean{
			return _connect;
		}
		 public function Send(buf:ByteArray):void
		{
			 
			 socket.writeBytes(buf);
			 
			 socket.flush();
		}

		 private function connectListener (e:Event):void {
			
			if (socket.connected)
			{
				trace("Connected! Waiting for data...");
				
			}
			/*var swfLoader:Loader = new Loader();
			swfLoader.load(new URLRequest("Game.swf"));
			while (m.numChildren>0)
			{
				m.removeChildAt(0);
			}
			m.addChild(swfLoader);*/
			/*game=new Game();
			m.addChild(game);*/
		}
		 private function socketDataListener (e:ProgressEvent):void {
			buffer = new ByteArray();
			out("New socket data arrived.");			
			socket.readBytes(buffer, buffer.length, socket.bytesAvailable);
			var tr:Transport=Transport.getInstance();
			var s:String="";
			for(var i:int = 0;i <= buffer.length-1; i++){
				s+=buffer[i].toString()+" ";
			}
			//trace("Accept: "+s+"\n");
			buffer.position = 0;
			tr.Decode(buffer);
			buffer.clear();
			
		}
		 private function closeListener (e:Event):void {
			_connect = false;
			buffer.position = buffer.length - 1;
			var lastByte:int = buffer.readUnsignedByte();
			if (lastByte != Connect.EOT) {
				return;
			}
			buffer.length = buffer.length - 1;
			loader = new Loader();
			loader.loadBytes(buffer);
			loader.contentLoaderInfo.addEventListener(Event.INIT, 
				assetInitListener);
		}
		 private function assetInitListener (e:Event):void {
			addChild(loader.content);
			out("Asset initialzed.");
		}
		 private function ioErrorListener (e:IOErrorEvent):void {
			out("I/O Error: " + e.text);
		}
		 private function out (msg:String):void {
			//trace(msg);
		}
	}
}