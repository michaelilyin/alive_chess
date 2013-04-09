/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package vsu.alivechess.net;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;

import vsu.alivechess.utils.ByteHelper;


/**
 *
 * @author Al-mal
 */
public class ClientApp {
	private static ClientApp instance;
	
    private boolean close = false;
    private Socket socket;
    private DataInputStream in;
    private DataOutputStream out;

    public ClientApp(String ip, ErrorListener lError) throws UnknownHostException, IOException{
    	instance = this;
		socket = new Socket(ip, 22000);

        in  = new DataInputStream(socket.getInputStream());
        out = new DataOutputStream(socket.getOutputStream());

        new Thread(new SenderRunnable(lError)).start();
        new Thread(new ReceiverRunnable(lError)).start();
    }

    public void send(byte[] msg) throws IOException{
        out.write(msg);
    }

    public void close() throws IOException{
        if(socket.isConnected()){
            close = true;
            socket.close();
            out.close();
            in.close();            
        }
    }

    public boolean isClose() {
        return close;
    }

    public boolean isConnect(){
    	return socket.isConnected();
    }

    private class SenderRunnable implements Runnable {
        private ErrorListener lError;

        public SenderRunnable(ErrorListener lError){
            this.lError = lError;
        }

        public void run() {
            while (!isClose()) {
                try {
                    Thread.sleep(10);
                    byte[] msg = Sender.getInstance().getMessage();
                    if(msg != null)
                        send(msg);
                } catch (IOException ex) {
                    lError.error(ex.getMessage());
                } catch (InterruptedException ex) {
                    lError.error(ex.getMessage());
                }

            }
        }
    }

    private class ReceiverRunnable implements Runnable {
        private ErrorListener lError;

        public ReceiverRunnable(ErrorListener lError){
            this.lError = lError;
        }

        public void run() {
            while (!isClose()) {
                try {
                    //int count = in.available();
                    byte[] msg = new byte[10240];
                    int countBytes = in.read(msg);
                    if (countBytes > 0) {
                    	msg = ByteHelper.getPartByteArray(msg, 0, countBytes);
                        Receiver.getInstance().receiveMessage(msg);
                    }
                    if(countBytes == -1)
                    	break;
                } catch (IOException ex) {
                    lError.error(ex.getMessage());
                }  catch (ClassNotFoundException ex) {
                    lError.error(ex.getMessage());
                }
            }
        }
    }
    
    public static ClientApp getInstance(){
    	return instance;
    }

}
