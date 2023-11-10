using Game.UI;
using Mirror;

namespace Game.Network {
    public class ServerData : NetworkBehaviour {
        public static ServerData Instance { get; private set; }
        private void Awake() {
            if (Instance == null)
                Instance = this;
        }
        private void OnDestroy() {
            if (Instance == this)
                Instance = null;
        }

        public event System.Action OnDataChange = delegate { };

        [SyncVar(hook = nameof(Hook_ChangeName))] public string ServerName;
        [SyncVar(hook = nameof(Hook_ChangeId))] public string ServerId;

        public override void OnStartServer() {
            base.OnStartServer();

            Net.QueryServerData.QueryAsync(data => { ServerName = data; OnDataChange(); }, Net.QueryServerData.Type.Name);
            Net.QueryServerData.QueryAsync(data => { ServerId = data; OnDataChange(); }, Net.QueryServerData.Type.Code);
        }

        void Hook_ChangeName(string _, string @new) {
            OnDataChange();
        }

        void Hook_ChangeId(string _, string @new) {
            OnDataChange();
        }
    }
}