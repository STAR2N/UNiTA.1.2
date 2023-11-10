using System;
#if PUN2_NETWORK_PROVIDER
using FrostweepGames.VoicePro.NetworkProviders.PUN;
#endif
#if MIRROR_NETWORK_PROVIDER
using FrostweepGames.VoicePro.NetworkProviders.Mirror;
#endif

namespace FrostweepGames.VoicePro
{
	public class NetworkRouter
	{
		private const string Unknown = "Unknown";

		/// <summary>
		/// Network event handler that raises when network data recieved
		/// </summary>
		public event Action<INetworkActor, byte[]> NetworkDataReceivedEvent;

		private static NetworkRouter _Instance;

		/// <summary>
		/// Instance of a network router
		/// </summary>
		public static NetworkRouter Instance
		{
			get
			{
				if (_Instance == null)
					_Instance = new NetworkRouter();
				return _Instance;
			}
		}

		/// <summary>
		/// Current network provider instance
		/// </summary>
		private INetworkProvider _networkProvider;

		/// <summary>
		/// Current Network type
		/// </summary>
		public Enumerators.NetworkType NetworkType { get; private set; } = Enumerators.NetworkType.Unknown;

		public bool ReadyToTransmit => _networkProvider.ReadyToTransmit;

		/// <summary>
		/// Registers user in network, but first registers network
		/// </summary>
		/// <param name="id">user id</param>
		/// <param name="name">user name</param>
		/// <param name="networkType">type of the network will use</param>
		public void Register(int id, string name, Enumerators.NetworkType networkType)
		{
			NetworkType = networkType;

			switch (networkType)
			{
				case Enumerators.NetworkType.PUN2:
					{
#if PUN2_NETWORK_PROVIDER
						_networkProvider = new PUNNetworkProvider();
						_networkProvider.Init(new PUNNetworkProvider.PUNNetworkActor(id, name));
						_networkProvider.NetworkDataReceivedEvent += ReceiveNetworkDataActionHandler;
#endif
					}
					break;
				case Enumerators.NetworkType.Mirror:
					{
#if MIRROR_NETWORK_PROVIDER
						_networkProvider = new MirrorNetworkProvider();
						_networkProvider.Init(new MirrorNetworkProvider.MirrorNetworkActor(id, name));
						_networkProvider.NetworkDataReceivedEvent += ReceiveNetworkDataActionHandler;
#endif
					}
					break;
				default:
					throw new NotImplementedException("Network didn't registered! Network type didn't implemented.");
			}
		}

		/// <summary>
		/// Sends data over network based on selected NetworkType
		/// </summary>
		/// <param name="parameters"></param>
		/// <param name="data"></param>
		public void SendNetworkData(NetworkParameters parameters, byte[] data)
		{
			if (_networkProvider == null)
				throw new NullReferenceException("Network didn't registered! Try call Register function first.");

			_networkProvider.SendNetworkData(parameters, data);
		}

		/// <summary>
		/// Unregisters and destroys current network user and network connection at all
		/// </summary>
		public void Unregister()
		{
			_networkProvider?.Dispose();
			_networkProvider = null;
		}

		/// <summary>
		/// Returns current network state based on connection
		/// </summary>
		/// <returns></returns>
		public string GetNetworkState()
		{
			return _networkProvider != null ? _networkProvider.GetNetworkState() : Unknown;
		}

		/// <summary>
		/// Returns current connection state bsed on server
		/// </summary>
		/// <returns></returns>
		public string GetConnectionToServer()
		{
			return _networkProvider != null ? _networkProvider.GetConnectionToServer() : Unknown;
		}

		/// <summary>
		/// Returns name of a room user connected to
		/// </summary>
		/// <returns></returns>
		public string GetCurrentRoomName()
		{
			return _networkProvider != null ? _networkProvider.GetCurrentRoomName() : Unknown;
		}

		/// <summary>
		/// Handler of receiving data from different networks
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="data"></param>
		private void ReceiveNetworkDataActionHandler(INetworkActor sender, byte[] data)
		{
			NetworkDataReceivedEvent?.Invoke(sender, data);
		}

		public class NetworkParameters
		{
			public bool sendToAll;

			public bool reliable;
		}
	}
}