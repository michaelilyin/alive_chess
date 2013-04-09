// Generated by the protocol buffer compiler.  DO NOT EDIT!

package AliveChessLibrary.Commands.BattleCommand {

  import com.google.protobuf.*;
  import flash.utils.*;
  import com.hurlant.math.BigInteger;
  
  public final class Battle extends Message {
    public function Battle() {
      registerField("id","",Descriptor.INT32,Descriptor.LABEL_OPTIONAL,1);
      registerField("Respondent","AliveChessLibrary.Commands.BattleCommand.King",Descriptor.MESSAGE,Descriptor.LABEL_OPTIONAL,2);
      registerField("youStep","",Descriptor.BOOL,Descriptor.LABEL_OPTIONAL,3);
      registerField("PlayerArmy","AliveChessLibrary.Commands.BattleCommand.Unit",Descriptor.MESSAGE,Descriptor.LABEL_REPEATED,4);
      registerField("OpponentArmy","AliveChessLibrary.Commands.BattleCommand.Unit",Descriptor.MESSAGE,Descriptor.LABEL_REPEATED,5);
    }
    // optional int32 id = 1;
    public var id:int = 0;
    
    // optional .AliveChessLibrary.Commands.BattleCommand.King _respondent = 2;
    public var Respondent:AliveChessLibrary.Commands.BattleCommand.King = null;
    
    // optional bool youStep = 3;
    public var youStep:Boolean = false;
    
    // repeated .AliveChessLibrary.Commands.BattleCommand.Unit _playerArmy = 4;
    public var PlayerArmy:Array = new Array();
    
    //fix bug 1 protobuf-actionscript3
    //dummy var using AliveChessLibrary.Commands.BattleCommand. necessary to avoid following exception
    //ReferenceError: Error #1065: Variable NetworkInfo is not defined.
    //at global/flash.utils::getDefinitionByName()
    //at com.google.protobuf::Message/readFromCodedStream()
    private var PlayerArmyDummy:AliveChessLibrary.Commands.BattleCommand.Unit = null;
    
    // repeated .AliveChessLibrary.Commands.BattleCommand.Unit _opponentArmy = 5;
    public var OpponentArmy:Array = new Array();
    
    //fix bug 1 protobuf-actionscript3
    //dummy var using AliveChessLibrary.Commands.BattleCommand. necessary to avoid following exception
    //ReferenceError: Error #1065: Variable NetworkInfo is not defined.
    //at global/flash.utils::getDefinitionByName()
    //at com.google.protobuf::Message/readFromCodedStream()
    private var OpponentArmyDummy:AliveChessLibrary.Commands.BattleCommand.Unit = null;
    
  
  }
}