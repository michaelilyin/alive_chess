/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package util;

import java.io.IOException;
import javax.microedition.lcdui.Image;

/**
 *
 * @author е
 */

//класс представляет собой набор методов по заргузке картинок из ресурсов
//все методы скопированы из gamedesign
//имена методов и файлов ресурсов оставлены без изменений
public class ImageLoader {

    private static Image topview_tiles;
    private static Image arrows;
    private static Image Border30x30;
    private static Image Border16x16;
    private static Image cursor;
    private static Image blood;
    private static Image Back30x30;
    private static Image Back16x16;
    private static Image CastleBackground1;
    private static Image castle1;
    private static Image StrategyField;
    private static Image Heroes;
    private static Image FieldObjects;


    public static Image getStrategyField() throws IOException {
        if (StrategyField == null) {
            StrategyField = Image.createImage("/field.png");
        }
        return StrategyField;
    }

    public static Image getHeroes() throws IOException {
        if (Heroes == null) {
            Heroes = Image.createImage("/heroes.png");
        }
        return Heroes;
    }

    public static Image getFieldObjects() throws IOException {
        if (FieldObjects == null) {
            FieldObjects = Image.createImage("/fieldobjects.png");
        }
        return FieldObjects;
    }
//    public static Image getTopview_tiles() throws java.io.IOException {
//        if (topview_tiles == null) {
//            topview_tiles = Image.createImage("/topview_tiles.png");
//        }
//        return topview_tiles;
//    }

    public static Image getArrow1() throws java.io.IOException {
        if (arrows == null) {
            arrows = Image.createImage("/arrows.png");
        }
        return arrows;
    }

    public static Image getBorder30x30() throws java.io.IOException {
        if (Border30x30 == null) {
            Border30x30 = Image.createImage("/Border30x30.png");
        }
        return Border30x30;
    }

    public static Image getCursor() throws java.io.IOException {
        if (cursor == null) {
            cursor = Image.createImage("/cursor.png");
        }
        return cursor;
    }

    public static Image getBlood() throws java.io.IOException {
        if (blood == null) {
            blood = Image.createImage("/blood.png");
        }
        return blood;
    }

    public static Image getBack30x30() throws java.io.IOException {
        if (Back30x30 == null) {
            Back30x30 = Image.createImage("/back1.png");
        }
        return Back30x30;
    }

    public static Image getBack16x16() throws java.io.IOException {
        if (Back16x16 == null) {
            Back16x16 = Image.createImage("/Back16x16.png");
        }
        return Back16x16;
    }

    public static Image getBorder16x16() throws java.io.IOException {
        if (Border16x16 == null) {
            Border16x16 = Image.createImage("/Border16x16.png");
        }
        return Border16x16;
    }

    public static Image getCastleBackground1() throws java.io.IOException {
        if (CastleBackground1 == null) {
            CastleBackground1 = Image.createImage("/CastleBackground1.png");
        }
        return CastleBackground1;
    }

    /*public static Image getCastle1() throws java.io.IOException {
        if (castle1 == null) {
            castle1 = Image.createImage("/castle1.png");
        }
        return castle1;
    }*/
}
