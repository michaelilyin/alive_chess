/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package net;

import world.FieldSector;

/**
 *
 * @author Вадим
 */
public class SynchronizedData {
    private FieldSector data;

    public SynchronizedData(FieldSector data) {
        setData(data);
    }

    public synchronized FieldSector getData() {
        return data;
    }

    public synchronized void setData(FieldSector data) {
        this.data=data;
    }
}
