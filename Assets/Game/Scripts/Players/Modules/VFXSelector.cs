namespace Game.Controller {
    using System.Collections.Generic;
    using UnityEngine;

    public class VFXSelector : MonoBehaviour {
        [SerializeField] int m_Index;
        public int Index {
            get => m_Index;
            set => Select(value);
        }
        public GameObject m_CurrentVFX;
        [SerializeField] List<GameObject> m_VFX;

        [SerializeField] AnimCharacterController AnimController;

        private void Awake() {
            foreach (var obj in m_VFX) {
                obj.SetActive(false);
            }
            Select(Index);
        }

        public void Select(int i) {
            if (m_CurrentVFX != null)
                m_CurrentVFX.SetActive(false);

            m_CurrentVFX = null;
            m_Index = i;
            m_CurrentVFX = m_VFX[i];

            if (m_CurrentVFX != null) {
                m_CurrentVFX.SetActive(true);
            }

            if(AnimController != null) {
                AnimController.animator = m_CurrentVFX.transform.GetComponent<Animator>();
            }
        }

#if UNITY_EDITOR
        private void OnValidate() {
            if (m_Index < 0) {
                m_Index = 0;
            } else if (m_VFX.Count == 0) {
                m_Index = 0;
            } else if (m_Index >= m_VFX.Count) {
                m_Index = m_VFX.Count - 1;
            }
        }
#endif
    }
}