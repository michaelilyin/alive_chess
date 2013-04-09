package vsu.alivechess.activities;

import java.io.IOException;
import java.net.UnknownHostException;

import vsu.alivechess.R;
import vsu.alivechess.R.layout;
import vsu.alivechess.net.ClientApp;
import vsu.alivechess.net.ErrorListener;
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
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Toast;

public class StartAct extends Activity {
	StartAct cont;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        cont = this;
        setContentView(R.layout.main);

        new AppHelper(this);

    }

    public void click_btn_enter(final View view){
        try {
            //String ip = "5.168.246.181";
            String ip = "192.168.2.1";
            
            new ClientApp(ip, new ErrorListener() {

                @Override
                public void error(final String str) {
                	runOnUiThread(new Runnable() {
						@Override
						public void run() {
							Toast.makeText(cont, str, Toast.LENGTH_LONG).show();
						}
					});
                }
            });
            
            if(ClientApp.getInstance().isConnect()){
            	startActivity(new Intent(this, LoginAct.class));
            }
           
        } catch (UnknownHostException ex) {
        	error(ex.getMessage());	
        } catch (IOException ex) {
        	error(ex.getMessage());
        }
    }
    
    public void click_btn_exit(final View view){
    	finish();
    }
    
    private void error(String str){
		Toast.makeText(this, str, Toast.LENGTH_LONG).show();    	
    }
}