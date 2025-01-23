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

        private int _smoothness = 1;

        private int smooth_average = 0;

        private int _width = 50;
        private int _height = 50;

        private int _pixelWidth = 10;
        private int _pixelHeight = 10;

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
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    grid[i, j] = rng.Next(5) * 50;
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
                                }
                            }
                        }
                        smooth_average = smooth_average / 9;
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
                    _spriteBatch.Draw(rect, new Rectangle(i * (_pixelWidth), j * (_pixelHeight), _pixelWidth, _pixelHeight), new Color(0, grid[i, j], 0));
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}