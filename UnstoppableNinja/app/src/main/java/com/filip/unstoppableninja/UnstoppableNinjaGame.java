package com.filip.unstoppableninja;

import com.filip.androidgames.framework.Screen;
import com.filip.androidgames.framework.impl.AndroidGame;

public class UnstoppableNinjaGame extends AndroidGame
{
    @Override
    public Screen getStartScreen() {
        return new LoadingScreen(this);
    }
}


