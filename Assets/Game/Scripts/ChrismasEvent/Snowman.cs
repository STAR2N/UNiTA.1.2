using System.Collections.Generic;
using Game.Controller.Player;
using Game.Player;
using System.Linq;

namespace Game.MiniGame.SnowMan
{
    using UnityEngine;
    
    public class Snowman : MonoBehaviour
    {
        public static Snowman Instance { get; private set; }

        [SerializeField]
        GameObject arms;
        [SerializeField]
        GameObject hat;
        [SerializeField]
        GameObject nose;

        [SerializeField]
        GameObject messagePopup;
        [SerializeField]
        SnowmanDialog messageDialog;

        [SerializeField]
        ParticleSystem particle;

        [SerializeField]
        List<Item> items = new List<Item>();

        bool isStarted = false;
        List<ItemType> acquireItems = new List<ItemType>();

        #region Unity Event
        private void Awake()
        {
            arms.SetActive(false);
            hat.SetActive(false);
            nose.SetActive(false);
            messagePopup.SetActive(false);
            ChangeActiveAllItems(false);
        }
        private void OnEnable()
        {
            if (Instance == null)
                Instance = this;
        }
        private void OnDisable()
        {
            if (Instance == this)
                Instance = null;
        }
        private void Update()
        {
            if (messagePopup.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (!isStarted)
                    {
                        messageDialog.ChangePage("QuestInfo");
                        ChangeActiveAllItems(true);
                        isStarted = true;
                    }
                    else if (acquireItems.Contains(ItemType.Nose) && acquireItems.Contains(ItemType.Hat) && acquireItems.Contains(ItemType.Arm))
                    {
                        particle.Emit(30);
                    }
                }
            }
        }
        #endregion

        public void ChangeActiveAllItems(bool active)
        {
            foreach(var item in items)
            {
                if (item.gameObject.activeSelf != active)
                    item.gameObject.SetActive(active);
            }
        }

        public void AcquireItem(ItemType module)
        {
            bool CheckAndChange(GameObject obj)
            {
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                    return true;
                }
                return false;
            }

            switch (module)
            {
                case ItemType.Hat:
                    if (CheckAndChange(hat))
                    {
                        messageDialog.ChangePage("hat");
                        acquireItems.Add(ItemType.Hat);
                    }
                    break;
                case ItemType.Arm:
                    if (CheckAndChange(arms))
                    {
                        messageDialog.ChangePage("arms");
                        acquireItems.Add(ItemType.Arm);
                    }
                    break;
                case ItemType.Nose:
                    if (CheckAndChange(nose))
                    {
                        messageDialog.ChangePage("nose");
                        acquireItems.Add(ItemType.Nose);
                    }
                    break;
            }
            
            if (acquireItems.Contains(ItemType.Nose) && acquireItems.Contains(ItemType.Hat) && acquireItems.Contains(ItemType.Arm))
            {
                messageDialog.ChangePage("end");
                var item = items.Where(e => e.Type == module).FirstOrDefault();
                var pos = item.transform.position;
                var dir = Camera.main.transform.position - item.transform.position;
                dir.y = 0;
                var rot = Quaternion.LookRotation(dir, Vector3.up);
                transform.SetPositionAndRotation(pos, rot);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out WalkingCharacter player))
                return;
            if (!player.isLocalPlayer)
                return;

            messagePopup.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out WalkingCharacter player))
                return;
            if (!player.isLocalPlayer)
                return;
            
            messagePopup.SetActive(false);
        }
    }

    public enum ItemType
    {
        None,
        Hat,
        Arm,
        Nose
    }
}