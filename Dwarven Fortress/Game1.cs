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

        List<int> tect_point_width;
        List<int> tect_point_height;

        private int _smoothness = 3;

        private int smooth_average = 0;

        private int _width = 250;
        private int _height = 125;

        private int _pixelWidth = 8;
        private int _pixelHeight = 8;

        private int _peaks = 165;
        private int _mountains = 155;
        private int _forest = 130;
        private int _plains = 115;
        private int _sands = 110;

        private int tect_points = 450;

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
            int border_sea_width;
            int border_sea_height;

            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = _width * (_pixelWidth);
            _graphics.PreferredBackBufferHeight = _height * (_pixelWidth);
            _graphics.ApplyChanges();

            grid = new int[_width, _height];
            grid_buffer = new int[_width, _height];

            R = new int[_width, _height];
            G = new int[_width, _height];
            B = new int[_width, _height];

            border_sea_height = _height / 12;
            border_sea_width = _width / 15;

            tect_point_width = new List<int>();
            tect_point_height = new List<int>();

            for (int x = 0; x < tect_points; x++)
            {
                tect_point_width.Add(rng.Next(_width));
                tect_point_height.Add(rng.Next(_height));
            }

            // create gid
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (i >= 0 && i < border_sea_width || j >= 0 && j < border_sea_height || i <= _width && i > _width - border_sea_width || j <= _height && j > _height - border_sea_height)
                    {
                        grid[i, j] = rng.Next(100, _sands);
                    }
                    else
                    {
                        grid[i, j] = rng.Next(255);
                    }
                }
            }

            // create mountains
            for (int i = 0; i < tect_point_height.Count; i++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    for (int l = -1; l <= 1; l++)
                    {
                        int nk = tect_point_width[i] + k;
                        int nl = tect_point_height[i] + l;
                        if (nk >= 0 && nk < _width && nl >= 0 && nl < _height)
                        {
                            grid[nk, nl] = rng.Next(_mountains - 10, 255);
                        }
                    }
                }
            }

            // smooth grid
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

            // color grid
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if( grid[i, j] > 255 )
                    {
                        R[i, j] = 245;
                        G[i, j] = 0;
                        B[i, j] = 0;
                    }
                    else if (grid[i, j] >= _peaks)
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

            // draw grid
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