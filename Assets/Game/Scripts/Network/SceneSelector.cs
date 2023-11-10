namespace Game.Network {
    using Mirror;

    public class SceneSelector : NetworkBehaviour {
        private void Start() {
            GameNetworkManager.Instance.ServerChangeScene(GameNetworkManager.SelectedLevel);
        }
    }
}