// Generated by the protocol buffer compiler.  DO NOT EDIT!

package AliveChessLibrary.Commands.BigMapCommand {

  import com.google.protobuf.*;
  import flash.utils.*;
  import com.hurlant.math.BigInteger;
  public final class SingleObject extends Message {
    public function SingleObject() {
      registerField("SingleObjectId","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,1);
      registerField("X","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,2);
      registerField("Y","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,3);
      registerField("SingleObjectType","",Descriptor.ENUM,Descriptor.LABEL_OPTIONAL,4);
      registerField("WayCost","",Descriptor.FIXED32,Descriptor.LABEL_OPTIONAL,5);
    }
    // optional int32 _singleObjectId = 1;
    public var SingleObjectId:int = 0;
    
    // optional int32 _x = 2;
    public var X:int = 0;
    
    // optional int32 _y = 3;
    public var Y:int = 0;
    
    // optional .AliveChessLibrary.Commands.BigMapCommand.SingleObjectType _singleObjectType = 4;
    public var SingleObjectType:Number = -1; //No default value for now...
    
    // optional fixed32 _wayCost = 5;
    public var WayCost:int = 0;
    
  
  }
}