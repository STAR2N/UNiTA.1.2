using UnityEngine;
using UnityEngine.EventSystems;

namespace iLLi
{
    public class MouseRotator : MonoBehaviour
    {
        [SerializeField] Transform target;

        private void Awake()
        {
            if (target == null)
                target = transform;
        }

        private void OnDisable()
        {
            target.rotation = Quaternion.identity;
        }

        private void OnMouseDrag()
        {
            if (!isActiveAndEnabled)
                return;

            if (EventSystem.current.currentSelectedGameObject != null)
                return;

            var horizontal = Input.GetAxis("Mouse X");
            var vertical = Input.GetAxis("Mouse Y");

            target.Rotate(Vector3.down, horizontal, Space.World);
            target.Rotate(Vector3.right, vertical, Space.World);

            BroadcastMessage(nameof(IEvent.OnDragInspectable), new Vector2(horizontal, vertical), SendMessageOptions.DontRequireReceiver);
        }

        public interface IEvent
        {
            void OnDragInspectable(Vector2 rotation);
        }
    }
}