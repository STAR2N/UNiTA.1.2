namespace Game.Controller.Player {
    using UnityEngine;

    public abstract class IPlayerController : MonoBehaviour {
        public abstract void AssignObject(GameObject obj);
    }
}