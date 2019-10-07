using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class Room
    {
        public int roomID;
        public int maxClient;
        public List<string> userNames = new List<string>();
    }
}
