using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mirror.SimpleWeb
{
    /// <summary>
    /// Handles Handshake to the server when it first connects
    /// <para>The client handshake does not need buffers to reduce allocations since it only happens once</para>
    /// </summary>
    internal class ClientHandshake
    {
        public bool TryHandshake(Connection conn, Uri uri)
        {
            try
            {
                Stream stream = conn.stream;

                byte[] keyBuffer = new byte[16];
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(keyBuffer);
                }

                string key = Convert.ToBase64String(keyBuffer);
                string keySum = key + Constants.HandshakeGUID;
                byte[] keySumBytes = Encoding.ASCII.GetBytes(keySum);
                Log.Verbose($"Handshake Hashing {Encoding.ASCII.GetString(keySumBytes)}");

                byte[] keySumHash = SHA1.Create().ComputeHash(keySumBytes);

                string expectedResponse = Convert.ToBase64String(keySumHash);
                string handshake =
                    $"GET {uri.PathAndQuery} HTTP/1.1\r\n" +
                    $"Host: {uri.Host}:{uri.Port}\r\n" +
                    $"Upgrade: websocket\r\n" +
                    $"Connection: Upgrade\r\n" +
                    $"Sec-WebSocket-Key: {key}\r\n" +
                    $"Sec-WebSocket-Version: 13\r\n" +
                    "\r\n";

                byte[] encoded = Encoding.ASCII.GetBytes(handshake);
                stream.Write(encoded, 0, encoded.Length);

                byte[] responseBuffer = new byte[1000];

                int? lengthOrNull = ReadHelper.SafeReadTillMatch(stream, responseBuffer, 0, responseBuffer.Length, Constants.endOfHandshake);

                if (!lengthOrNull.HasValue)
                {
                    Log.Error("Connected closed before handshake");
                    return false;
                }

                string responseString = Encoding.ASCII.GetString(responseBuffer, 0, lengthOrNull.Value);
                string acceptHeader = "Sec-WebSocket-Accept: ";
                foreach (var line in responseString.Split('\n'))
                {
                    if (line.StartsWith(acceptHeader, StringComparison.OrdinalIgnoreCase))
                    {
                        var value = line.Remove(0, acceptHeader.Length).Trim();
                        if (value == expectedResponse)
                        {
                            return true;
                        } else
                        {
                            Log.Error($"Response key incorrect, Response:{value} Expected:{expectedResponse}");
                            return false;
                        }
                    }
                }
                Log.Error($"Fail to find response key, Expected:{expectedResponse}");
                return false;
            }
            catch (Exception e)
            {
                Log.Exception(e);
                return false;
            }
        }
    }
}
