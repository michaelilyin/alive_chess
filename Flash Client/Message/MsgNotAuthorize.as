package Message
{
	import fl.controls.Button;
	
	import flash.display.MovieClip;
	import flash.events.MouseEvent;
	
	public class MsgNotAuthorize extends MovieClip
	{
		public var btnReg:Button;
		public var  btnAgain:Button;
		private var p:MovieClip = null;
		
		public function MsgNotAuthorize(p:MovieClip)
		{
			super();
			this.p = p;
			btnAgain.addEventListener(MouseEvent.CLICK, clickListener);
		}
		private function clickListener(e:MouseEvent):void{
			parent.removeChild(this);
		}
	}
}