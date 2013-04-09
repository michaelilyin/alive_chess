package vsu.alivechess.net.executors;

import com.google.protobuf.Message;

public abstract class AbstractExecutor implements IExecutor{
    private Message resp;

    public void setResponce(Message msg) {
        resp = msg;
    }

    public Message getResponce() {
        return resp;
    }	
}
