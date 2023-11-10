using Game;
using Game.Network;
using LightReflectiveMirror;
using Mirror;
using System.Linq;
using System.Collections;
using UnityEngine;

public class AutoStartHost : MonoBehaviour
{
    LightReflectiveMirrorTransport Transport => Mirror.Transport.activeTransport as LightReflectiveMirrorTransport;

    // Start is called before the first frame update
    void Start()
    {
        string desireMap = null;
        if (StartupManager.Instance != null)
            desireMap = StartupManager.Instance.Params.Map;

        string map;
        if (desireMap != null && GameNetworkManager.Instance.Levels.Levels.TryGetValue(desireMap, out var level))
        {
            map = level.Scene;
        } else
        {
            map = GameNetworkManager.Instance.Levels.Levels.FirstOrDefault().Value.Scene;
        }
        GameNetworkManager.SelectedLevel = map;

        var transport = Transport;
        transport.serverName = System.Guid.NewGuid().ToString();
        GameNetworkManager.Instance.networkAddress = StartupManager.Instance.Params.ServerURL;
        transport.serverIP = StartupManager.Instance.Params.ServerURL;

        var extraServerData = new ExtraServerData();
        extraServerData.HostCharacterIdx = User.VFX;
        extraServerData.Map = GameNetworkManager.SelectedLevel;

        transport.extraServerData = JsonUtility.ToJson(extraServerData);

        transport.isPublicServer = true;
        transport.ConnectToRelay();
    }
    public void OnConnectedToRelay()
    {
        Transport.RequestServerList();
    }
    public void OnDisconnectedToRelay()
    {
        IEnumerator Retry()
        {
            Debug.Log($"Retry after 5 seconds...");
            yield return new WaitForSecondsRealtime(5);
            Transport.ConnectToRelay();
        }
        StartCoroutine(Retry());
    }
    public void OnListUpdated()
    {
        var transport = Transport;
        foreach (var room in transport.relayServerList)
        {
            if (room.maxPlayers <= room.currentPlayers)
                continue;
            try
            {
                var data = JsonUtility.FromJson<ExtraServerData>(room.serverData);
                if (data.ApplicationVersion != ExtraServerData.APPLICATION_VERSION)
                    continue;
                if (data.NetworkVersion != ExtraServerData.NETWORK_VERSION)
                    continue;
                if (data.Map != GameNetworkManager.SelectedLevel)
                    continue;
                NetworkManager.singleton.networkAddress = room.serverId;
                NetworkManager.singleton.StartClient();
            }
            catch (System.Exception e) {
                Debug.LogError(e);
            }
        }
        NetworkManager.singleton.StartHost();
    }
}
