using System;
using System.IO;
using ProtoBuf;
using Photon.SocketServer;
using System.Collections.Generic;

namespace GameServer
{
    public static class PackageHelper
    {
        public static byte[] Serialize<T>(T instance)
        {
            byte[] bytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, instance);
                bytes = new byte[ms.Position];
                var fullBytes = ms.GetBuffer();
                Array.Copy(fullBytes, bytes, bytes.Length);
            }
            return bytes;
        }

        public static T Desirialize<T>(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }

        public static void SetData(OperationResponse response, byte[] bytes)
        {
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data[0] = bytes;
            response.Parameters = data;
        }
    }
}
