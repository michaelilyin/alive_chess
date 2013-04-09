package vsu.alivechess.activities;

import vsu.alivechess.R;
import vsu.alivechess.net.Sender;
import vsu.alivechess.net.commands.AliveChessProtos.RegisterResponse;
import vsu.alivechess.net.executors.RegisterExecutor;
import vsu.alivechess.utils.AppHelper;
import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

public class RegisterAct extends Activity {
	Activity cont;
	TextView login;
	TextView pass;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.register);
        cont = this;
        
        login = (TextView) findViewById(R.id.reg_txt_login);
        pass = (TextView) findViewById(R.id.reg_txt_pass);
    }

    public void click_btn_register(final View view){
    	Sender.getInstance().sendRegister(login.getText().toString(), pass.getText().toString()
    			, new RegisterExecutor() {
			@Override
			public void execute() {
				RegisterResponse resp = (RegisterResponse) getResponce();
				if(resp.getIsSuccessed()){
					AppHelper.toast(cont, "Вы успешно зарегестрированы");
					finish();
				} else {
					AppHelper.toast(cont, "Вы не зарегестрированы");					
				}
			}
		});
    }
    
    public void click_btn_back(final View view){
    	finish();
    }
}
