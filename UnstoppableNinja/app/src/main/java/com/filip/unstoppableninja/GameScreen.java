package com.filip.unstoppableninja;

import android.graphics.Color;
import android.graphics.Point;

import com.filip.androidgames.framework.Game;
import com.filip.androidgames.framework.Graphics;
import com.filip.androidgames.framework.Input.TouchEvent;
import com.filip.androidgames.framework.Pixmap;
import com.filip.androidgames.framework.Screen;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

class Enemy
{
    int x;
    int y;
    int dir;
    Point offset;
    GameScreen parent;
    boolean alerted;
    Enemy(int myX, int myY, GameScreen myParent){
        x=myX;
        y=myY;
        parent = myParent;
        alerted = false;
    }
    void Update(){
        Random randomGenerator = new Random();
        if(alerted) {
            int newDir = parent.getWaypoint(x, y);
            if (newDir != 0) {
                dir = newDir - 1;
            }
        }
        else{

            if(randomGenerator.nextInt(10)==9){
                if(randomGenerator.nextInt(2)==1){
                    dir--;
                }
                else{
                    dir++;
                }

            }
        }
        offset = new Point(0,0);
        setDir();
        int tar =getWall(x+offset.x,y+offset.y);

        if(tar==2||tar==10){
            offset.x=0;offset.y=0;
            if(randomGenerator.nextInt(2)==1){
                dir--;
            }
            else{
                dir++;
            }
        }
        x+=offset.x;
        y+=offset.y;
        y=Math.min(Math.max(y,1),41);
        x=Math.min(Math.max(x,1),41);

        //

        setDir();
        Point newOffset = offset;
        for(int i=0;i<7;i++){
            Point playerPoint = parent.getPlayerPos();

            if(playerPoint.x==x+newOffset.x && playerPoint.y==y+newOffset.y){
                parent.playerWaypoint();
                System.out.println("alert");
                alerted=true;

            }

            tar =getWall(x+newOffset.x,y+newOffset.y);
            if(tar==2 || tar ==10){
                break;
            }

            if(newOffset.x>0){newOffset.x++;}
            if(newOffset.x<0){newOffset.x--;}
            if(newOffset.y>0){newOffset.y++;}
            if(newOffset.y<0){newOffset.y--;}
        }

    }

    void setDir(){
        if(dir<0){dir=3;}
        if(dir>3){dir=0;}
        if(dir==0){offset.y=-1;}
        if(dir==1){offset.x=1;}
        if(dir==2){offset.y=1;}
        if(dir==3){offset.x=-1;}
    }

    int getX(){
        return x;
    }
    int getY(){
        return y;
    }
    int getDir(){
        return dir;
    }
    int getWall(int checkX,int checkY)
    {
        return parent.getSpace(checkX,checkY);
    }

    boolean killPlayer (int theX, int theY)
    {
        if (theX == x && theY == y)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

}

class Powerup
{
    int Px;
    int Py;

    int GetPX(){
        return Px;
    }
    int GetPY(){
        return Py;
    }
    GameScreen Parent;
    Pixmap pickup;
    Powerup(int myX, int myY, GameScreen myParent)
    {
        Px=myX;
        Py=myY;
        Parent = myParent;
    }
    void PUpdate()
    {
        Pixmap smoke = Assets.smokeDrop;
        Pixmap shuriken =Assets.shurikenDrop;
        Pixmap caltrops =Assets.caltropsDrop;
        Random rand = new Random();
        int Pdrop = rand.nextInt(3) + 6;

        if(Pdrop ==6)
        {
            pickup = smoke;
        }
        if(Pdrop ==7)
        {
            pickup = shuriken;
        }
        if(Pdrop ==8)
        {
            pickup = caltrops;
        }
    }
}


public class GameScreen extends Screen
{
    enum GameState
    {
        Running,
        Paused,
        GameOver
    }
    // These are map variables.
    int [][] mapArray = new int [43][43];
    int [][] waypointArray = new int[43][43];
    int playerX, playerY;
    //0123, UpRightDownLeft, set playerDir to previousDir +-1 when turning
    int playerDir = 0, previousDir = 0;
    //stuff for tilt
    int drawAgain = 1, Score = 0, KilledEnemies = 1;
    Pixmap tiltPixmap = Assets.tiltUp; Pixmap enemyPixmap = Assets.enemyDown;
    //These are update speeed variables
    float tickCounter = 0.0f, tickTarget = 0.25f, timePassed = 0.0f, enemySpawnTime = 5.0f;
    boolean spawningEnemy = false;

