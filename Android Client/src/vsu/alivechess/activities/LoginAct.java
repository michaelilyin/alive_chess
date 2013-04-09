package vsu.alivechess.activities;

import vsu.alivechess.R;
import vsu.alivechess.net.Receiver;
import vsu.alivechess.net.Sender;
import vsu.alivechess.net.commands.AliveChessProtos.AuthorizeResponse;
import vsu.alivechess.net.commands.AliveChessProtos.ErrorMessage;
import vsu.alivechess.net.commands.AliveChessProtos.GetGameStateResponse;
import vsu.alivechess.net.commands.AliveChessProtos.GetMapResponse;
import vsu.alivechess.net.executors.AuthorizeExecutor;
import vsu.alivechess.net.executors.ErrorExecutor;
import vsu.alivechess.net.executors.GetGameStateExecutor;
import vsu.alivechess.net.executors.GetMapExecutor;
import vsu.alivechess.utils.AppHelper;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;

public class LoginAct extends Activity {
	private LoginAct cont;
	
	EditText txtLogin;
	EditText txtPass;
	
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.login);
        cont = this;
        
        txtLogin = (EditText) findViewById(R.id.login_txt_login);
        txtPass = (EditText) findViewById(R.id.login_txt_pass);
        
        Receiver.getInstance().addExecutor(new ErrorExecutor() {

            public void execute() {
                ErrorMessage eMsg = (ErrorMessage) getResponce();
                AppHelper.toast(cont, eMsg.getMessage());
            }
        });
    }
    
    public void click_btn_login(final View view){
    	String login = txtLogin.getText().toString();
    	String pass = txtPass.getText().toString();
        Sender.getInstance().sendAuthorize(login, pass, new AuthorizeExecutor() {
        	
        	@Override
            public void execute() {
                AuthorizeResponse aResp = (AuthorizeResponse) this.getResponce();
                
                startActivity(new Intent(cont, BigMapAct.class));
                finish();

            }
        });        	
    	
    }
    
    public void click_btn_register(final View view){
    	startActivity(new Intent(this, RegisterAct.class));
    }
    
    public void click_btn_back(final View view){
    	finish();
    }
}
