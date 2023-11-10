using Mirror;
using System.Runtime.InteropServices;

public class VideoUtils : NetworkBehaviour
{
    [DllImport("__Internal")]
    private static extern void ExitRoom_React(string roomCode, string roomName);

    [TargetRpc(channel = Channels.Reliable)]
    public void RpcExitRoom(NetworkConnection target, string roomCode, string roomName)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            ExitRoom_React(roomCode, roomName);
#endif
    }
}