    Graphics g = game.getGraphics();

    GameState state = GameState.Running;

    List<Enemy> enemies;
    //World world;
    //int oldScore = 0;
    //String score = "0";
    List<Powerup> Pickups;

    public GameScreen(Game game)
    {
        super(game);
        //world = new World();
        generateMap(10, 20);
        playerX = mapArray.length / 2;
        playerY = mapArray.length / 2;
        enemies = new ArrayList<Enemy>();
        Pickups = new ArrayList<Powerup>();
        drawMap();
    }

    @Override
    public void update(float deltaTime)
    {
        List<TouchEvent> touchEvents = game.getInput().getTouchEvents();

        //if(state == GameState.Ready)
        //   updateReady(touchEvents);
        if(state == GameState.Running)
            updateRunning(touchEvents, deltaTime);
        if(state == GameState.Paused)
            updatePaused(touchEvents);
        if(state == GameState.GameOver)
            updateGameOver(touchEvents);
    }



    // This function will draw the initial map.
    private void drawMap ()
    {
        Graphics g = game.getGraphics();
        int x, y;
        //g.getHeight()

        for (x = playerX - 4; x <= playerX + 4; x++)
        {
            for (y = playerY - 6; y <= playerY + 6; y++)
            {
                int drawPos[] = {
                        //((y - playerY) * Assets.floorTile.getWidth()) + (g.getWidth()-Assets.floorTile.getWidth()) / 2
                        //, ((x - playerX) * (Assets.floorTile.getHeight())) + (g.getHeight()-Assets.floorTile.getHeight())/2
                        ((x - playerX) * Assets.floorTile.getHeight()) + (g.getWidth()-Assets.floorTile.getWidth()) / 2
                        , ((y - playerY) * Assets.floorTile.getWidth()) + (g.getHeight()-Assets.floorTile.getHeight())/2
                };
                // Check if we're in range of the map array. If so, we start drawing.
                if (x >= 0 && x <= 42 && y >= 0 && y <= 42)
                {
                    //get the pixel coordinates of the tile to draw
                    if (mapArray[x][y] == 0 || mapArray[x][y] == 1)
                    {
                        //Draw floors here
                        g.drawPixmap(Assets.floorTile, drawPos[0], drawPos[1], ((x+y)%3*90));
                    }
                    else if (mapArray[x][y] == 2)
                    {
                        //Draw walls here
                        g.drawPixmap(Assets.wallTile, drawPos[0], drawPos[1]);
                    }
                    else if (mapArray[x][y] == 3)
                    {
                        //Player starting point, draw a floor
                        g.drawPixmap(Assets.floorTile, drawPos[0], drawPos[1]);
                    }

                    else if (mapArray[x][y] == 4)
                    {
                        //enemy will be drawn in the following for loop
                        // Enemy starting point.
                        g.drawPixmap(Assets.floorTile, drawPos[0], drawPos[1]);
                        //g.drawPixmap(Assets.enemyDown, drawPos[0], drawPos[1]);
                    }

                    else if (mapArray[x][y] == 5)
                    {
                        //pickup will be drawn in the following for loop
                        g.drawPixmap(Assets.floorTile, drawPos[0], drawPos[1]);

                    }
                    else
                    {
                        //Draw a pause icon by default to show unassigned grid spot
                        g.drawPixmap(Assets.wallTile, drawPos[0], drawPos[1], ((x+y)%3*90));
                    }
                    //if the current location is the player position, draw the player
                    if(playerX == x && playerY == y){
                        g.drawPixmap(tiltPixmap, drawPos[0], drawPos[1]);
                        //g.drawPixmap(Assets.tiltUp, drawPos[0], drawPos[1]);
                    }
                } else {
                    g.drawPixmap(Assets.wallTile, drawPos[0], drawPos[1]);
                }
            }
        }
        for(int i=0;i<enemies.size();i++)
        {
            if(enemies.get(i)!=null)
            {
                if(Math.abs( enemies.get(i).getX() - playerX) <= 4
                        && Math.abs( enemies.get(i).getY() - playerY) <= 4){
                    switch (enemies.get(i).getDir()) {
                        case 0:
                            enemyPixmap = Assets.enemyUp;
                            break;
                        case 1:
                            enemyPixmap = Assets.enemyRight;
                            break;
                        case 2:
                            enemyPixmap = Assets.enemyDown;
                            break;
                        case 3:
                            enemyPixmap = Assets.enemyLeft;
                            break;
                    }
                    g.drawPixmap(enemyPixmap, ((enemies.get(i).getX() - playerX) * Assets.floorTile.getHeight()) + (g.getWidth() - Assets.floorTile.getWidth()) / 2
                            , ((enemies.get(i).getY() - playerY) * Assets.floorTile.getWidth()) + (g.getHeight() - Assets.floorTile.getHeight()) / 2);
                }
            }
        }

        for(int i=0;i<Pickups.size();i++)
        {
            if(Pickups.get(i)!=null)
            {
                if(Math.abs( Pickups.get(i).GetPX() - playerX) <= 5
                        && Math.abs( Pickups.get(i).GetPY() - playerY) <= 5)
                {
                    g.drawPixmap(Pickups.get(i).pickup, ((Pickups.get(i).GetPX() - playerX) * Assets.floorTile.getHeight()) + (g.getWidth() - Assets.floorTile.getWidth()) / 2
                            , ((Pickups.get(i).GetPY() - playerY) * Assets.floorTile.getWidth()) + (g.getHeight() - Assets.floorTile.getHeight()) / 2);
                }
            }
        }


    }
    private void updateReady(List<TouchEvent> touchEvents)
    {
        if(touchEvents.size() > 0)
            state = GameState.Running;
    }

