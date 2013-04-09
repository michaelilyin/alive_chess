/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Authorization;

import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IAuthorizationCommandListener;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class AuthorizeResponse implements IProtoDeserializable, ICommand {
    private boolean isNewPlayer;          //1 protomember
    private boolean isAuthorized;         //2 protomember
    private String errorMessage;          //3 protomember

    public AuthorizeResponse(){}

    public boolean getIsNewPlayer(){
        return isNewPlayer;
    }

    public boolean getIsAuthorized(){
        return isAuthorized;
    }

    public String getErrorMessage(){
        return errorMessage;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        isNewPlayer = dsr.readBool(1);
        isAuthorized = dsr.readBool(2);
        try{
            errorMessage = dsr.readString(3);
        }catch(IOException e){
            if (e.getMessage().compareTo("The wrong field number")==0){
                errorMessage = null;
            }else throw e;
        }
    }

    public void Execute(Object listener) {
        IAuthorizationCommandListener l = (IAuthorizationCommandListener)listener;
        l.AuthorizeResponseReceived(this);
    }



}
