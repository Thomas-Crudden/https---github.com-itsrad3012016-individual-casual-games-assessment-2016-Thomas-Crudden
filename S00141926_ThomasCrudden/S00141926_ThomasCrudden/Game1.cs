using GameServerConsole;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace S00141926_ThomasCrudden
{
    public static class Helpers
    {
        public static GraphicsDevice GraphicsDevice { get; set; }
    }
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private NetPeerConfiguration ClientConfig;
        private NetClient client;
        private string InGameMessage = string.Empty;
        private SpriteFont font;
        Player thisPlayer;
        List<Player> OtherPlayers = new List<Player>();
        Dictionary<string, Texture2D> playerTextures = new Dictionary<string, Texture2D>();
        Texture2D textureCollectable;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            ClientConfig = new NetPeerConfiguration("s00143451");
            //for the client
            ClientConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            client = new NetClient(ClientConfig);
            client.Start();
            InGameMessage = "This Client has a unique id of " + client.UniqueIdentifier.ToString();
            // Note Named parameters for more readable code
            //client.Connect(host: "127.0.0.1", port: 12345);
            //search in local network at port 50001
            client.DiscoverLocalPeers(12345);
            Helpers.GraphicsDevice = GraphicsDevice;
            new GetGameInputComponent(this);
            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("keyboardfont");
            playerTextures = Loader.ContentLoad<Texture2D>(Content, @".\PlayerIcons\");
            textureCollectable = Content.Load<Texture2D>("Collectable\\Square");


        }


        protected override void UnloadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                NetOutgoingMessage sendMsg = client.CreateMessage();
                PlayerData playerLeaving = thisPlayer.PlayerDataPacket;
                playerLeaving.header = "leaving";
                string json = JsonConvert.SerializeObject(playerLeaving);

                Exit();


            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                InGameMessage = "Sending Message";
                NetOutgoingMessage sendMsg = client.CreateMessage();
                sendMsg.Write("Hello there from client at " + gameTime.TotalGameTime.ToString());
                client.SendMessage(sendMsg, NetDeliveryMethod.ReliableOrdered);
            }


            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, InGameMessage, new Vector2(10, 10), Color.White);
            if (thisPlayer != null)
                spriteBatch.Draw(playerTextures[thisPlayer.ImageName], thisPlayer.Position, Color.White);
            foreach (Player other in OtherPlayers)
                spriteBatch.Draw(playerTextures[other.ImageName], other.Position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CheckMessages()
        {
            NetIncomingMessage ServerMessage;
            if ((ServerMessage = client.ReadMessage()) != null)
            {
                switch (ServerMessage.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        string message = ServerMessage.ReadString();
                        //InGameMessage = "Data In " + message;
                        process(message);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        InGameMessage = ServerMessage.ReadString();
                        client.Connect(ServerMessage.SenderEndPoint);
                        InGameMessage = "Connected to " + ServerMessage.SenderEndPoint.Address.ToString();
                        if (thisPlayer == null)
                        {
                            string ImageName = "Badges_" + Utility.NextRandom(0, playerTextures.Count - 1);
                            thisPlayer = new Player(client, Guid.NewGuid(), ImageName,
                                          new Vector2(Utility.NextRandom(100, GraphicsDevice.Viewport.Width - 100),
                                                       Utility.NextRandom(100, GraphicsDevice.Viewport.Height - 100)));
                        }
                        if (InGameMessage == ClientConfig.AppIdentifier)
                        {
                            client.Connect(ServerMessage.SenderEndPoint);
                        }
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        switch (ServerMessage.SenderConnection.Status)
                        {
                            /* .. */
                        }
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        //InGameMessage = ServerMessage.ReadString();
                        break;

                        /* .. */

                        InGameMessage = "unhandled message with type: "
                            + ServerMessage.MessageType.ToString();
                        break;
                }
            }
        }
        private void process(string v)
        {
            // Need a try catch here
            PlayerData otherPlayer = JsonConvert.DeserializeObject<PlayerData>(v);
            // if it's the same player back just ignore it
            if (otherPlayer.playerID == thisPlayer.PlayerID)
                return;

            switch (otherPlayer.header)
            {
                case "Joined":
                    // Add the player to this game as another player
                    string ImageName = "Badges_" + Utility.NextRandom(0, playerTextures.Count - 1);
                    Player newPlayer = new Player(client, otherPlayer.imageName, otherPlayer.playerID, new Vector2(otherPlayer.X, otherPlayer.Y));
                    OtherPlayers.Add(newPlayer);

                    break;
                default:
                    break;
            }

        }

    }
}