    private void updateRunning(List<TouchEvent> touchEvents, float deltaTime)
    {
        int len = touchEvents.size();
        for(int i = 0; i < len; i++) {
            TouchEvent event = touchEvents.get(i);
            if(event.type == TouchEvent.TOUCH_UP) {
                if(event.x < 64 && event.y < 64) {
                    if(Settings.soundEnabled)
                        Assets.click.play(1);
                    state = GameState.Paused;
                    return;
                }
            }
            if(event.type == TouchEvent.TOUCH_DOWN) {
                if(event.x < 64 && event.y > (g.getHeight()-64)) {
                    //world.snake.turnLeft();
                    playerDir = previousDir - 1;
                    drawAgain = 1;//since Tilt is facing a different direction, redraw the scene
                }
                if(event.x > (g.getWidth()-64) && event.y > (g.getHeight()-64)) {
                    //world.snake.turnRight();
                    playerDir = previousDir + 1;
                    drawAgain = 1;//since Tilt is facing a different direction, redraw the scene
                }
                if (playerDir > 3) playerDir -= 4;
                if (playerDir < 0) playerDir += 4;
                waypointArray[playerX][playerY]=playerDir+1;
                switch (playerDir) {
                    case 0:
                        tiltPixmap = Assets.tiltUp;
                        break;
                    case 1:
                        tiltPixmap = Assets.tiltRight;
                        break;
                    case 2:
                        tiltPixmap = Assets.tiltDown;
                        break;
                    case 3:
                        tiltPixmap = Assets.tiltLeft;
                        break;
                }
            }
        }

        tickCounter += deltaTime;
        timePassed += deltaTime;
        if (tickCounter >= tickTarget) {
            tickCounter -= tickTarget;
            drawAgain = 1;

            // Joe's enemy spawn system

            // this will eventually spawn an enemy at a random location.
            if (timePassed % enemySpawnTime < 1 && !spawningEnemy)
            {
                spawningEnemy = true;
                spawnThing(4);
                spawnThing(5);
            }

            if (timePassed % enemySpawnTime > 1 && spawningEnemy)
            {
                spawningEnemy = false;
            }

            // Cory's player movement
            //g.drawPixmap(Assets.background, 0, 0);
            if (playerDir > 3) playerDir -= 4;
            if (playerDir < 0) playerDir += 4;
            switch (playerDir) {
                case 0:
                    if (mapArray[playerX][playerY - 1] != 2 && mapArray[playerX][playerY - 1] != 10) {
                        playerY--;
                        Score += 1;
                        if(waypointArray[playerX][playerY]!=0){playerWaypoint();}
                    }

                    if (playerY < 1) playerY = 1;
                    tiltPixmap = Assets.tiltUp;
                    break;
                case 1:
                    if (mapArray[playerX + 1][playerY] != 2 && mapArray[playerX + 1][playerY] != 10) {
                        playerX++;
                        Score += 1;
                        if(waypointArray[playerX][playerY]!=0){playerWaypoint();}
                    }

                    if (playerX > mapArray.length - 2) playerX = mapArray.length - 2;
                    tiltPixmap = Assets.tiltRight;
                    break;
                case 2:
                    if (mapArray[playerX][playerY + 1] != 2 && mapArray[playerX][playerY + 1] != 10){
                        playerY++;
                        Score += 1;
                        if(waypointArray[playerX][playerY]!=0){playerWaypoint();}
                    }
                    if (playerY > mapArray.length - 2) playerY = mapArray.length - 2;
                    tiltPixmap = Assets.tiltDown;
                    break;
                case 3:
                    if (mapArray[playerX - 1][playerY] != 2 && mapArray[playerX - 1][playerY] != 10) {
                        playerX--;
                        Score += 1;
                        if(waypointArray[playerX][playerY]!=0){playerWaypoint();}
                    }
                    if (playerX < 1) playerX = 1;
                    tiltPixmap = Assets.tiltLeft;
                    break;
            }
            previousDir = playerDir;

            //update Enemies
            for (int j = 0; j<mapArray[0].length; j++) {
                for (int i = 0; i < mapArray.length; i++) {
                    if(mapArray[j][i]==4){mapArray[j][i]=0;}
                }
            }
            for(int i=0;i<enemies.size();i++){

                if(enemies.get(i)!=null){
                    enemies.get(i).Update();

                    mapArray[enemies.get(i).getX()][enemies.get(i).getY()]=4;
                    if (enemies.get(i).killPlayer(playerX, playerY))
                    {
                        SaveScore();
                        game.setScreen(new HighscoreScreen(game));
                    }
                }
            }
        }
        /*world.update(deltaTime);
        if(world.gameOver) {
            if(Settings.soundEnabled)
               //Assets.bitten.play(1);
            state = GameState.GameOver;
        }
        if(oldScore != world.score) {
            oldScore = world.score;
            score = "" + oldScore;
            if(Settings.soundEnabled)
                Assets.eat.play(1);
        }*/
    }

