using UnityEngine;
using UnityEngine.Events;

public class TagTrigger : MonoBehaviour
{
    [SerializeField] string targetTag = "Default";
    [SerializeField] UnityEvent triggerEnterEvent = new UnityEvent();
    [SerializeField] UnityEvent triggerExitEvent = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            // Debug.Log($"Trigger entered by {other}", this);
            triggerEnterEvent.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            // Debug.Log($"Trigger exited by {other}", this);
            triggerExitEvent.Invoke();
        }
    }
}
