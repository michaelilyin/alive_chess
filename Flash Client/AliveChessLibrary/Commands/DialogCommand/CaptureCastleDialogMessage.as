// Generated by the protocol buffer compiler.  DO NOT EDIT!

package AliveChessLibrary.Commands.DialogCommand {

  import com.google.protobuf.*;
  import flash.utils.*;
  import com.hurlant.math.BigInteger;
  public final class CaptureCastleDialogMessage extends Message {
    public function CaptureCastleDialogMessage() {
      registerField("type","",Descriptor.ENUM,Descriptor.LABEL_OPTIONAL,1);
      registerField("DisputeId","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,2);
    }
    // optional .AliveChessLibrary.Commands.DialogCommand.DialogState type = 1;
    public var type:Number = -1; //No default value for now...
    
    // optional int32 _disputeId = 2;
    public var DisputeId:int = 0;
    
  
  }
}