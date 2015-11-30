package com.filip.unstoppableninja;

import com.filip.androidgames.framework.Game;
import com.filip.androidgames.framework.Graphics;
import com.filip.androidgames.framework.Input.TouchEvent;
import com.filip.androidgames.framework.Screen;

import java.util.List;

public class MainMenuScreen extends Screen {
    public MainMenuScreen(Game game) {
        super(game);
    }

    @Override
    public void update(float deltaTime) {
        Graphics g = game.getGraphics();
        List<TouchEvent> touchEvents = game.getInput().getTouchEvents();

        int len = touchEvents.size();
        for(int i = 0; i < len; i++) {
            TouchEvent event = touchEvents.get(i);
            if(event.type == TouchEvent.TOUCH_UP) {

                // Toggle sound
                if(inBounds(event, 0, g.getHeight() - 64, 64, 64)) {
                    Settings.soundEnabled = !Settings.soundEnabled;
                    if(Settings.soundEnabled)
                        Assets.click.play(1);
                }

                // New Game
                if(inBounds(event,  g.getWidth() / 2 - 96, g.getHeight() / 2 - 50, 192, 42) ) {
                    game.setScreen(new GameScreen(game));
                    if(Settings.soundEnabled)
                        Assets.click.play(1);
                    return;
                }

                // Highscore
                if(inBounds(event, g.getWidth() / 2 - 96, g.getHeight() / 2 + 50, 192, 42) ) {
                    game.setScreen(new HighscoreScreen(game));
                    if(Settings.soundEnabled)
                        Assets.click.play(1);
                    return;
                }
            }
        }
    }

    @Override
    public void present(float deltaTime) {
        Graphics g = game.getGraphics();
        g.drawPixmap(Assets.background, 0, 0);
        g.drawPixmap(Assets.mainMenuTitle,  g.getWidth() / 2 - 150, g.getHeight() / 2 - 150);

        g.drawPixmap(Assets.highScoreButton,g.getWidth() / 2 - 96, g.getHeight() / 2 + 50);
        //g.drawPixmap(Assets.highScoreButton, g.getWidth() / 2 - 96, g.getHeight() / 2 + 50, 0, 0,  192, 42);

        g.drawPixmap(Assets.startButton, g.getWidth() / 2 - 96, g.getHeight() / 2 - 50, 0, 0, 192, 42);

        if(Settings.soundEnabled)
            g.drawPixmap(Assets.soundOn, 0, 1 + g.getHeight() - Assets.soundOn.getHeight() , 0, 0, 64, 64);
        else
            g.drawPixmap(Assets.soundOff, 0, 1 + g.getHeight() - Assets.soundOn.getHeight(), 0, 0, 64, 64);
    }

    @Override
    public void pause() {
        Settings.save(game.getFileIO());
    }

    @Override
    public void resume() {

    }

    @Override
    public void dispose() {

    }
}
