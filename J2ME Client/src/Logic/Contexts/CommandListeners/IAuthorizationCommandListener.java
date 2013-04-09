/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Logic.Contexts.CommandListeners;

import commands.Authorization.ExitFromGameResponse;
import commands.Authorization.AuthorizeResponse;
import commands.Authorization.RegisterResponse;
//import Commands.Authorization.*;

/**
 *
 * @author Admin
 */
public interface IAuthorizationCommandListener {
    public void AuthorizeResponseReceived(AuthorizeResponse c);
    public void ExitFromGameResponseReceived(ExitFromGameResponse c);
    public void RegisterResponseReceived(RegisterResponse c);
}
