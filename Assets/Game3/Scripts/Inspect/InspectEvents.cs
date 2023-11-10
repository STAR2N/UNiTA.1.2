using UnityEngine;
using UnityEngine.Events;

namespace iLLi
{
    public class InspectEvents : MonoBehaviour
    {
        [SerializeField] UnityEvent<Camera> cameraEvent = new UnityEvent<Camera>();

        private void Awake()
        {
            var inspect = FindObjectOfType<InspectUI>();
            cameraEvent.Invoke(inspect.Camera);
        }
    }
}