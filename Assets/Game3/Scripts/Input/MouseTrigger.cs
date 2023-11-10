using UnityEngine;
using UnityEngine.Events;

namespace iLLi
{
    public class MouseTrigger : MonoBehaviour
    {
        public EMouse Button;

        [SerializeField] UnityEvent TriggerEvent = new UnityEvent();
        [SerializeField] UnityEvent UntriggerEvent = new UnityEvent();

        private void Update()
        {
            if (Input.GetMouseButtonDown((int)Button))
                TriggerEvent.Invoke();
            if (Input.GetMouseButtonUp((int) Button))
                UntriggerEvent.Invoke();
        }

        public enum EMouse
        {
            Left = 0,
            Right = 1,
            Middle = 2
        }
    }
}