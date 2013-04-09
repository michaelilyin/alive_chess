/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package net;

import java.util.Vector;


/**
 *
 * @author Вадим
 */
public class Queue {
     private Vector tasks;
    private boolean waiting;
    private boolean shutdown;

    public Queue() {
        tasks = new Vector();
        waiting = false;
    }

    public void put(Object r) {
        tasks.addElement(r);
        if (waiting) {
            synchronized (this) {
                notifyAll();
            }
        }
    }

    public Object take() {
        if (tasks.isEmpty()) {
            synchronized (this) {
                waiting = true;
                try {
                    while (waiting) {
                        wait();
                    }
                } catch (InterruptedException ie) {
                }
            }
        }
        Object result = (Object) tasks.firstElement();
        tasks.removeElementAt(0);
        return result;
    }
}
