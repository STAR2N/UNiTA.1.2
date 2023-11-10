using UnityEngine;

namespace iLLi
{
    public class HeadRotator : MonoBehaviour
    {
        [SerializeField] Transform head;
        [SerializeField, Range(0, 89.9f)] float maxClampX = 60f;
        [SerializeField, Range(0, 89.9f)] float minClampX = 60f;

        private void Update()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;

            Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        public void Rotate(float horizontal, float vertical)
        {
            var rot = head.localRotation.eulerAngles;

            rot.x = Mathf.Clamp(Mathf.DeltaAngle(0, rot.x) - vertical, -minClampX, maxClampX);
            rot.y += horizontal;

            head.localRotation = Quaternion.Euler(rot);
        }
    }
}