/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package util;

import java.io.IOException;
import java.io.InputStream;
import java.util.Vector;

/**
 *
 * @author е
 *
 * Переделать под возвращение tiles[][]
 */
public class Transmitter {

    public static int[][] getBinaryData(InputStream is) throws IOException {
        int[][] map = null;
        int c = 0;
        byte[] b=new byte[1026];
        c = is.read(b,0,1026);

        if (c>2) {
            // читаем размер карты (она квадратная -- size x size)
            int sizeX = b[0], sizeY=b[1];
            map = new int[sizeX][];
            for (int i = 0; i < sizeX;i++) {
                map[i] = new int[sizeY];
            }
            //читаем карту
            
                //map[i] = new int[sizeY];
            for (int j = 0;j < sizeY;j++) {
                for (int i = 0; i < sizeX;i++)
                    map[i][j] = b[i+j*sizeY+2];
            }
           // for (int i=0; i<c;i++)
          //  System.out.write(b[i]);
        }
        return map;
    }

    //unstable! xsize
    public static int[][] getBinaryData(InputStream is, int fromX, int sizeX, int fromY, int sizeY)
            throws IOException {
        int[][] map = null;
        int filesizeX = is.read();
        int filesizeY = is.read();
        System.out.println(fromX);
        System.out.println(fromY);
        if (fromX < 0 || fromX + sizeX > filesizeX || fromY < 0 || fromY + sizeY > filesizeY)
            return null;

        int offset = fromY * filesizeX + fromX;
        is.skip(offset); //делаем fileseek на начало

        byte[] b = new byte[sizeX]; //массив для 1 строки карты

        map = new int[sizeX][]; //инициализируем результат
        for (int i = 0; i < sizeX;i++) {
            map[i] = new int[sizeY];
        }
        
        //читаем карту
        for (int j = 0; j < sizeY; j++) {
            is.read(b);//читаем строку длиной sizeX
            for (int i = 0; i < sizeX; i++)
                map[i][j] = b[i];
            is.skip(filesizeX - sizeX);//fileseek на след.строку
        }
        //System.out.println("getBinaryData ends.");
        return map;
    }

    public static Vector[] getTextData(InputStream is) throws IOException {
        Vector[] map=new Vector[0];
        int c = 0;
        byte[] b=new byte[256];
        c = is.read(b,0,255);

        if (c>1) {
            // читаем размер карты (она квадратная -- size x size)
            int size = b[0] - '0';
            map = new Vector[size];
            //читаем карту
            for (int i = 0; i < size;i++) {
                map[i] = new Vector();
                for (int j = 0;j < size;j++)
                    map[i].addElement(new Integer(b[i*size+j+1] - '0'));
            }
           // for (int i=0; i<c;i++)
          //  System.out.write(b[i]);
        }
        return map;
    }
}
