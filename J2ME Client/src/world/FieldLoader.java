/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package world;

import Logic.Location;
import util.Transmitter;
import java.io.IOException;
import java.io.InputStream;
import java.util.Hashtable;

/**
 * предполагается, что класс работает через класс транспорта с сервером.
 * временно сделана заглушка - данные прямо в этом классе.
 * возможно впоследствии, для снижения размера используемой памяти, хэш-таблица будет удалена.
 * (или заменена на другой способ кэширования)
 * тогда останется прямая загрузка с сервера.
 * @author е
 */

public class FieldLoader {

    Hashtable FieldSectors; //доступна извне для WorldPathFinder

    public FieldLoader() {
        FieldSectors = new Hashtable(8);
    }

    public FieldSector getStrategyFieldSector(Location coords) {
        if (coords == null) return null;
        FieldSector buf = (FieldSector) FieldSectors.get(coords);
        if (buf == null) {
            System.out.println("getStrategyFieldSector: loading from server: " + coords.toString());
            //загрузить с сервера


            //временная заглушка: грузим из файла
            try {
                InputStream is = getClass().getResourceAsStream("/gribles.bin");
                buf = new FieldSector(Transmitter.getBinaryData(is,
                        coords.getX() * FieldSector.SECTOR_WIDTH,
                        FieldSector.SECTOR_WIDTH,
                        coords.getY() * FieldSector.SECTOR_HEIGHT,
                        FieldSector.SECTOR_HEIGHT));
                //if (buf == null) return null;
                buf.setGlobalCoordinates(new Location(coords));
                System.out.println("getStrategyFieldSector: adding to hash: " + coords.toString());
                FieldSectors.put(buf.getGlobalCoordinates(), buf); //добавляем в хэш-таблицу
                is.close();
            } catch (IOException ex) {
                ex.printStackTrace();
            }           
        }
        return buf;
    }
}
