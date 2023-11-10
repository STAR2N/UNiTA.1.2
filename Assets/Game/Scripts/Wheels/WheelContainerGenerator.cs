
namespace Game.Wheels {
    using Mirror;
    using UnityEngine;

    public class WheelContainerGenerator : NetworkBehaviour {
        [SerializeField] Transform m_TargetObject;
        [SerializeField] GameObject m_ContainerPrefab;

        private void Awake() {
            netIdentity.serverOnly = true; // Force server only
        }

        public override void OnStartServer() {
            base.OnStartServer();

            var origins = m_TargetObject.GetComponentsInChildren<DesireWheelOrigin>();

            foreach(var origin in origins) {
                var container = Instantiate(m_ContainerPrefab);
                origin.CopyMethod.Source = container.transform;
                NetworkServer.Spawn(container);
            }
        }
    }
}