using System.Collections.Generic;
using UnityEngine;

namespace Game.MiniGame.SnowMan
{
    public class SnowmanDialog : MonoBehaviour
    {
        [SerializeField]
        string firstPage;

        private void Start()
        {
            ChangePage(firstPage);
        }

        public void ChangePage(string pageName)
        {
            foreach(Transform page in transform)
            {
                var active = pageName == page.name;
                if (active != page.gameObject.activeSelf)
                    page.gameObject.SetActive(active);
            }
        }
    }
}