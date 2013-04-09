package  Symbols{
	
	import flash.display.MovieClip;
	import fl.controls.TextArea;	
	import Net.Connect;
	
	public class connectionLog extends MovieClip {
		
		public var taC:TextArea;
		
		public function connectionLog() {
			taC=new TextArea();
			// constructor code
			taC.move(10,373);
			taC.setSize(300,25);
			//ta1.text=" ";
			addChild(taC);
			
		}
		public function outCon(msg:*):void{
			taC.text+=msg;
			}
	}
	
}
