using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Player_movement
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static Texture2D rect;

        private int x = 10;
        private int y = 10;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //_graphics.PreferredBackBufferWidth = 1000;
            //_graphics.PreferredBackBufferHeight = 1000;
            //_graphics.ApplyChanges();

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
            x++;
            y++;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            while (x >= 0 && y >= 0 && x <= 1000 && y <= 1000)
            {
                _spriteBatch.Draw(rect, new Rectangle(x, y, 10, 10), Color.Chocolate);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
