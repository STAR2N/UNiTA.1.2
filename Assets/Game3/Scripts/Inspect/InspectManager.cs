using UnityEngine;

namespace iLLi
{
    public class InspectManager : MonoBehaviour
    {
        public Inspectable Selection { get; private set; }

        public void Select(Inspectable teleportable)
        {
            Selection = teleportable;
        }
        public void Deselect(Inspectable inspectable, bool preventTrigger = false)
        {
            if (Selection != inspectable)
                return;

            if (Selection == null)
                return;

            var selection = Selection;
            Selection = null;

            if (preventTrigger)
                return;

            selection.Show();
        }
    }
}