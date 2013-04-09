package GameMovieClip
{
	import ACEvents.AuthorizeEvent.AuthorizeEvent;
	
	import AliveChessLibrary.Commands.RegisterCommands.AuthorizeRequest;
	import AliveChessLibrary.Commands.RegisterCommands.AuthorizeResponse;
	
	import Net.ProtoBufferCodec;
	import Net.Transport;
	
	import Symbols.LoadGame;
	import Symbols.connectionLog;
	import Symbols.sMainMenu;
	
	import com.google.protobuf.Message;
	
	import fl.controls.Button;
	import fl.controls.CheckBox;
	import fl.controls.ComboBox;
	import fl.controls.Label;
	import fl.controls.TextArea;
	import fl.controls.TextInput;
	import fl.events.ComponentEvent;
	
	import flash.display.*;
	import flash.display.Shape;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.geom.Rectangle;
	import flash.text.TextFormat;
	import flash.utils.ByteArray;
	
	import google.protobuf.FieldDescriptorProto.Label;
	
	import main.Main;
	
	public class Authorize extends MovieClip
	{
		//private var con:Connect;
		private var txtLogin:TextInput;
		private var txtPas:TextInput;
		private var cb:CheckBox;
		private var lAuthorize:Label;
		private var lLogin:Label;
		private var lPas:Label;
		private var lType:Label;
		private var btn:Button;
		private var btnConnect:Button;
		private var btnMenu:Button;
		private var tf:TextFormat=new TextFormat();
		var lPlayer:Label = new Label();
		private var type:Boolean = true;
		private var transport:Transport;
		private var ar:AuthorizeResponse;
		private var ma:Main = null;
		private var sMenu:sMainMenu = new sMainMenu();
		
		public function Authorize(m:Main)
		{
			stop();
			this.ma = m;
			transport = Transport.getInstance();
			transport.addEventListener(AuthorizeEvent.AUTHORIZE,msgExecute);
			var t:title1=new title1();
			var r:rect=new rect();
			t.x = 5;
			r.x = 160;
			t.y = 10;
			r.y = 90;
			addChild(t);
			addChild(r);
			createUI();
			setUpHandlers();
		}
		private function msgExecute(e:Event):void
		{
			try{
				ar = AuthorizeResponse (transport.msgResponse);
				trace (ar.IsAuthorized.toString()+" "+ar.IsNewPlayer.toString()+ar.ErrorMessage);
				if(ar.IsAuthorized){
					trace("Можно загружать игру");
					ma.removeChild(this);
					gotoAndStop(3);
					var g:Game = new Game(ma);
					removeChild(sMenu);
				}
				removeEventListener(AuthorizeEvent.AUTHORIZE,msgExecute);
			}
			catch(er:Error){
				trace("No good");
			}
		}
		public function Destroy():void
		{
			stage.removeChild(this);
		}
		
		private function createUI():void
		{
			tf.size = 18;
			tf.font = "Times New Roman";
			bldLabels();
			bldTxtInput();
			bldCB();
			bldBtn();
			bldMenu();
		}
		public function bldMenu():void{
			btnMenu = new Button();
			btnMenu.label = "Menu";
			btnMenu.selected = true;
			btnMenu.setSize(70,22);
			btnMenu.move(330,0);
			addChild(btnMenu);
			btnMenu.addEventListener(MouseEvent.CLICK,menuClick):
		}
		private function menuClick(e:MouseEvent):void{
			ma.MainMenu();
		}
		private function bldLabels():void
		{
			lAuthorize=new Label();
			lAuthorize.setSize(100,30);
			lAuthorize.text = "Authorize";
			lAuthorize.move(165,100);
			lAuthorize.setStyle("textFormat",tf);
			addChild(lAuthorize);
			lLogin=new Label();
			lLogin.setSize(100,30);
			lLogin.text = "Login";
			lLogin.move(165,135);
			lLogin.setStyle("textFormat",tf);
			addChild(lLogin);
			lPas=new Label();
			lPas.setSize(100,30);
			lPas.text = "Password";
			lPas.move(165,170);
			lPas.setStyle("textFormat",tf);
			addChild(lPas);
			lType=new Label();
			lType.setSize(100,30);
			lType.text = "Type";
			lType.move(165,210);
			lType.setStyle("textFormat",tf);
			addChild(lType);
		}
		private function bldTxtInput():void
		{
			txtLogin=new TextInput();
			txtLogin.setSize(100,20);
			txtLogin.move(275,135);
			txtLogin.text = "player";
			addChild(txtLogin);
			txtPas=new TextInput();
			txtPas.setSize(100,20);
			txtPas.move(275,170);
			txtPas.text = "pw";
			txtPas.displayAsPassword = true;
			txtPas.editable = true;
			addChild(txtPas);
		}
		private function bldCB():void
		{
			cb=new CheckBox();
			cb.setSize(100,20);
			cb.selected = true;
			cb.label = "Super User";
			cb.move(270,210);
			addChild(cb);
		}
		private function bldBtn():void
		{
			btnConnect = new Button();
			btnConnect.label = "Connect";
			btnConnect.selected = true;
			btnConnect.setSize(100,22);
			btnConnect.move(165,245);
			addChild(btnConnect);
			
			btn = new Button();
			btn.label = "Ok";
			btn.selected = true;
			btn.setSize(100,22);
			btn.move(275,245);
			btn.enabled = false;
			addChild(btn);
			
		}
		private function setUpHandlers():void
		{
			btnConnect.addEventListener(MouseEvent.CLICK,conHandler);
			btn.addEventListener(MouseEvent.CLICK,clickHandler);
			cb.addEventListener(MouseEvent.CLICK,cbHandler);
		}
		private function conHandler(e:MouseEvent):void
		{
			transport.SConnect();
			removeChild(btnConnect);			
			btn.enabled = true;
		}
		private function cbHandler(e:MouseEvent):void
		{
			type = e.target.selected;
		}
		private function clickHandler(e:MouseEvent):void
		{
			
			trace("Login: " + txtLogin.text + ", Password: " + txtPas.text + ", Type: " + type.toString());
			var c:connectionLog=new connectionLog();
			var ar:AuthorizeRequest = new AuthorizeRequest();
			ar.login = txtLogin.text;
			ar.password = txtPas.text;			
			transport.SendCommand(ar);			
		}
	}

}