// Generated by the protocol buffer compiler.  DO NOT EDIT!

package AliveChessLibrary.Commands.CastleCommand {

  import com.google.protobuf.*;
  import flash.utils.*;
  import com.hurlant.math.BigInteger;
  public final class Unit extends Message {
    public function Unit() {
      registerField("UnitId","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,1);
      registerField("UnitType","",Descriptor.ENUM,Descriptor.LABEL_OPTIONAL,2);
      registerField("UnitCount","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,3);
    }
    // optional int32 _unitId = 1;
    public var UnitId:int = 0;
    
    // optional .AliveChessLibrary.Commands.CastleCommand.UnitType _unitType = 2;
    public var UnitType:Number = -1; //No default value for now...
    
    // optional int32 _unitCount = 3;
    public var UnitCount:int = 0;
    
  
  }
}