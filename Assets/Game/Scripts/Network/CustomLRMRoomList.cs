namespace Game.Network {
    using LightReflectiveMirror;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// Original LRM does not support https.
    /// It can be fixed with configure original LRM codes but i does not prefer it.
    /// So make custom class and using it.
    /// - https://github.com/rrrfffrrr
    /// </summary>
    public static class CustomLRMRoomList {
        public static event Action OnNewServerList = delegate { };
        public static List<Room> RoomList { get; private set; } = new List<Room>();

        public static IEnumerator GetServerList(string uri) {
//            string uri = $"http://{serverIP}:{endpointServerPort}/api/compressed/servers";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
                webRequest.SetRequestHeader("Access-Control-Allow-Credentials", "true");
                webRequest.SetRequestHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
                webRequest.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                webRequest.SetRequestHeader("Access-Control-Allow-Origin", "*");

                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                var result = webRequest.downloadHandler.text;

                switch (webRequest.result) {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogWarning("LRM | Network Error while retreiving the server list!");
                        break;

                    case UnityWebRequest.Result.Success:
                        var rooms = JsonWrapper<Room>.FromJson(Decompress(result));
                        RoomList = new List<Room>(rooms);
                        OnNewServerList();
                        break;
                }
            }
        }

        private static string Decompress(string compressedText) {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream()) {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress)) {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        [Serializable]
        private class JsonWrapper<T> {
            public T[] Items;

            public static T[] FromJson(string json) {
                return JsonUtility.FromJson<JsonWrapper<T>>("{\"Items\":" + json + "}").Items;
            }
        }
    }

}