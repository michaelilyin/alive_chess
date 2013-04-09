package vsu.alivechess.activities;

import vsu.alivechess.R;
import vsu.alivechess.net.Sender;
import vsu.alivechess.net.executors.LeaveCastleExecutor;
import android.app.Activity;
import android.content.Intent;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Bundle;
import android.widget.TextView;

public class CastleAct extends Activity {
	CastleAct cont;
	TextView txt;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        cont = this;
        setContentView(R.layout.castle);
        
        txt = (TextView) findViewById(R.id.castle_txt);
        
        Intent intent = getIntent();
        int castleId = intent.getExtras().getInt("castleId");
        txt.setText(""+castleId);
    }

    @Override
    public void finish() {
    	Sender.getInstance().sendLeaveCastle(new LeaveCastleExecutor() {			
			@Override
			public void execute() {
				runOnUiThread(new Runnable() {
					@Override
					public void run() {
						cont.closeAct();
					}
				});
			}
		});
    }
    
    public void closeAct(){
    	super.finish();
    }
}
