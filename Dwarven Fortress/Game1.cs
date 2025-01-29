using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dwarven_Fortress
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static Texture2D rect;

        int[,] grid;
        int[,] grid_buffer;

        Random rng;

        int[,] R;
        int[,] G;
        int[,] B;


        private int _smoothness = 2;

        private int smooth_average = 0;

        private int _width = 200;
        private int _height = 100;

        private int _pixelWidth = 10;
        private int _pixelHeight = 10;

        private int _peaks = 175;
        private int _mountains = 160;
        private int _forest = 130;
        private int _plains = 115;
        private int _sands = 110;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            rng = new Random(); 
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);
            int num_neighbours;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = _width * (_pixelWidth);
            _graphics.PreferredBackBufferHeight = _height * (_pixelWidth);
            _graphics.ApplyChanges();

            grid = new int[_width, _height];
            grid_buffer = new int[_width, _height];

            R = new int[_width, _height];
            G = new int[_width, _height];
            B = new int[_width, _height];

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    grid[i, j] = rng.Next(255);
                }
            }

            for (int x = 0; x < _smoothness; x++)
            {
                for (int i = 0; i < _width; i++)
                {
                    for (int j = 0; j < _height; j++)
                    {
                        smooth_average = 0;
                        num_neighbours = 0;

                        for (int di = -1; di <= 1; di++)
                        {
                            for (int dj = -1; dj <= 1; dj++)
                            {
                                int ni = i + di;
                                int nj = j + dj;
                                if (ni >= 0 && ni < _width && nj >= 0 && nj < _height)
                                {
                                    smooth_average = smooth_average + grid[ni, nj];
                                    num_neighbours++;
                                }
                            }
                        }
                        smooth_average = smooth_average / num_neighbours;
                        grid_buffer[i, j] = smooth_average;
                    }
                }
                for (int i = 0; i < _width; i++)
                {
                    for (int j = 0; j < _height; j++)
                    {
                        grid[i, j] = grid_buffer[i, j];
                    }
                }
            }
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (grid[i, j] >= _peaks)
                    {
                        R[i, j] = 250;
                        G[i, j] = 250;
                        B[i, j] = 250;
                    }
                    else if (grid[i, j] >= _mountains)
                    {
                        R[i, j] = 137;
                        G[i, j] = 137;
                        B[i, j] = 137;
                    }
                    else if (grid[i, j] >= _forest)
                    {
                        R[i, j] = 0;
                        G[i, j] = 128;
                        B[i, j] = 0;
                    }
                    else if (grid[i, j] >= _plains)
                    {
                        R[i, j] = 5;
                        G[i, j] = 159;
                        B[i, j] = 4;
                    }
                    else if (grid[i, j] >= _sands)
                    {
                        R[i, j] = 194;
                        G[i, j] = 178;
                        B[i, j] = 128;
                    }
                    else
                    {
                        R[i, j] = 29;
                        G[i, j] = 29;
                        B[i, j] = 130;
                    }
                }
            }

                    base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new Color[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _spriteBatch.Draw(rect, new Rectangle(i * (_pixelWidth), j * (_pixelHeight), _pixelWidth, _pixelHeight), new Color(R[i, j], G[i, j], B[i, j]));
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}