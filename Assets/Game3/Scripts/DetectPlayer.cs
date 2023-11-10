using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace iLLi
{
    public class DetectPlayer : MonoBehaviour
    {
        [SerializeField] UnityEvent<GameObject> DetectEvent = new UnityEvent<GameObject>();
        [SerializeField] UnityEvent<GameObject> LostEvent = new UnityEvent<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                DetectEvent.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                LostEvent.Invoke(other.gameObject);
        }
    }
}