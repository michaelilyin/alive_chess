/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Decoders;

import commands.Authorization.ExitFromGameResponse;
import commands.Authorization.AuthorizeResponse;
import commands.Authorization.RegisterResponse;
//import Commands.Authorization.*;
import commands.Util.Commands;

/**
 *
 * @author Admin
 */
public class AuthorizationDecoder extends DefaultDecoder {
    private static AuthorizationDecoder instance;
    
    private AuthorizationDecoder(){}

    public static AuthorizationDecoder getInstance(){
        if (instance == null) instance = new AuthorizationDecoder();
        return instance;
    }

    public void recognizeCommand(int c) {
        switch (c){
            case Commands.AUTHORISE_RESPONSE: command = new AuthorizeResponse(); break;
            case Commands.EXIT_FROM_GAME_RESPONSE: command = new ExitFromGameResponse(); break;
            case Commands.REGISTER_RESPONSE: command = new RegisterResponse(); break;
        }
    }
}
