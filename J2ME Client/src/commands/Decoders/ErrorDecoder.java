/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Decoders;

import commands.Error.ErrorMessage;
import commands.Util.Commands;

/**
 *
 * @author Admin
 */
public class ErrorDecoder extends DefaultDecoder {
    private static ErrorDecoder instance;

    private ErrorDecoder(){}

    public static ErrorDecoder getInstance(){
        if (instance == null) instance = new ErrorDecoder();
        return instance;
    }

    public void recognizeCommand(int c) {
        switch (c){
            case Commands.ERROR_MESSAGE: command = new ErrorMessage(); break;
        }
    }


}
