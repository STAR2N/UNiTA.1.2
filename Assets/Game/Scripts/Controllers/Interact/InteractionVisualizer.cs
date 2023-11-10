namespace Game.Controller {
    using UnityEngine;

    public class InteractionVisualizer : MonoBehaviour {
        [SerializeField] InteractionController Controller;
        [SerializeField] GameObject Visual;

        private void Update() {
            if (Controller.Interactables.Count > 0) {
                TryShowObject();
                MoveToObject(Controller.Interactables[0].transform);
            } else {
                TryHideObject();
            }
        }

        private void TryShowObject() {
            if (!Visual.activeSelf)
                Visual.SetActive(true);
        }
        private void TryHideObject() {
            if (Visual.activeSelf)
                Visual.SetActive(false);
        }

        private void MoveToObject(Transform target) {
            Visual.transform.position = target.position;
            //Visual.transform.LookAt(transform);
        }
    }
}