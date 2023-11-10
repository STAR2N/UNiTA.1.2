using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace iLLi
{
    public class OnClickEvent : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] UnityEvent clickEvent = new UnityEvent();

        public void OnPointerClick(PointerEventData eventData)
        {
            clickEvent.Invoke();
        }
    }
}