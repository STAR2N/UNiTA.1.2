namespace Game.Activity.Loader {
    using UnityEngine;

    public class SingleActivityLoader : MonoBehaviour {
        public GameObject Activity;

        private void Awake() {
            Instantiate(Activity);
            Destroy(gameObject);
        }
    }
}