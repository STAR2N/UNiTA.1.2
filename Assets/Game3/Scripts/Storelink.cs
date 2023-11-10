using System.Runtime.InteropServices;
using UnityEngine;

namespace iLLi
{
    public class Storelink : MonoBehaviour
    {
        [SerializeField] string URL;

        public void Open()
        {
            OpenStoreLink_React(URL);
        }

        [DllImport("__Internal")]
        private static extern void OpenStoreLink_React(string url);
    }
}