    // This will create something new on the map.
    private void spawnThing (int thing)
    {
        boolean spawned = false;
        Random randomGenerator = new Random();
        int xPos = 0, yPos = 0;

        do
        {
            xPos = randomGenerator.nextInt( mapArray.length - 5) + 2;
            yPos = randomGenerator.nextInt( mapArray.length - 5) + 2;

            // Checks if it's empty.
            if (mapArray[xPos][yPos] == 0 || mapArray[xPos][yPos] == 1)
            {
                mapArray[xPos][yPos] = thing;
                //mapArray[playerX + 1][playerY + 1] = thing;
                if(thing==4)
                {
                    System.out.println("Spawned an enemy.");
                    enemies.add(new Enemy(xPos,yPos,this));
                }

                if(thing==5)
                {
                    System.out.println("Spawned a Pickup.");
                    Pickups.add(new Powerup(xPos, yPos, this));
                    Pickups.get(Pickups.size()-1).PUpdate();
                }


                spawned = true;
            }

        }

        while (!spawned);
    }

    // The 2D array we're storing map data on, the number of walls to make, the maximum wall length.
    private void generateMap (int wallNumber, int wallMaxLength)
    {
        /* The mapArray will record what tiles are placed where on the map, in the form of numbers.
        0 is open space, 1 is open space adjacent to a wall or player(diagonally as well), 2 is a wall,
        3 is a player, 4 is an enemy, 5 is a power up pickup, 6 is a used shuriken, 7 is used caltrops,
        8 is used smoke bomb, 10 is edge of the map.*/

        Random randomGenerator = new Random();

        // randomX and randomY are used to pick a random co-ordinate on mapArray
        // wallDirection and wallLength are used to pick a random direction and length for spawned walls.
        // wallTiles record the amount of wall tiles placed.
        int randomX = 0, randomY = 0, wallDirection = 0, wallLength = 0, wallTiles = 0;

        int x = 0, y = 0;

        // This creates the edge of the map, and places the player.

        mapArray[(mapArray.length / 2) - 1][(mapArray.length / 2) - 1] = 1;
        mapArray[(mapArray.length / 2)][(mapArray.length / 2) - 1] = 1;
        mapArray[(mapArray.length / 2) + 1][(mapArray.length / 2) - 1] = 1;

        mapArray[(mapArray.length / 2) - 1][(mapArray.length / 2)] = 1;
        mapArray[(mapArray.length / 2)][(mapArray.length / 2)] = 3;
        mapArray[(mapArray.length / 2) + 1][(mapArray.length / 2)] = 1;

        mapArray[(mapArray.length / 2) - 1][(mapArray.length / 2) + 1] = 1;
        mapArray[(mapArray.length / 2)][(mapArray.length / 2) + 1] = 1;
        mapArray[(mapArray.length / 2) + 1][(mapArray.length / 2) + 1] = 1;

        for (x = 0; x < mapArray.length; x++)
        {
            for (y = 0; y < mapArray.length; y++)
            {
                // Placing border.
                if (y == 0 || y == mapArray.length - 1)
                {
                    mapArray[x][y] = 10;
                }
            }

            y --;
            // Placing border.
            if (x == 0 || x == mapArray.length - 1)
            {
                for (int z = 0; z < mapArray.length - 1; z++)
                {
                    mapArray[x][z] = 10;
                }
            }
        }

        // This adds walls. It will keep doing so until either the correct number of walls have been placed,
        // or 1 third of the map is wall tile.
        do
        {
            // These coordinates will be at least 1 space away from the borders.
            randomX = randomGenerator.nextInt( mapArray.length - 5) + 2;
            randomY = randomGenerator.nextInt( mapArray.length - 5) + 2;

            // Check if the randomly generated coordinate is an open space.
            if (mapArray[randomX][randomY] == 0)
            {
                // We've used one of our walls, and made 1 wall tile.
                wallNumber --;
                wallTiles ++;
                mapArray[randomX][randomY] = 2;

                // Generate a random direction of left (0), right (1), down (2), or up (3).
                wallDirection = randomGenerator.nextInt(4);
                wallLength = randomGenerator.nextInt(wallMaxLength - 2) + 2;

                // Left
                if (wallDirection == 0)
                {
                    // Change the tiles adjacent to our start point to 1, or "wall adjacent"
                    // We aren't changing the tiles in the direction we're going. So in this instance,
                    // we're changing the tile above us, below us, and all 3 tiles to the right of us.
                    mapArray[randomX + 1][randomY - 1] = 1;
                    mapArray[randomX + 1][randomY] = 1;
                    mapArray[randomX + 1][randomY + 1] = 1;

                    mapArray[randomX][randomY - 1] = 1;
                    mapArray[randomX][randomY + 1] = 1;
                }

                // Right
                else if (wallDirection == 1)
                {
                    mapArray[randomX - 1][randomY - 1] = 1;
                    mapArray[randomX - 1][randomY] = 1;
                    mapArray[randomX - 1][randomY + 1] = 1;

                    mapArray[randomX][randomY - 1] = 1;
                    mapArray[randomX][randomY + 1] = 1;
                }

                // Down
                else if (wallDirection == 2)
                {
                    mapArray[randomX - 1][randomY + 1] = 1;
                    mapArray[randomX][randomY + 1] = 1;
                    mapArray[randomX + 1][randomY + 1] = 1;

                    mapArray[randomX - 1][randomY] = 1;
                    mapArray[randomX + 1][randomY] = 1;
                }

                // Up
                else
                {
                    mapArray[randomX - 1][randomY - 1] = 1;
                    mapArray[randomX][randomY - 1] = 1;
                    mapArray[randomX + 1][randomY - 1] = 1;

                    mapArray[randomX - 1][randomY] = 1;
                    mapArray[randomX + 1][randomY] = 1;
                }

                // This will place walls in the desired direction, for the desired length,
                // unless we run into an obstacle, in which case we stop.
                for (x = 1; x < wallLength; x++)
                {
                    // Left
                    if (wallDirection == 0)
                    {
                        // Check if the next tile is open.
                        if (mapArray[randomX - x][randomY] == 0)
                        {
                            // Check if this is the last wall tile in this wall. If so, change all adjacent tiles.
                            if (x == wallLength - 1)
                            {
                                // Place the next wall tile, change the adjacent tiles, increment wallTiles.
                                wallTiles ++;
                                mapArray[randomX - x][randomY + 1] = 1;
                                mapArray[randomX - x][randomY] = 2;
                                mapArray[randomX - x][randomY - 1] = 1;

                                mapArray[randomX - x - 1][randomY + 1] = 1;
                                mapArray[randomX - x - 1][randomY] = 1;
                                mapArray[randomX - x - 1][randomY - 1] = 1;
                            }

                            else
                            {
                                // Place the next wall tile, change the adjacent tiles not in our way, increment wallTiles.
                                wallTiles ++;
                                mapArray[randomX - x][randomY + 1] = 1;
                                mapArray[randomX - x][randomY] = 2;
                                mapArray[randomX - x][randomY - 1] = 1;
                            }
                        }

                        // We've found an obstacle, wall terminated.
                        else
                        {
                            break;
                        }
                    }

                    // Right
                    else if (wallDirection == 1)
                    {
                        // Check if the next tile is open.
                        if (mapArray[randomX + x][randomY] == 0)
                        {
                            // Check if this is the last wall tile in this wall. If so, change all adjacent tiles.
                            if (x == wallLength - 1)
                            {
                                // Place the next wall tile, change the adjacent tiles, increment wallTiles.
                                wallTiles ++;
                                mapArray[randomX + x][randomY + 1] = 1;
                                mapArray[randomX + x][randomY] = 2;
                                mapArray[randomX + x][randomY - 1] = 1;

                                mapArray[randomX + x + 1][randomY + 1] = 1;
                                mapArray[randomX + x + 1][randomY] = 1;
                                mapArray[randomX + x + 1][randomY - 1] = 1;
                            }

                            else
                            {
                                // Place the next wall tile, change the adjacent tiles not in our way, increment wallTiles.
                                wallTiles ++;
                                mapArray[randomX + x][randomY + 1] = 1;
                                mapArray[randomX + x][randomY] = 2;
                                mapArray[randomX + x][randomY - 1] = 1;
                            }
                        }

                        // We've found an obstacle, wall terminated.
                        else
                        {
                            break;
                        }
                    }

                    // Down
                    else if (wallDirection == 2)
                    {
                        // Check if the next tile is open.
                        if (mapArray[randomX][randomY - x] == 0)
                        {
                            // Check if this is the last wall tile in this wall. If so, change all adjacent tiles.
                            if (x == wallLength - 1)
                            {
                                // Place the next wall tile, change the adjacent tiles, increment wallTiles.
                                wallTiles ++;
                                mapArray[randomX - 1][randomY - x] = 1;
                                mapArray[randomX][randomY - x] = 2;
                                mapArray[randomX + 1][randomY - x] = 1;

                                mapArray[randomX- 1][randomY - x - 1] = 1;
                                mapArray[randomX][randomY - x - 1] = 1;
                                mapArray[randomX + 1][randomY - x - 1] = 1;
                            }

                            else
                            {
                                // Place the next wall tile, change the adjacent tiles not in our way, increment wallTiles.
                                wallTiles ++;
                                mapArray[randomX - 1][randomY -x] = 1;
                                mapArray[randomX][randomY - x] = 2;
                                mapArray[randomX + 1][randomY - x] = 1;
                            }
                        }

                        // We've found an obstacle, wall terminated.
                        else
                        {
                            break;
                        }
                    }

                    // Up
                    else
                    {
                        // Check if the next tile is open.
                        if (mapArray[randomX][randomY + x] == 0)
                        {
                            // Check if this is the last wall tile in this wall. If so, change all adjacent tiles.
                            if (x == wallLength - 1)
                            {
                                // Place the next wall tile, change the adjacent tiles, increment wallTiles.
                                wallTiles ++;
                                mapArray[randomX - 1][randomY + x] = 1;
                                mapArray[randomX][randomY + x] = 2;
                                mapArray[randomX + 1][randomY + x] = 1;

                                mapArray[randomX- 1][randomY + x + 1] = 1;
                                mapArray[randomX][randomY + x + 1] = 1;
                                mapArray[randomX + 1][randomY + x + 1] = 1;
                            }

                            else
                            {
                                // Place the next wall tile, change the adjacent tiles not in our way, increment wallTiles.
                                wallTiles ++;
                                mapArray[randomX - 1][randomY + x] = 1;
                                mapArray[randomX][randomY + x] = 2;
                                mapArray[randomX + 1][randomY + x] = 1;
                            }
                        }

                        // We've found an obstacle, wall terminated.
                        else
                        {
                            break;
                        }
                    }

                }
            }

            else
            {

            }
        } while (wallTiles < (mapArray.length * mapArray.length) / 3 && wallNumber != 0);
    }

