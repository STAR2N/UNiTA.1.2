namespace Game {
    using UnityEngine;
    using Game.Controller;

    public class UserVFXDispatcher : MonoBehaviour {
        [SerializeField] VFXSelector m_Manager;

        private void Awake() {
            User.OnVFXChange += User_OnVFXChange;
            m_Manager.Index = User.VFX;
        }
        private void OnDestroy() {
            User.OnVFXChange -= User_OnVFXChange;
        }

        private void User_OnVFXChange(int val) {
            m_Manager.Index = val;
        }
    }
}