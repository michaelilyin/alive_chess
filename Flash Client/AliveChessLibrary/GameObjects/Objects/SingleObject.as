package AliveChessLibrary.GameObjects.Objects
{
	import com.google.protobuf.*;
	import com.hurlant.math.BigInteger;
	
	import flash.utils.*;
	
	public class SingleObject extends Message
	{
		public function SingleObject() {
			registerField("SingleObjectId","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,1);
			registerField("X","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,2);
			registerField("Y","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,3);
			registerField("SingleObjectType","AliveChessLibrary.GameObjects.Objects.SingleObjectType",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,4);
			registerField("WayCost","",Descriptor.FIXED32,Descriptor.LABEL_OPTIONAL,6);
			}
		public var SingleObjectId:int = 0;
		public var X:int = 0;
		public var Y:int = 0;
		public var SingleObjectType:Number = 0;
		public var WayCost:Number = 0;
	}
}