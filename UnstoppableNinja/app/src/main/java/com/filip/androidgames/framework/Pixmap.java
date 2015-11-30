package com.filip.androidgames.framework;

import com.filip.androidgames.framework.Graphics.PixmapFormat;

public interface Pixmap 
{
    public int getWidth();
    public int getHeight();
    public PixmapFormat getFormat();
    public void dispose();
}

