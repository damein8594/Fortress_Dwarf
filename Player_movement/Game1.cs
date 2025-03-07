using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Transactions;

namespace Player_movement
{

    public class Game1 : Game
    {

        private Point GetMousePosition()
        {
            MouseState mouseState = Mouse.GetState();
            return new Point(mouseState.X, mouseState.Y);
        }


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static Texture2D rect;

        Random rng;

        int[,] grid;
        int[,] grid_buffer;

        private int _peaks = 165;

        private int tect_points = 450;


        private int screen_width = 1000;
        private int screen_height = 1000;

        private int pixelWidth = 10;
        private int pixelHeight = 10;

        private int width = 250;
        private int height = 125;

        private int x = 0;
        private int y = 0;

        private int xdirection = 1;
        private int ydirection = 1;

        private int speed = 10;

        private int mousex = 0;
        private int mousey = 0;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = width * (pixelWidth);
            _graphics.PreferredBackBufferHeight = height * (pixelWidth);
            _graphics.ApplyChanges();

            grid = new int[width, height];
            grid_buffer = new int[width, height];

            base.Initialize();
        }



        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new Color[] { Color.Chocolate });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouse = Mouse.GetState(); // Add this line to get the current mouse state
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                
                Point mousePosition = GetMousePosition();
                mousex = mousePosition.X;
                mousey = mousePosition.Y;
            }

            //if (x < mousex) { xdirection = 1; }
            //if (x == mousex) { xdirection = 0; }
            //if (x > mousex) { xdirection = -1; }

            //if (y < mousey) { ydirection = 1; }
            //if (y == mousey) { ydirection = 0; }
            //if (y > mousey) { ydirection = -1; }

            //x = x + (xdirection * speed);
            //y = y + (ydirection * speed);

            
            if (x < mousex) { x += speed; }
            if (x == mousex) { x += 0; }
            if (x > mousex) { x -= speed; }

            if (y < mousey) { y += speed; }
            if (y == mousey) { y += speed; }
            if (y > mousey) { y -= speed; }


            //if (x == screen_width - pixel_width){xdirection =-1;}
            //if (y == screen_hight  - pixel_hight){ydirection = -1;}
            //if (x == 0) { xdirection = 1; }
            //if (y == 0) { ydirection = 1; }

            // TODO: Add your update logic here 

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();



            _spriteBatch.Draw(rect, new Rectangle(x, y, pixelWidth, pixelHeight), Color.Chocolate);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
