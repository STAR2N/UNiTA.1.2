namespace Game.Controller {
    using Game.Utility;
    using Mirror;
    using UnityEngine;

    public class ViewPosition : NetworkBehaviour {
        public static ListMap<string, ViewPosition> Positions = new ListMap<string, ViewPosition>();

        [Range(0, 180)] public float Fov = 0;
        public bool EnableAdvancedRotation = false;

        [SyncVar(hook = nameof(Hook_RoomName))] public string RoomName;
        private void Hook_RoomName(string @old, string @new) {
            if (!string.IsNullOrEmpty(@old)) {
                Positions.RemoveElement(@old, this);
            }
            if (!string.IsNullOrEmpty(@new)) {
                Positions.AddElement(@new, this);
            }
        }

        private void Awake() {
            Hook_RoomName(null, RoomName);
        }

        private void OnDestroy() {
            Hook_RoomName(RoomName, null);
        }
    }
}