    private void updatePaused(List<TouchEvent> touchEvents)
    {
        int len = touchEvents.size();
        for(int i = 0; i < len; i++)
        {
            TouchEvent event = touchEvents.get(i);
            if(event.type == TouchEvent.TOUCH_UP)
            {
                if(event.x > ((g.getWidth() / 2) - 50.f)  && event.x <= ((g.getWidth() / 2 ) + 50.0f))
                {
                    if(event.y > ((g.getHeight() / 2) + Assets.resumeButton.getHeight() - 30.f) && event.y <= ((g.getHeight() / 2) + Assets.resumeButton.getHeight()) + 1) {
                        if(Settings.soundEnabled)
                            Assets.click.play(1);
                        state = GameState.Running;
                        return;
                    }
                    if(event.y > ((g.getHeight() / 2) + Assets.quitButton.getHeight() - 5.f) && event.y <= ((g.getHeight() / 2) + Assets.quitButton.getHeight() + 50)) {
                        if(Settings.soundEnabled)
                            Assets.click.play(1);
                        game.setScreen(new MainMenuScreen(game));
                        return;
                    }
                }
            }
        }
    }

    private void updateGameOver(List<TouchEvent> touchEvents) {
        int len = touchEvents.size();
        for(int i = 0; i < len; i++) {
            TouchEvent event = touchEvents.get(i);
            if(event.type == TouchEvent.TOUCH_UP) {
                if(event.x >= 128 && event.x <= 192 &&
                        event.y >= 200 && event.y <= 264) {
                    if(Settings.soundEnabled)
                        Assets.click.play(1);
                    game.setScreen(new MainMenuScreen(game));
                    return;
                }
            }
        }
    }


