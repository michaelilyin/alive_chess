/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Util;


/**
 * @author Admin
 * Is an interface ONLY to messages that are RECEIVED by client
 */
public interface ICommand {
    /**
     * This method is used to initiate interaction between received
     * command and listener.
     * 
     * @param listener is an object, that implements
     * {@code IAuthorizationCommandListener},
     * {@code IBattleCommandListener},
     * {@code IBigMapCommandListener},
     * {@code ICastleCommandListener},
     * {@code IDialogCommandListener},
     * {@code IEmpireCommandListener}
     */
    public void Execute(Object listener);
}
