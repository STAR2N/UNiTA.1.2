using UnityEngine;

namespace iLLi
{
    public class TeleportIndicator : MonoBehaviour
    {
        [SerializeField] GameObject groundIndicator;
        [SerializeField] GameObject wallIndicator;

        private void Awake()
        {
            ShowIndicator(EPlaneType.None);
        }

        public void ShowIndicator(EPlaneType type)
        {
            void SetActiveInternal(GameObject obj, EPlaneType targetType)
            {
                var isType = type == targetType;
                if (isType != obj.activeSelf)
                    obj.SetActive(isType);
            }

            SetActiveInternal(groundIndicator, EPlaneType.Ground);
            SetActiveInternal(wallIndicator, EPlaneType.Wall);
        }
    }
}