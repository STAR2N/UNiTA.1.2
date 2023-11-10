using UnityEngine;

namespace Game.TransformExtension {
    public class UniformedRotation : MonoBehaviour {
        [SerializeField] Vector3 m_Axis = Vector3.up;
        [SerializeField] Space m_RelatedTo = Space.World;
        [SerializeField] float m_Speed;

        private void LateUpdate() {
            transform.Rotate(m_Axis, m_Speed * Time.deltaTime, m_RelatedTo);
        }
    }
}