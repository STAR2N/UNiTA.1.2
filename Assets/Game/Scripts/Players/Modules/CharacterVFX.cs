namespace Game {
    using UnityEngine;

    public class CharacterVFX : MonoBehaviour {
        [SerializeField] Transform m_HeadOrigin;
        public Transform HeadOrigin => m_HeadOrigin;
    }
}