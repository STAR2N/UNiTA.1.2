namespace Game.Network {
    [System.Serializable]
    public class ExtraServerData {
        public const string NETWORK_VERSION = "1.0.5"; // If this string is not same on client and server, client wont show server on list
        public static string APPLICATION_VERSION = UnityEngine.Application.version;

        public string NetworkVersion = NETWORK_VERSION;
        public string ApplicationVersion = APPLICATION_VERSION;

        public int HostCharacterIdx = 0;
        public string Map;
    }
}