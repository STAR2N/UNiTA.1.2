namespace Game.PhysicsExtension {
    using UnityEngine;

    public class PlatformTrigger : MonoBehaviour {
        [SerializeField] Platform OwnPlatform;

        private void OnTriggerEnter(Collider other) {
            OwnPlatform.Regist(other.transform);
        }

        private void OnTriggerExit(Collider other) {
            OwnPlatform.Unregist(other.transform);
        }
    }
}