    @Override
    public void present(float deltaTime) {
        Graphics g = game.getGraphics();



        if (drawAgain > 0){
            drawMap();
            drawAgain = 0;
        }
        //drawWorld(world);
        //if(state == GameState.Ready)
        //drawReadyUI();
        if(state == GameState.Running)
            drawRunningUI();
        if(state == GameState.Paused)
            drawPausedUI();
        if(state == GameState.GameOver)
            drawGameOverUI();



        /*drawText(g, score,
                g.getWidth() / 2 - score.length()*20 / 2,
                g.getHeight() - 42);*/
    }

    /*private void drawWorld(World world) {
        Graphics g = game.getGraphics();
        Snake snake = world.snake;
        SnakePart head = snake.parts.get(0);
        Stain stain = world.stain;


        Pixmap stainPixmap = null;
        if(stain.type == Stain.TYPE_1)
            stainPixmap = Assets.stain1;
        if(stain.type == Stain.TYPE_2)
            stainPixmap = Assets.stain2;
        if(stain.type == Stain.TYPE_3)
            stainPixmap = Assets.stain3;
        int x = stain.x * 32;
        int y = stain.y * 32;
        g.drawPixmap(stainPixmap, x, y);

        int len = snake.parts.size();
        for(int i = 1; i < len; i++) {
            SnakePart part = snake.parts.get(i);
            x = part.x * 32;
            y = part.y * 32;
            g.drawPixmap(Assets.tail, x, y);
        }

        Pixmap headPixmap = null;
        if(snake.direction == Snake.UP)
            headPixmap = Assets.headUp;
        if(snake.direction == Snake.LEFT)
            headPixmap = Assets.headLeft;
        if(snake.direction == Snake.DOWN)
            headPixmap = Assets.headDown;
        if(snake.direction == Snake.RIGHT)
            headPixmap = Assets.headRight;
        x = head.x * 32 + 16;
        y = head.y * 32 + 16;
        g.drawPixmap(headPixmap,
                x - headPixmap.getWidth() / 2,
                y - headPixmap.getHeight() / 2);
    }

    private void drawReadyUI() {
        Graphics g = game.getGraphics();

        g.drawPixmap(Assets.ready, 47, 100);
        g.drawLine(0, 416, 480, 416, Color.BLACK);
    }*/

