/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package net;

import commands.Decoders.AuthorizationDecoder;
import commands.Decoders.BattleDecoder;
import commands.Decoders.BigMapDecoder;
import commands.Decoders.CastleDecoder;
import commands.Decoders.ChatDecoder;
import commands.Decoders.DefaultDecoder;
import commands.Decoders.DialogDecoder;
import commands.Decoders.EmpireDecoder;
import commands.Decoders.ErrorDecoder;
import commands.Decoders.StatisticDecoder;
import commands.Util.ICommand;
import Logic.Contexts.GameContext;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import javax.microedition.io.Connector;
import javax.microedition.io.SocketConnection;

/**
 *
 * @author Admin
 */

//3 потока:
//executor
//receiver
//sender
public class SocketTransport {

    public static final int AUTHORIZATION_LIMIT = 5;
    public static final int BIG_MAP_LIMIT = 38;
    public static final int DIALOG_LIMIT = 66;
    public static final int CASTLE_LIMIT = 89;
    public static final int BATTLE_LIMIT = 131;
    public static final int EMPIRE_LIMIT = 180;
    public static final int STATISTIC_LIMIT = 209;
    public static final int CHAT_LIMIT = 253;
    public static final int ERROR_LIMIT = 666;
    
    private InputStream is;
    private OutputStream os;
    private SocketConnection con;
    private DefaultDecoder dcdr;
    private Queue received;
    private Queue to_send;
    private GameContext gameContext;
    private byte[] buffer;
    private boolean shutdown;

    public SocketTransport(){ }

    private void setDecoder(int d){
        if (d <= AUTHORIZATION_LIMIT){
            dcdr = AuthorizationDecoder.getInstance(); return;
        }
        if (d <= BIG_MAP_LIMIT){
            dcdr = BigMapDecoder.getInstance(); return;
        }
        if (d <= DIALOG_LIMIT){
            dcdr = DialogDecoder.getInstance(); return;
        }
        if (d <= CASTLE_LIMIT){
            dcdr = CastleDecoder.getInstance(); return;
        }
        if (d <= BATTLE_LIMIT){
            dcdr = BattleDecoder.getInstance(); return;
        }
        if (d <= EMPIRE_LIMIT){
            dcdr = EmpireDecoder.getInstance(); return;
        }
        if (d <= STATISTIC_LIMIT){
            dcdr = StatisticDecoder.getInstance(); return;
        }
        if (d <= CHAT_LIMIT){
            dcdr = null; return;
        }
        if (d <= ERROR_LIMIT){
            dcdr = ErrorDecoder.getInstance(); return;
        }
    }

    public boolean Connect(String hostname, int port){
        try{
            con = (SocketConnection) Connector.open("socket://" + hostname + ":" + port);
            is = con.openInputStream();
            os = con.openOutputStream();
            shutdown = false;
        } catch (IOException exc){
            System.out.println("Unable to connect : " + exc.getMessage());
            shutdown = true;
        }
        return shutdown;
    }

    public void Close() throws IOException {
        shutdown = true;
        is.close();
        os.close();
        con.close();
    }

    public boolean isShutdown() {
        return shutdown;
    }

    public void Send(IProtoSerializableRequest command){
        to_send.put(command);
    }
    /**
     * Посылает массив байт в сокет
     * @param buf Массив для посылки
     */
    private void send(byte[] buf) throws IOException {
        System.out.println("SOCKET: buf to write:");
        for (int i = 0; i < buf.length; i++) {
            System.out.print(buf[i] + " ");
        }
        try {
            os.write(buf);
        } catch (Exception exc) {
            System.out.println("что-то при записи: " + exc.getMessage());
        }
        os.flush();
    }

    private class Receiver implements Runnable{

        public void run() {
            try{
                while (!shutdown){
                    int len = is.available();
                    if (len >= 8){              //то есть доступны номер команды и ее длина
                        buffer = new byte[8];
                        is.read(buffer, 0, 8);
                        FieldDeserializer dsr = new FieldDeserializer(buffer);
                        int com_id = dsr.readIntNonSerialized();
                        setDecoder(com_id);
                        int l = dsr.readIntNonSerialized();
                        buffer = new byte[l];
                        is.read(buffer, 0, l);
                        IProtoDeserializable command = dcdr.DeserializeCommand(buffer);
                        received.put(command);
                    }
                }
            } catch (Exception e){
                System.out.println("Receiver: " + e.getMessage());
                e.printStackTrace();
            }

        }
    }

    private class Sender implements Runnable{

        public void run() {
            while (!shutdown) {
                IProtoSerializableRequest r = (IProtoSerializableRequest)to_send.take();
                try {
                    Thread.sleep(100);
                    send(r.toByte());
                } catch (InterruptedException ex) {
                    ex.printStackTrace();
                } catch (IOException ex) {
                    ex.printStackTrace();
                } catch (Exception ex) {
                    System.out.println("Sender: " + "произошло что-то страшное");
                }
            }
        }
    }

    private class Executor implements Runnable{

        public void run() {
            while (!shutdown) {
                ICommand r = (ICommand)received.take();
                r.Execute(gameContext);
            }
        }
    }
}
