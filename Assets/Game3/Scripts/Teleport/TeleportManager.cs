using UnityEngine;

namespace iLLi
{
    public class TeleportManager : MonoBehaviour
    {
        [SerializeField] TeleportIndicator indicator;
        [SerializeField] float groundYNorm = 0.81f;

        public Teleportable Hovering { get; private set; }
        public Teleportable Selection { get; private set; }

        EPlaneType planeType;

        public void Focus(Teleportable teleportable, InteractionManager.Params param)
        {
            Hovering = teleportable;

            if (Mathf.Abs(param.Normal.y) < groundYNorm)
            {
                planeType = EPlaneType.Wall;
                indicator.transform.SetPositionAndRotation(param.Point, Quaternion.LookRotation(param.Normal));
            } else
            {
                planeType = EPlaneType.Ground;
                indicator.transform.SetPositionAndRotation(param.Point, (param.Normal.y > 0) ? Quaternion.identity : Quaternion.Euler(180, 0, 0));
            }
            indicator.ShowIndicator(planeType);
        }
        public void Defocus(Teleportable teleportable)
        {
            if (Hovering == teleportable)
            {
                Hovering = default;
                indicator.ShowIndicator(EPlaneType.None);
            }
        }
        public void Select(Teleportable teleportable)
        {
            Selection = teleportable;
        }
        public void Deselect(Teleportable teleportable, bool preventTrigger = false)
        {
            if (Selection != teleportable)
                return;

            if (Selection == null)
                return;

            var selection = Selection;
            Selection = null;

            if (preventTrigger)
                return;

            switch(planeType)
            {
                case EPlaneType.Wall:
                    transform.position = indicator.transform.position + indicator.transform.forward * 1f;
                    break;
                case EPlaneType.Ground:
                    transform.position = indicator.transform.position + indicator.transform.up * 1.7f;
                    break;
            }
        }
    }

    public enum EPlaneType
    {
        None,
        Ground,
        Wall
    }
}