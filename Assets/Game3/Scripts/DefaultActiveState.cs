using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iLLi
{
    /// <summary>
    /// Active state for play mode
    /// </summary>
    public class DefaultActiveState : MonoBehaviour
    {
        [SerializeField] bool active;

        private void Awake()
        {
            gameObject.SetActive(active);
        }
    }
}