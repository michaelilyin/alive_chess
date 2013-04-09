/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package gui;

import Logic.GameMidlet;
import world.FieldSector;
import Logic.Location;
import gui.SpriteLoader;
import java.io.IOException;
import java.util.Vector;
import javax.microedition.lcdui.game.Sprite;
import javax.microedition.lcdui.game.TiledLayer;
import obj.AnimatedObject;
import obj.Castle;
import obj.Hero;
import obj.LayeredObject;
import world.WorldContext;
import world.WorldPathFinder;

/**
 *
 * @author Вадим и Роман
 */
public class WorldCanvas extends ExtGameCanvas {
    
    private WorldContext World;
    private WorldPathFinder PathFinder;
    private LayerCache LCache;
    

    private boolean HeroInBattle;
    private byte lastHeroDirection;

    public WorldCanvas(GameMidlet m, WorldContext context) {
        super(m);
        World = context;
        try {
            this.init();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    protected void init() throws IOException {
        super.init();
        System.out.println("WorldCanvas init()");

        PathFinder = new WorldPathFinder(World);
        //заглушка
        //sector = fl.getStrategyFieldSector(new Location());

        Location defpos = new Location(0, 0);
        spriteCursor.setPosition(defpos.getX() * SpriteLoader.TILE_WIDTH,
                defpos.getY() * SpriteLoader.TILE_HEIGHT);
        
        //TODO: цикл по героям, а лучше по всем объектам WorldContext
        SpriteLoader.updateLayerManager(lm, spriteCursor);
        for(int i = 0; i < World.getAnimatedObjects().size(); i++) {
            AnimatedObject h = (AnimatedObject) World.getAnimatedObjects().elementAt(i);
            SpriteLoader.updateLayerManager(lm, h.getAnimator().getSprite());
            timer.scheduleAtFixedRate(h.getAnimator(), 0,
                h.getAnimator().getCurrentFrameDelay());
        }
        //System.out.println(World.getLayeredObjects().size());
        for(int i = 0; i < World.getLayeredObjects().size(); i++) {
            LayeredObject c = (LayeredObject)World.getLayeredObjects().elementAt(i);
            SpriteLoader.updateLayerManager(lm, c.getObjLayer());
        }
        LCache = new LayerCache(World, lm, defpos);
        
        
        HeroInBattle = false;
        adjustViewport(this.viewPortX, this.viewPortY, spriteCursor);
        System.out.println("WorldCanvas init() ends");
    }

    public boolean spriteCollides(Sprite sprite) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public boolean spriteCollidesWithBorder(Sprite sprite) {
        return
            (sprite.getRefPixelX() < 0)
            || (sprite.getRefPixelY() < 0)
            || (sprite.getRefPixelX() >= World.getSizeX() * FieldSector.SECTOR_WIDTH * SpriteLoader.TILE_WIDTH)
            || (sprite.getRefPixelY() >= World.getSizeY() * FieldSector.SECTOR_HEIGHT * SpriteLoader.TILE_HEIGHT)
            ;
    }

    /**
     * Попытка движения спрайта. проверка на столкновение только с краями поля!
     * При успехе вызов adjustViewport
     * @param sprite
     * @param speed
     * @param direction
     * @return
     */
    protected boolean tryMoveWithinBorderAndAdjust(Sprite sprite, int speed, int direction) {
        boolean res = true;
        int xSpeed, ySpeed;
        xSpeed = ySpeed = 0;
        switch (direction) {
            case UP: { ySpeed = -speed; break; }
            case RIGHT: { xSpeed = speed; break; }
            case DOWN: { ySpeed = speed; break; }
            case LEFT: { xSpeed = -speed; break; }
            default: { throw new IllegalArgumentException("Invalid direction."); }
        }
        sprite.move(xSpeed, ySpeed);
        if (spriteCollidesWithBorder(sprite)) {
            res = false;
            sprite.move(-xSpeed, -ySpeed);
        }
        else adjustViewport(this.viewPortX + xSpeed, this.viewPortY + ySpeed, sprite);
        return res;
    }

    public void run() {
        int keyState, step = -1, pos = 0;
        Vector trace = null;
        Vector SpriteVector = new Vector();
        Location StartLoc = null;
        while (!this.interrupted) {
            //check for user input
            keyState = getKeyStates();

            if ((keyState & LEFT_PRESSED) != 0) {
                lastDirection = LEFT;
                tryMoveWithinBorderAndAdjust(spriteCursor, CURSOR_SPEED, lastDirection);
            }
            else if ((keyState & RIGHT_PRESSED) != 0) {
                lastDirection = RIGHT;
                tryMoveWithinBorderAndAdjust(spriteCursor, CURSOR_SPEED, lastDirection);
            }
            if ((keyState & UP_PRESSED) != 0) {
                lastDirection = UP;
                tryMoveWithinBorderAndAdjust(spriteCursor, CURSOR_SPEED, lastDirection);
            }
            else if ((keyState & DOWN_PRESSED) != 0) {
                lastDirection = DOWN;
                tryMoveWithinBorderAndAdjust(spriteCursor, CURSOR_SPEED, lastDirection);
            }

            if ((keyState & FIRE_PRESSED) != 0) {
//                if (spriteCollidesWithCastle(spriteKarel) && spriteCollidesWithCastle(spriteCursor)) {
//                    Midlet.viewCastle();
//                }
                // запускаем обращение к сокету, пока заглушка
                //отправить сообщение о перемещении героя в клетку.
                
                //World.fireUpdate(new MoveKingRequest(new Location(spriteCursor.getRefPixelX()/16, spriteCursor.getRefPixelY()/16)));
                if (step == -1) {
                    clear_trace(SpriteVector);
                    pos = 0;
                    //trace = sector.findShortestPath(StartsX, StartsY, EndsX, EndsY);
                    trace = PathFinder.findGlobalShortestPath(World.getPlayer().getMainHero().getRefX(),
                            World.getPlayer().getMainHero().getRefY(),
                            spriteCursor.getRefPixelX()/16,
                            spriteCursor.getRefPixelY()/16);
                    if (trace == null || trace.isEmpty())
                        continue;
                    try {
                        SpriteVector = drawTrace(trace);
                    } catch (IOException ex) {
                        ex.printStackTrace();
                    }
                    step = trace.size()-1;
                    World.getPlayer().getMainHero().getAnimator().forward();
                    StartLoc = new Location(World.getPlayer().getMainHero().getLoc());
                }
            }

            if (step >= 0) {
                Location PrevLoc,CurLoc = (Location)trace.elementAt(step);
                if (step == trace.size() - 1) {
                    PrevLoc = StartLoc;
                }
                else {
                    PrevLoc = (Location)trace.elementAt(step+1);
                }
                move_hero(PrevLoc,CurLoc);
                pos++;

                if (pos%4 == 0) {
                    step--;
                    clear_sprite(SpriteVector, 0);
                    //пока нет связи с сервером, вручную меняем координаты героя
                    Location hloc = World.getPlayer().getMainHero().getLoc();
                    if (CurLoc.getX() < PrevLoc.getX()) hloc.incX(-1);
                    else if (CurLoc.getX() > PrevLoc.getX()) hloc.incX(1);
                    if (CurLoc.getY() < PrevLoc.getY()) hloc.incY(-1);
                    else if (CurLoc.getY() > PrevLoc.getY()) hloc.incY(1);
                }
            }
            else {
                step = -1;
                World.getPlayer().getMainHero().getAnimator().setMoving(false);
                //clear_trace(SpriteVector);
            }


//            if (heroCollidesWithEnemy()) {
//            //инициировать сражение
//                spriteThomasRandomMovement.stop();
//                //spriteKarel
//                HeroInBattle = true;
//                System.out.println("StrategyCanvas initiates the battle");
//                Midlet.viewBattle();
//                System.out.println("StrategyCanvas after initiating the battle");
//                HeroInBattle = false;
//            }

            repaintOnIteration();
        }
    }

    public void stop() {
        super.stop();
    }

    /**
     * ф-ия для отладки: отображает рез-т алг.Дейкстры - дистанции точек.
     */
    /*public void drawDistances(){
        Graphics g = getGraphics();
        for(int i = 0; i < StrategyField.getWidth(); i++)
            for(int j = 0; j < StrategyField.getHeight(); j++)
                if (StrategyField.getCell(i, j).getDistance() != Integer.MAX_VALUE) {
                    //g.setFont(Font.getDefaultFont());
                    //g.setColor(0, 0, 0);
                    g.drawString(String.valueOf(StrategyField.getCell(i, j).getDistance()), -viewPortX + i*16 + 8, -viewPortY + j*16, g.TOP |g.LEFT); //StrategyField.getCell(i, j).getDistance();
                }
        flushGraphics(0, 0, this.getWidth(), this.getHeight());


        try {
            Thread.sleep(5000);
        } catch (InterruptedException ex) {
            ex.printStackTrace();
        }
    }*/

    /**
     * рисуем путь на карте.
     */
    public Vector drawTrace(Vector trace) throws IOException{
        Vector SpriteVector = new Vector();
        Sprite TraceArrow = new Sprite(sl.getTraceArrow());
        TraceArrow.defineReferencePixel(8, 8);

        if (trace == null || trace.isEmpty()) return null;
        Sprite sprite = new Sprite(TraceArrow);
        Location CurLoc, NextLoc;// = trace[trace.length - 1];

        for (int i = trace.size() - 1; i > 0; i--) {
            sprite = new Sprite(TraceArrow);
            CurLoc = (Location)trace.elementAt(i);
            NextLoc = (Location)trace.elementAt(i - 1);
            if (Math.abs(NextLoc.getX() - CurLoc.getX())
                    + Math.abs(NextLoc.getY() - CurLoc.getY()) == 2) {
                sprite.setFrame(1);
            }
            //else sprite.setFrame(0);

            if (NextLoc.getY() > CurLoc.getY()){
                if (NextLoc.getX() <= CurLoc.getX()) {
                    sprite.setTransform(Sprite.TRANS_ROT180);
                }
                else sprite.setTransform(Sprite.TRANS_ROT90);
            }
            else if (NextLoc.getY() < CurLoc.getY()){
                if (NextLoc.getX() < CurLoc.getX())
                    sprite.setTransform(Sprite.TRANS_ROT270);
            }
            else if (NextLoc.getY() == CurLoc.getY()){
                 if (NextLoc.getX() > CurLoc.getX())
                     sprite.setTransform(Sprite.TRANS_ROT90);
                 else sprite.setTransform(Sprite.TRANS_ROT270);
            }


            this.appendSpriteToLm(sprite, CurLoc.getX() * SpriteLoader.TILE_WIDTH,
                    CurLoc.getY() * SpriteLoader.TILE_HEIGHT);
            SpriteVector.addElement(sprite);

        }

        //рисуем крестик
        NextLoc = (Location)trace.elementAt(0);
        sprite = new Sprite(TraceArrow);
        sprite.setFrame(2);
        this.appendSpriteToLm(sprite, NextLoc.getX() * SpriteLoader.TILE_WIDTH,
                    NextLoc.getY() * SpriteLoader.TILE_HEIGHT);
        SpriteVector.addElement(sprite);
        return SpriteVector;
    }

    /**
     * Удаляет спрайты массива из layermanager, затем очищает массив
     */
    public void clear_trace(Vector sprite_vector) {
        while (sprite_vector.size() > 0)
            clear_sprite(sprite_vector, 0);
        //sprite_vector.removeAllElements();
    }

    private void clear_sprite(Vector sprite_vector, int index) {
        if (index < 0 || index >= sprite_vector.size()) return;
        lm.remove((Sprite)sprite_vector.elementAt(index));
        sprite_vector.removeElementAt(index);
    }

    /**
     * Добавляет спрайт в layermanager и в sprite_vector
     */

    public void appendSpriteToLm(Sprite sprite, int x, int y) {
        sprite.setPosition(x,y);
        lm.insert(sprite, 2);
        //this.SpriteVector.addElement(sprite);
    }

    /**
     * Двигает спрайт героя из клетки PrevLoc в клетку CurLoc (клетки соседние).
     * Вызывается 4 раза для продвижения на 1 клетку (тем самым обеспечивая плавность)
     * */
    public void move_hero(Location PrevLoc, Location CurLoc) {
        if (HeroInBattle) return;
        int dx = CurLoc.getX()- PrevLoc.getX();
        int dy = CurLoc.getY()- PrevLoc.getY();

        if (dx < 0) lastHeroDirection = LEFT;
        else if (dx > 0) lastHeroDirection = RIGHT;
        else if (dy < 0) lastHeroDirection = UP;
        else lastHeroDirection = DOWN;
                  
        World.getPlayer().getMainHero().getAnimator().setDirection(lastHeroDirection);

        //assign the sequence playback direction
        
        //move the sprite
        World.getPlayer().getMainHero().getAnimator().getSprite().move(dx * SpriteLoader.TILE_WIDTH / 4,
                dy * SpriteLoader.TILE_HEIGHT / 4);
    }

    protected void adjustViewport(int x, int y, Sprite sprite) {

        //TiledLayer tlCenter
        tlBase = LCache.getLayer(LayerCache.CENTER);
        int sx = sprite.getX();
        int sy = sprite.getY();

        int xmin = this.viewPortX + MIN_BUFFER;
        int xmax = this.viewPortX + this.getWidth() - sprite.getWidth() - MIN_BUFFER;
        int ymin = this.viewPortY + MIN_BUFFER;
        int ymax = this.viewPortY + this.getHeight() - sprite.getHeight() - MIN_BUFFER;

        //if the sprite is not near the any screen edges don't adjust
        if (sx >= xmin && sx <= xmax && sy >= ymin && sy <= ymax) {
            return;
        }

        //if the sprite is moving left but isn't near the left edge of the screen don't adjust
        if (this.lastDirection == LEFT && sx >= xmin) {
            return;
        }
        //if the sprite is moving right but isn't near the right edge of the screen don't adjust
        if (this.lastDirection == RIGHT && sx <= xmax) {
            return;
        }
        //if the sprite is moving up but isn't at near top edge of the screen don't adjust
        if (this.lastDirection == UP && sy >= ymin) {
            return;
        }
        //if the sprite is moving down but isn't at near bottom edge of the screen don't adjust
        if (this.lastDirection == DOWN && sy <= ymax) {
            return;
        }

        //only adjust x to values that ensure the base tiled layer remains visible
        //and no white space is shown
        //ВНИМАНИЕ! поле lastDirection важно для правильного скроллинга
        int buf;
        buf = x;// = sprite.getRefPixelX();
        if (x < this.tlBase.getX() && lastDirection == LEFT) { //если влево
            if (LCache.getCenterCoords().getX() == 0) { //если слева граница
                buf = this.tlBase.getX();
            }
            else if (sprite.getRefPixelX() < this.tlBase.getX()) LCache.MoveCenter(LayerCache.LEFT); //иначе двигаем центр
        } else if (x + this.getWidth() > this.tlBase.getX() + this.tlBase.getWidth()) {
            if (LCache.getCenterCoords().getX() == World.getSizeX() - 1) {
                buf = this.tlBase.getX() + this.tlBase.getWidth() - this.getWidth();
            }
            else if (sprite.getRefPixelX() > this.tlBase.getX() + this.tlBase.getWidth())
                LCache.MoveCenter(LayerCache.RIGHT);
        } 
        viewPortX = buf;
        

        //only adjust y to values that ensure the base tiled layer remains visible
        //and no white space is shown
        buf = y;// = sprite.getRefPixelY();
        if (y < this.tlBase.getY() && lastDirection == UP) {
            if (LCache.getCenterCoords().getY() == 0) {
                buf = this.tlBase.getY();
            }
            else if(sprite.getRefPixelY() < this.tlBase.getY()) LCache.MoveCenter(LayerCache.UP);
        } else if (y + this.getHeight() > this.tlBase.getY() + this.tlBase.getHeight() && lastDirection == DOWN) {
            if (LCache.getCenterCoords().getY() == World.getSizeY() - 1) {
                buf = this.tlBase.getY() + this.tlBase.getHeight() - this.getHeight();

            }
            else if(sprite.getRefPixelY() > this.tlBase.getY() + this.tlBase.getHeight())
                LCache.MoveCenter(LayerCache.DOWN);
        }
        viewPortY = buf;
        tlBase = LCache.getLayer(LayerCache.CENTER);
        //adjust the viewport
        this.lm.setViewWindow(this.viewPortX, this.viewPortY, this.getWidth(), this.getHeight());
    }
}
