using System;

namespace FrostweepGames.VoicePro
{
    public interface INetworkProvider
    {
        event Action<INetworkActor, byte[]> NetworkDataReceivedEvent;

        bool ReadyToTransmit { get; }

        void Init(INetworkActor networkActor);
        void Dispose();
        void SendNetworkData(NetworkRouter.NetworkParameters parameters, byte[] bytes);

		string GetNetworkState();
        string GetConnectionToServer();
        string GetCurrentRoomName();
	}
}