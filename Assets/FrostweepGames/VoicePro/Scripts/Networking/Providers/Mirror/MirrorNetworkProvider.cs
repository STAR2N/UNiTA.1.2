#if MIRROR_NETWORK_PROVIDER
using Mirror;
using System;

namespace FrostweepGames.VoicePro.NetworkProviders.Mirror
{
    /// <summary>
    /// Mirror Network Provider for data transmition
    /// </summary>
    public class MirrorNetworkProvider : INetworkProvider
    {
        public event Action<INetworkActor, byte[]> NetworkDataReceivedEvent;

        private INetworkActor _networkActor;

        public bool ReadyToTransmit => NetworkClient.isConnected;

        public void Dispose()
        {
            NetworkClient.UnregisterHandler<TransportVoiceMessage>();

            _networkActor = null;
            NetworkDataReceivedEvent = null;
        }

        public void Init(INetworkActor networkActor)
        {
            _networkActor = networkActor;

            NetworkClient.RegisterHandler<TransportVoiceMessage>(NetworkEventReceivedHandler);
        }

        public void SendNetworkData(NetworkRouter.NetworkParameters parameters, byte[] bytes)
        {
            TransportVoiceMessage message = new TransportVoiceMessage()
            {
               sender = _networkActor.ToString(),
               bytes = bytes,
               targetAll = parameters.sendToAll
            };
            NetworkClient.connection.Send(message);
        }

        public string GetNetworkState()
        {
            return NetworkClient.isConnected ? "Connected" : "Disconnected";
        }

        public string GetConnectionToServer()
        {
            return NetworkClient.serverIp;
        }

        public string GetCurrentRoomName()
        {
            return NetworkClient.serverIp;
        }

        /// <summary>
        /// event handler of network events
        /// </summary>
        /// <param name="photonEvent"></param>
        private void NetworkEventReceivedHandler(TransportVoiceMessage message)
        {
            var sender = MirrorNetworkActor.FromString(message.sender);

            if (!message.targetAll && sender.Id == _networkActor.Id)
                return;

            NetworkDataReceivedEvent?.Invoke(sender, message.bytes);
        }

        public class MirrorNetworkActor : INetworkActor
        {
            public int Id { get; private set; }

            public string Name { get; private set; }

            public MirrorNetworkActor(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public override string ToString()
            {
                return Id + "_" + Name;
            }

            public static MirrorNetworkActor FromString(string data)
            {
                string[] split = data.Split('_');
                return new MirrorNetworkActor(int.Parse(split[0]), split[1]);
            }
        }
    }

    public struct TransportVoiceMessage : NetworkMessage
    {
        public string sender;
        public byte[] bytes;
        public bool targetAll;
    }
}
#endif