/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package vsu.alivechess.utils;

import android.app.Activity;
import android.content.Context;
import android.widget.Toast;

/**
 *
 * @author Al-mal
 */
public class AppHelper {
	private static Context cont;
	
	public AppHelper(Context context) {
		cont = context;
	}
	
	public static void toast(final Activity context, final String text){
		context.runOnUiThread(new Runnable() {			
			@Override
			public void run() {
				Toast.makeText(context, text, Toast.LENGTH_LONG).show();
			}
		});
	}
}
