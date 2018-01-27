using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AsteroidAssault__MonoGame_CSharp_
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameStates { TitleScreen, Playing, PlayerDead, GameOver };
        GameStates gameState = GameStates.TitleScreen;
        Texture2D titleScreen;
        Texture2D spriteSheet;
        StarField starField;
        AsteroidManager asteroidManager;
        PlayerManager playerManager;
        EnemyManager enemyManager;
        ExplosionManager explosionManager;
        CollisionManager collisionManager;
        SpriteFont pericles14;

        private float playerDeathDelayTime = 3f;
        private float playerDeathTimer = 0f;
        private float titleScreenTimer = 0f;
        private float titleScreenDelayTime = 1f;

        private int playerStartingLives = 2;
        private Vector2 playerStartLocation = new Vector2(390, 550);
        private Vector2 scoreLocation = new Vector2(20, 10);
        private Vector2 livesLocation = new Vector2(20, 25);
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600
            };
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            titleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen");
            spriteSheet = Content.Load<Texture2D>(@"Textures\spriteSheet");
            starField = new StarField(Window.ClientBounds.Width, Window.ClientBounds.Height,
                200, new Vector2(0, 30), spriteSheet, new Rectangle(0, 450, 2, 2));
            asteroidManager = new AsteroidManager(10, spriteSheet, new Rectangle(0, 0, 50, 50),
                20, Window.ClientBounds.Width, Window.ClientBounds.Height);
            playerManager = new PlayerManager(spriteSheet, new Rectangle(0, 150, 50, 50), 3,
                new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height));
            enemyManager = new EnemyManager(spriteSheet, new Rectangle(0, 200, 50, 50), 6,
                playerManager, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height));
            explosionManager = new ExplosionManager(spriteSheet, new Rectangle(0, 100, 50, 50),
                3, new Rectangle(0, 450, 2, 2));
            collisionManager = new CollisionManager(asteroidManager, playerManager,
                enemyManager, explosionManager);
            SoundManager.Initialize(Content);
            pericles14 = Content.Load<SpriteFont>(@"Fonts\Pericles14");
        }

        protected override void UnloadContent(){}

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState)
            {
                case GameStates.TitleScreen:
                    titleScreenTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (titleScreenTimer >= titleScreenDelayTime)
                    {
                        if ((Keyboard.GetState().IsKeyDown(Keys.Space)) ||
                            (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed))
                        {
                            playerManager.LivesRemaining = playerStartingLives;
                            playerManager.PlayerScore = 0;
                            ResetGame();
                            gameState = GameStates.Playing;
                        }
                    }
                    break;
                case GameStates.Playing:
                    starField.Update(gameTime);
                    asteroidManager.Update(gameTime);
                    playerManager.Update(gameTime);
                    enemyManager.Update(gameTime);
                    explosionManager.Update(gameTime);
                    collisionManager.CheckCollisions();

                    if (playerManager.Destroyed)
                    {
                        playerDeathTimer = 0f;
                        enemyManager.Active = false;
                        playerManager.LivesRemaining--;
                        if (playerManager.LivesRemaining < 0)
                        {
                            gameState = GameStates.GameOver;
                        }
                        else
                        {
                            gameState = GameStates.PlayerDead;
                        }
                    }
                    break;
                case GameStates.PlayerDead:
                    playerDeathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    starField.Update(gameTime);
                    asteroidManager.Update(gameTime);
                    enemyManager.Update(gameTime);
                    playerManager.PlayerShotManager.Update(gameTime);
                    explosionManager.Update(gameTime);
                    if (playerDeathTimer >= playerDeathDelayTime)
                    {
                        ResetGame();
                        gameState = GameStates.Playing;
                    }
                    break;
                case GameStates.GameOver:
                    playerDeathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    starField.Update(gameTime);
                    asteroidManager.Update(gameTime);
                    enemyManager.Update(gameTime);
                    playerManager.PlayerShotManager.Update(gameTime);
                    explosionManager.Update(gameTime);
                    if (playerDeathTimer >= playerDeathDelayTime)
                    {
                        gameState = GameStates.TitleScreen;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (gameState == GameStates.TitleScreen)
            {
                spriteBatch.Draw(titleScreen, 
                    new Rectangle(0, 0, Window.ClientBounds.Width, 
                    Window.ClientBounds.Height), Color.White);
            }

            if ((gameState == GameStates.Playing) ||
                (gameState == GameStates.PlayerDead) ||
                (gameState == GameStates.GameOver))
            {
                starField.Draw(spriteBatch);
                asteroidManager.Draw(spriteBatch);
                playerManager.Draw(spriteBatch);
                enemyManager.Draw(spriteBatch);
                explosionManager.Draw(spriteBatch);

                spriteBatch.DrawString(pericles14, "Score: " +
                    playerManager.PlayerScore.ToString(), scoreLocation, Color.Yellow);

                if (playerManager.LivesRemaining >= 0)
                {
                    spriteBatch.DrawString(pericles14, "Lives: " +
                        playerManager.LivesRemaining.ToString(), livesLocation, Color.Yellow);
                }
            }

            if (gameState == GameStates.GameOver)
            {
                spriteBatch.DrawString(pericles14, "G A M E   O V E R",
                    new Vector2(Window.ClientBounds.Width / 2 -
                    pericles14.MeasureString("G A M E   O V E R").X / 2, 50), Color.Red);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame()
        {
            playerManager.playerSprite.Location = playerStartLocation;
            foreach (Sprite asteroid in asteroidManager.Asteroids)
            {
                asteroid.Location = new Vector2(-500, -500);
            }
            enemyManager.Enemies.Clear();
            enemyManager.Active = true;
            enemyManager.EnemyShotManager.Shots.Clear();
            playerManager.PlayerShotManager.Shots.Clear();
            playerManager.Destroyed = false;
        }
    }
}