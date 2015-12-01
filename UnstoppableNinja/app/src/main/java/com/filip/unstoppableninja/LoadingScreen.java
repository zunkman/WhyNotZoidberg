package com.filip.unstoppableninja;

import com.filip.androidgames.framework.Game;
import com.filip.androidgames.framework.Graphics;
import com.filip.androidgames.framework.Graphics.PixmapFormat;
import com.filip.androidgames.framework.Screen;

public class LoadingScreen extends Screen {
    public LoadingScreen(Game game) {
        super(game);
    }

    @Override
    public void update(float deltaTime) {
        Graphics g = game.getGraphics();
        Assets.startButton = g.newPixmap("StartButton.png", PixmapFormat.ARGB4444);
        Assets.highScoreButton = g.newPixmap("HighScore.png", PixmapFormat.ARGB4444);

        Assets.soundOn = g.newPixmap("UnNin_SoundOn_Icon_64x.png", PixmapFormat.ARGB4444);
        Assets.soundOff = g.newPixmap("UnNin_SoundOff_Icon_64x.png", PixmapFormat.ARGB4444);

        Assets.resumeButton = g.newPixmap("Resume.png", PixmapFormat.ARGB4444);
        Assets.background = g.newPixmap("UnNin-BG-C.png", PixmapFormat.RGB565);

        Assets.arrowRightIcon = g.newPixmap("UnNin_RightArrow_FX_64x.png", PixmapFormat.RGB565);
        Assets.arrowLeftIcon = g.newPixmap("UnNin_RightArrow_FX_64x.png", PixmapFormat.RGB565);

        Assets.mainMenuTitle = g.newPixmap("MainMenuTitle.png", PixmapFormat.ARGB4444);

        Assets.numbers = g.newPixmap("numbers.png", PixmapFormat.ARGB4444);
        Assets.pauseIcon = g.newPixmap("UnNin_Pause_Icon_64x.png", PixmapFormat.ARGB4444);
        Assets.pauseTitle = g.newPixmap("PauseTitle.png", PixmapFormat.ARGB4444);
        Assets.quitButton = g.newPixmap("QuitGame.png", PixmapFormat.ARGB4444);
        Assets.gameOverTitle = g.newPixmap("Penguin.png", PixmapFormat.ARGB4444);

        Assets.smokeDrop = g.newPixmap("UnNin_SmokeBomb_Pickup_64x.png", PixmapFormat.ARGB4444);
        Assets.shurikenDrop = g.newPixmap("UnNin_Shuriken_Pickup_64x.png", PixmapFormat.ARGB4444);
        Assets.caltropsDrop = g.newPixmap("UnNin_Caltrops_Pickup_64x.png", PixmapFormat.ARGB4444);

        Assets.caltrops = g.newPixmap("UnNin_Caltrops_FX_64x.png", PixmapFormat.ARGB4444);
        Assets.smoke = g.newPixmap("UnNin_SmokeBomb_FX_64x.png", PixmapFormat.ARGB4444);
        Assets.shuriken = g.newPixmap("UnNin_Shuriken_FX_64x.png", PixmapFormat.ARGB4444);

        Assets.floorTile = g.newPixmap("UnNin_FloorTile_64x.png", PixmapFormat.ARGB4444);
        Assets.wallTile = g.newPixmap("UnNin_WallTile_V2_64x.png", PixmapFormat.ARGB4444);

        Assets.enemyDown = g.newPixmap("UnNin_Enemy_Down_64x.png", PixmapFormat.ARGB4444);

        Assets.tiltUp = g.newPixmap("UnNin_Ninja_Up_64x.png", PixmapFormat.ARGB4444);
        Assets.tiltDown = g.newPixmap("UnNin_Ninja_Down_64x.png", PixmapFormat.ARGB4444);
        Assets.tiltLeft = g.newPixmap("UnNin_Ninja_Left_64x.png", PixmapFormat.ARGB4444);
        Assets.tiltRight = g.newPixmap("UnNin_Ninja_Right_64x.png", PixmapFormat.ARGB4444);


        Assets.click = game.getAudio().newSound("click.ogg");
        Settings.load(game.getFileIO());
        game.setScreen(new MainMenuScreen(game));
    }

    @Override
    public void present(float deltaTime) {

    }

    @Override
    public void pause() {

    }

    @Override
    public void resume() {

    }

    @Override
    public void dispose() {

    }
}