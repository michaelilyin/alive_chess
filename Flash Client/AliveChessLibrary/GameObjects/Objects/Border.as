package AliveChessLibrary.GameObjects.Objects
{
	import com.google.protobuf.*;
	import com.hurlant.math.BigInteger;
	import flash.utils.*;
	
	public class Border extends Message
	{
		public function Border()
		{
			registerField("BorderId","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,1);
			registerField("X","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,2);
			registerField("Y","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,3);
			registerField("WayCost","",Descriptor.FIXED32,Descriptor.LABEL_OPTIONAL,4);
		}
		public var BorderId:int = 0;
		
		public var X:int = 0;
		
		public var Y:int = 0;
		
		public var WayCost:int = 0;
	}
}