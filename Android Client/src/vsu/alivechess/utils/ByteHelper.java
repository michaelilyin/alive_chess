/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package vsu.alivechess.utils;

/**
 *
 * @author Al-mal
 */
public class ByteHelper {

    public static byte[] concatBytes(byte[] first, byte[]... rest) {
        int totalLength = first.length;
        for (byte[] array : rest) {
            totalLength += array.length;
        }
        byte[] result = new byte[totalLength];//Arrays.copyOf(first, totalLength);
        System.arraycopy(first, 0, result, 0, first.length);
        int offset = first.length;
        for (byte[] array : rest) {
            System.arraycopy(array, 0, result, offset, array.length);
            offset += array.length;
        }
        return result;
    }


    public static byte[] getPartByteArray(byte[] b, int start, int length){
        byte[] result = new byte[length];
        for (int i = start; i < start+length; i++) {
            result[i-start] = b[i];
        }
        return result;
    }

    public static final int byteArrayToInt(byte [] b) {
            return (b[3] << 24)
                    + ((b[2] & 0xFF) << 16)
                    + ((b[1] & 0xFF) << 8)
                    + (b[0] & 0xFF);
    }

    public static byte[] intToByteArray(int value) {
        return new byte[] {
                (byte)value,
                (byte)(value >>> 8),
                (byte)(value >>> 16),
                (byte)(value >>> 24)
                };
    }

}
