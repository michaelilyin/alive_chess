// Generated by the protocol buffer compiler.  DO NOT EDIT!

package AliveChessLibrary.Commands.BigMapCommand {

  import AliveChessLibrary.GameObjects.Resources.Resource;
  
  import com.google.protobuf.*;
  import com.hurlant.math.BigInteger;
  
  import flash.utils.*;
  public final class GetResourceMessage extends Message {
    public function GetResourceMessage() {
      registerField("resource","AliveChessLibrary.GameObjects.Resources.Resource",Descriptor.MESSAGE,Descriptor.LABEL_OPTIONAL,1);
      registerField("fromMine","",Descriptor.BOOL,Descriptor.LABEL_OPTIONAL,2);
    }
    // optional .AliveChessLibrary.Commands.BigMapCommand.Resource resource = 1;
    public var resource:AliveChessLibrary.GameObjects.Resources.Resource = null;
    
    // optional bool fromMine = 2;
    public var fromMine:Boolean = false;
    
  
  }
}