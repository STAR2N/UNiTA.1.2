using UnityEngine;

namespace iLLi
{
    public class InspectableUtilities : MonoBehaviour
    {
        [SerializeField] Inspectable startup;

        private void Start()
        {
            if (startup != null)
                startup.Show();
        }

        public void Close()
        {
            var ui = GetComponentInParent<InspectUI>();
            if (ui == null)
                return;
            ui.Close();
        }
    }
}