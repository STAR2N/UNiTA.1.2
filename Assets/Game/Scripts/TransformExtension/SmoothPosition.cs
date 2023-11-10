using UnityEngine;

namespace Game.TransformExtension {
    public class SmoothPosition : MonoBehaviour {
        [SerializeField] Transform m_Source;
        [SerializeField] Transform m_Target;
        public Transform Target {
            get => (m_Target == null) ? transform : m_Target;
            set => m_Target = value;
        }

        [SerializeField] float m_Lerp;

        Vector3 m_LastPosition;

        private void LateUpdate() {
            var newPosition = m_Source.position;
            m_Target.position = Vector3.LerpUnclamped(m_LastPosition, newPosition, m_Lerp);
            m_LastPosition = newPosition;
        }
    }
}