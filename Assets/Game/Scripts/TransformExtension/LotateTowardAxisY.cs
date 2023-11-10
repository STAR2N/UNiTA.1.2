namespace Game.TransformExtension {
    using UnityEngine;

    public class LotateTowardAxisY : MonoBehaviour {
        [SerializeField] Camera m_Camera;

        public Transform ForwardOffset;
        public AnimationCurve Curve = AnimationCurve.Constant(0, 180, 1);
        
        public Camera Camera {
            get {
                return (m_Camera == null) ? Camera.main : m_Camera;
            }
        }

        private void LateUpdate() {
            var camera = Camera;
            if (camera != null && ForwardOffset != null) {
                var src = ForwardOffset.eulerAngles.y;

                var direction = transform.position - camera.transform.position;
                transform.LookAt(transform.position - direction, Camera.transform.up);
                var dst = transform.eulerAngles.y;

                var diff = Mathf.DeltaAngle(src, dst);
                var calced = Mathf.LerpAngle(src, dst, Curve.Evaluate(Mathf.Abs(diff)));

                transform.eulerAngles = new Vector3(0, calced, 0);
            }
        }
    }
}