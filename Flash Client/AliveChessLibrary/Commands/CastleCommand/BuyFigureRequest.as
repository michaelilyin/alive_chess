// Generated by the protocol buffer compiler.  DO NOT EDIT!

package AliveChessLibrary.Commands.CastleCommand {

  import com.google.protobuf.*;
  import flash.utils.*;
  import com.hurlant.math.BigInteger;
  public final class BuyFigureRequest extends Message {
    public function BuyFigureRequest() {
      registerField("FigureCount","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,1);
      registerField("FigureType","",Descriptor.ENUM,Descriptor.LABEL_OPTIONAL,2);
    }
    // optional int32 _figureCount = 1;
    public var FigureCount:int = 0;
    
    // optional .AliveChessLibrary.Commands.CastleCommand.UnitType _figureType = 2;
    public var FigureType:Number = -1; //No default value for now...
    
  
  }
}