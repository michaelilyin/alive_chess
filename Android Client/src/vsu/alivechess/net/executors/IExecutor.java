/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package vsu.alivechess.net.executors;

import com.google.protobuf.Message;

/**
 *
 * @author Al-mal
 */
public interface IExecutor {
    public void execute();
    public void setResponce(Message msg);
    public Message getResponce();
}
