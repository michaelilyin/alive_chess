/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Logic.Contexts.CommandListeners;

import commands.Error.ErrorMessage;

/**
 *
 * @author Admin
 */
public interface IErrorCommandListener {
    public void ErrorMessReceived(ErrorMessage c);
}
