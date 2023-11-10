namespace Game.Wheels {
    using UnityEngine;
    using Game.TransformExtension;

    public class DesireWheelOrigin : MonoBehaviour {
        [SerializeField] CopyTransform m_CopyMethod;
        public CopyTransform CopyMethod => m_CopyMethod;
    }
}