namespace Game.TransformExtension {
    using UnityEngine;

    public class BillBoard : MonoBehaviour {
        [SerializeField] Camera m_Camera;
        public Camera Camera {
            get {
                return (m_Camera == null) ? Camera.main : m_Camera;
            }
        }

        private void LateUpdate() {
            var camera = Camera;
            if (camera != null)
                transform.LookAt(transform.position - Camera.transform.forward, Camera.transform.up);
        }
    }
}