    private void drawRunningUI() {
        Graphics g = game.getGraphics();

        g.drawPixmap(Assets.pauseIcon, 0, 0, 0, 0, 64, 64);
        g.drawLine(0, 655, 480, 655, Color.BLACK);//g.drawLine(0, 416, 480, 416, Color.BLACK);

        g.drawPixmap(Assets.smokeDrop, 1 + g.getWidth() - Assets.smokeDrop.getWidth(), 0, 0, 0, 64, 64);

        g.drawPixmap(Assets.arrowLeftIcon, 0, g.getHeight() - 64, -1.0f, 1.0f);
        g.drawPixmap(Assets.arrowRightIcon, 1 + g.getWidth() - Assets.arrowRightIcon.getWidth(), g.getHeight() - 64, 0, 0, 64, 64);

        //Draws the score
        String ScoreText = Integer.toString(Score);
        drawText(g, ScoreText, g.getWidth() / 2 - 10, g.getHeight() / 2 + 300);
    }

    private void drawPausedUI() {
        Graphics g = game.getGraphics();

        g.drawPixmap(Assets.pauseTitle, ((g.getWidth() - Assets.pauseTitle.getWidth()) / 2), g.getHeight() / 4, 0, 0, 192, 42);

        g.drawLine(0, 655, 480, 655, Color.BLACK);

        g.drawPixmap(Assets.resumeButton, ((g.getWidth() - Assets.resumeButton.getWidth()) / 2), g.getHeight() / 2, 0, 0, 192, 42);
        g.drawPixmap(Assets.quitButton, ((g.getWidth() - Assets.quitButton.getWidth()) / 2), ((g.getHeight() / 2) + Assets.quitButton.getHeight()), 0, 0, 100, 42);
    }

