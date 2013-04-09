/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui;

import javax.microedition.lcdui.*;

/**
 *
 * @author OLEG
 */
public class SettingForm extends Form {

    private TextField textFieldLogin;
    private TextField textFieldPassword;

    public SettingForm() {
        super("Alive-Chess User Setting");
        textFieldLogin = new TextField("Login", "Slevin_Kelevra", 999, 0);
        textFieldPassword = new TextField("Password", "1234", 999, 0);
        this.append(textFieldLogin);
        this.append(textFieldPassword);
    }

    public String getPassword() {
        return textFieldPassword.getString();
    }

    public String getLogin() {
        return textFieldLogin.getString();
    }
}
