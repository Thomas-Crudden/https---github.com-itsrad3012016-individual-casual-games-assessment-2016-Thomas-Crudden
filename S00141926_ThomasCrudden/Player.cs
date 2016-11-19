using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using GameServerConsole;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace S00141926_ThomasCrudden
{
    public class Player: Sprite
    {
        string playerID;
        Vector2 position = new Vector2();
        NetClient _client;
        PlayerData _playerDataPacket;
        bool joined;

        public string ImageName = string.Empty;

        public Player(NetClient client, string ImgName, string playerid, Vector2 StartPos)
        {
            // Created as a reult of a joined message
            position = StartPos;
            playerID = playerid;
            ImageName = ImgName;

        }

        public Player(NetClient client, Guid playerid, string ImgName, Vector2 StartPos)
        {

            position = StartPos;
            playerID = playerid.ToString();
            ImageName = ImgName;
            // consruct a join player packet and serialise it
            _playerDataPacket = new PlayerData("Join", ImageName, PlayerID, StartPos.X, StartPos.Y);
            string json = JsonConvert.SerializeObject(_playerDataPacket);
            // construct the outgoing message
            NetOutgoingMessage sendMsg = client.CreateMessage();
            sendMsg.Write(json);
            client.SendMessage(sendMsg, NetDeliveryMethod.ReliableOrdered);


        }

        

        public PlayerData PlayerDataPacket
        {
            get
            {
                return _playerDataPacket;
            }

            set
            {
                _playerDataPacket = value;
                position.X = value.X;
                position.Y = value.Y;
            }
        }

        public string PlayerID
        {
            get
            {
                return playerID;
            }

            set
            {
                playerID = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public bool Joined
        {
            get
            {
                return joined;
            }

            set
            {
                joined = value;
            }
        }

        public void ChangePosition(Vector2 newPosition)
        {
            position = newPosition;

        }

        public void Move(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.W) && _position.Y > 0)
            {
                _position -= new Vector2(0, 1);

            }
            if (state.IsKeyDown(Keys.S) && _position.Y < 550)
            {
                _position += new Vector2(0, -1);

            }
            if (state.IsKeyDown(Keys.D) && _position.X < 760)
            {
                _position += new Vector2(1, 0);

            }
            if (state.IsKeyDown(Keys.A) && _position.X > 0)
            {
                _position -= new Vector2(-1, 0);

            }
        }
    }
}