    private void drawGameOverUI() {
        Graphics g = game.getGraphics();

        g.drawPixmap(Assets.gameOverTitle, 62, 100);
        g.drawPixmap(Assets.quitButton, 128, 200, 0, 128, 64, 64);
        g.drawLine(0, 655, 480, 655, Color.BLACK);
    }

    public void drawText(Graphics g, String line, int x, int y) {
        int len = line.length();
        for (int i = 0; i < len; i++) {
            char character = line.charAt(i);

            if (character == ' ') {
                x += 20;
                continue;
            }

            int srcX = 0;
            int srcWidth = 0;
            if (character == '.') {
                srcX = 200;
                srcWidth = 10;
            } else {
                srcX = (character - '0') * 20;
                srcWidth = 20;
            }

            g.drawPixmap(Assets.numbers, x, y, srcX, 0, srcWidth, 32);
            x += srcWidth;
        }
    }

    @Override
    public void pause() {
        if(state == GameState.Running)
            state = GameState.Paused;

        /*if(world.gameOver) {
            Settings.addScore(world.score);
            Settings.save(game.getFileIO());
        }*/
    }

    @Override
    public void resume() {

    }

    @Override
    public void dispose() {

    }

    // Saves and adds the current score to the highscore file
    //Call this when the player dies.
    public void SaveScore()
    {
        //Score *= KilledEnemies;
        Settings.addScore(Score);
    }

    public int getSpace(int checkX, int checkY){
        if (checkX >= 0 && checkX <= 42 && checkY >= 0 && checkY <= 42) {
            return mapArray[checkX][checkY];
        } else return -1;
    }
    public int getWaypoint(int checkX, int checkY) {
        if (checkX >= 0 && checkX <= 42 && checkY >= 0 && checkY <= 42) {
            return waypointArray[checkX][checkY];
        } else return -1;
    }
    public void clearWaypoint(int checkX, int checkY) {
        if (checkX >= 0 && checkX <= 42 && checkY >= 0 && checkY <= 42) {
            waypointArray[checkX][checkY] = 0;
        }
    }

    public Point getPlayerPos(){
        return new Point(playerX,playerY);
    }

public void playerWaypoint(){
    waypointArray[playerX][playerY]=playerDir+1;
}
}