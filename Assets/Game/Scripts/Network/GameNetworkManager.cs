using FrostweepGames.VoicePro.NetworkProviders.Mirror;
using Game.Scene;
using Game;
using Mirror;
using LightReflectiveMirror;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class GameNetworkManager : NetworkManager
{
    public static GameNetworkManager Instance => singleton as GameNetworkManager;

    public static readonly Dictionary<int, string> RoomNames = new Dictionary<int, string>();
    public static readonly Dictionary<int, float> NextAvailableFrameTime = new Dictionary<int, float>();
    
    public static readonly Dictionary<int, UserInfo> UserList = new Dictionary<int, UserInfo>();

    public static int FrameRate = 10;
    public static float FrameInterval { 
        get {
            if (FrameRate <= 0f)
                return 0;
            return 1f / FrameRate;
        }
    }

    public LevelData Levels;
    public CharacterImageData CharacterImages;
    public static string SelectedLevel = string.Empty;

    public static bool IsGameSceneLoaded = false;
    public string userEmail;

    [DllImport("__Internal")]
    private static extern void RequestShowExitReview_React();

    [DllImport("__Internal")]
    private static extern void ExitSpace_React();
    
    [DllImport("__Internal")]
    private static extern void ExitRoom_React(string roomCode, string roomName);

    [DllImport("__Internal")]
    private static extern void RequestEmail_React();

    [DllImport("__Internal")]
    private static extern void RequestNetworkServerIP_React();

    string latestServerId;

    public void STATIC_PRESEVECODE()
    {
        comm_setEmail(null);
        comm_setServerIP("127.0.0.1");
    }

    public override void Start()
    {
        base.Start();
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        RequestNetworkServerIP_React();
        RequestEmail_React();
#endif

        Debug.Log("NetworkStart");
    }
    public virtual void Update()
    {
        if (!isNetworkActive)
            return;

        if (transport is LightReflectiveMirrorTransport lrmTransport)
        {
            if (latestServerId != lrmTransport.serverId)
            {
                latestServerId = lrmTransport.serverId;
                if (!string.IsNullOrEmpty(latestServerId))
                    Debug.Log($"Server code: \"{latestServerId}\"");
            }
        }
    }

    #region Server
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<TransportVoiceMessage>(OnTransportVoiceServerEventHandler);
        NetworkServer.RegisterHandler<InVideoFrameMessage>(OnVideoFrameServerEventHandler);
        NetworkServer.RegisterHandler<AddUserListMessage>(OnAddUserListServerEventHandler);

        ServerChangeScene(SelectedLevel);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        NetworkServer.UnregisterHandler<TransportVoiceMessage>();
        NetworkServer.UnregisterHandler<InVideoFrameMessage>();
        NetworkServer.UnregisterHandler<AddUserListMessage>();
    }

    public override void OnServerConnect(NetworkConnection conn) {
        base.OnServerConnect(conn);
        RoomNames.Add(conn.connectionId, string.Empty);
        NextAvailableFrameTime.Add(conn.connectionId, Time.realtimeSinceStartup + FrameInterval);
        Debug.Log(conn.connectionId);
    }

    public override void OnServerReady(NetworkConnection conn) {
        base.OnServerReady(conn);

        if (IsGameSceneLoaded) {
            var pos = GetStartPosition();
            var obj = Instantiate(playerPrefab, pos.position, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, obj);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        base.OnServerDisconnect(conn);
        RoomNames.Remove(conn.connectionId);
        NextAvailableFrameTime.Remove(conn.connectionId);

        UserList.Remove(conn.connectionId);
        UpdateUserListToClient();
    }

    public override void OnServerSceneChanged(string sceneName) {
        base.OnServerSceneChanged(sceneName);

        if (sceneName != onlineScene) {
            IsGameSceneLoaded = true;

            foreach (var conn in NetworkServer.connections) {
                if (conn.Value.isReady) {
                    var obj = Instantiate(playerPrefab);
                    NetworkServer.AddPlayerForConnection(conn.Value, obj);
                }
            }
            if (mode == NetworkManagerMode.Host && NetworkClient.ready) {
                var pos = GetStartPosition();
                var obj = Instantiate(playerPrefab, pos.position, Quaternion.identity);
                NetworkServer.AddPlayerForConnection(NetworkServer.localConnection, obj);
            }
        }
    }

    public static void RoomForEach(string room, System.Action<int> callback) {
        foreach(var kv in RoomNames) {
            if (kv.Value == room) {
                callback(kv.Key);
            }
        }
    }
    #endregion

    #region Client
    public override void OnStartClient() {
        base.OnStartClient();
        NetworkClient.RegisterHandler<OutVideoFrameMessage>(OnVideoFrameClientEventHandler);
        NetworkClient.RegisterHandler<UpdateUserListMessage>(OnUpdateUserListEventHandler);
    }
    public override void OnStopClient() {
        base.OnStopClient();
        NetworkClient.UnregisterHandler<OutVideoFrameMessage>();
        NetworkClient.UnregisterHandler<UpdateUserListMessage>();
        #if UNITY_WEBGL == true && UNITY_EDITOR == false
        if(RoomManager.Instance) {
            if(RoomManager.Instance.isEnterSpace) {
                RequestShowExitReview_React();
                ExitSpace_React();
                ExitRoom_React("", "");
                RoomManager.Instance.isEnterSpace = false;
            }
        }
        #endif
    }

    public static event System.Action<OutVideoFrameMessage> OnNewVideoFrame = delegate { };
    public static event System.Action<UpdateUserListMessage> OnNewUserList = delegate { };
    #endregion

    #region Media
    private void OnTransportVoiceServerEventHandler(NetworkConnection connection, TransportVoiceMessage message) {
        if (RoomNames.TryGetValue(connection.connectionId, out var currentRoom)) {
            foreach (var conn in NetworkServer.connections) {
                if (RoomNames.TryGetValue(conn.Key, out var room) && !string.IsNullOrEmpty(room) && room == currentRoom && connection != conn.Value)
                {
                    conn.Value.Send(message);
                }
            }
        }
    }

    private void OnVideoFrameServerEventHandler(NetworkConnection connection, InVideoFrameMessage message) {
        if (!NextAvailableFrameTime.TryGetValue(connection.connectionId, out var next) || next > Time.realtimeSinceStartup) // Skip if not available
            return;

        NextAvailableFrameTime[connection.connectionId] = Time.realtimeSinceStartup + FrameInterval;

        var outMessage = new OutVideoFrameMessage {
            uid = connection.connectionId,
            OriginalSize = message.OriginalSize,
            Size = message.Size,
            Data = message.Data
        };

        if (RoomNames.TryGetValue(connection.connectionId, out var currentRoom)) {
            foreach (var conn in NetworkServer.connections) {
                if (RoomNames.TryGetValue(conn.Key, out var room) && !string.IsNullOrEmpty(room) && room == currentRoom && connection != conn.Value) {
                    conn.Value.Send(outMessage);
                }
            }
        }
    }

    private void OnAddUserListServerEventHandler(NetworkConnection connection, AddUserListMessage message) {
            var id = connection.connectionId;
            var name = message.name;
            var VFX = message.VFX;
            var isServer = message.isServer;
            if (!UserList.ContainsKey(id)) {
                UserList.Add(id, new GameNetworkManager.UserInfo {Name = name, VFX = VFX, isServer = isServer });
            }

            UpdateUserListToClient();
    }

    public void UpdateUserListToClient() {
        var names = new List<string>();
        var VFXs = new List<int>();
        var IsServers = new List<bool>();

        foreach(var kv in UserList) {
            names.Add(kv.Value.Name);
            VFXs.Add(kv.Value.VFX);
            IsServers.Add(kv.Value.isServer);
        }

        NetworkServer.SendToAll(new UpdateUserListMessage {names = names, VFXs = VFXs, IsServers = IsServers });
    }

    private void OnVideoFrameClientEventHandler(OutVideoFrameMessage message) {
        OnNewVideoFrame(message);
    }

    private void OnUpdateUserListEventHandler(UpdateUserListMessage message) {
        OnNewUserList(message);
    }
    #endregion

    void comm_setEmail(string email)
    {
        userEmail = email;
    }
    void comm_setServerIP(string ip)
    {
        if (transport is LightReflectiveMirrorTransport lrm)
        {
            networkAddress = ip;
            lrm.serverIP = ip;
        }
    }

    public struct InVideoFrameMessage : NetworkMessage {
        public Vector2Int OriginalSize;
        public Vector2Int Size;
        public byte[] Data;
    }
    public struct OutVideoFrameMessage : NetworkMessage {
        public int uid;
        public Vector2Int OriginalSize;
        public Vector2Int Size;
        public byte[] Data;
    }
    public struct AddUserListMessage : NetworkMessage {
        public string name;
        public int VFX;
        public bool isServer;
    }
    public struct Message : NetworkMessage {
        // public string test;
    }
    public struct UpdateUserListMessage : NetworkMessage {
        public List<string> names;
        public List<int> VFXs;
        public List<bool> IsServers;
    }

    public struct UserInfo {
        public string Name;
        public int VFX;
        public bool isServer;
    }
}
