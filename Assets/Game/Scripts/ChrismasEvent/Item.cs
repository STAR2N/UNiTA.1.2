using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game.MiniGame.SnowMan
{
    using UnityEngine;

    public class Item : MonoBehaviour
    {
        [SerializeField]
        new BoxCollider collider;

        [SerializeField]
        ItemType type;
        public ItemType Type => type;

        [SerializeField]
        UnityEvent onPickedUp = new UnityEvent();

        private void Start()
        {
            Trigger.Attach(collider, this);
        }

        public void TryConsume(WalkingCharacter player)
        {
            if (player == null)
                return;
            if (!player.isLocalPlayer)
                return;

            Consume();
        }

        public void Consume()
        {
            if (!collider.gameObject.activeSelf)
                return;
            PopupManager.Show($"item:acquire:{type}");
            PopupManager.HideAfter(5);
            collider.gameObject.SetActive(false);
            Snowman.Instance.AcquireItem(type);
            onPickedUp.Invoke();
        }

        private class Trigger : MonoBehaviour
        {
            public static void Attach(Collider collider, Item item)
            {
                var trigger = collider.gameObject.AddComponent<Trigger>();
                trigger.item = item;
            }

            Item item;

            private void OnTriggerEnter(Collider other)
            {
                if (!other.TryGetComponent(out WalkingCharacter player))
                    return;

                item.TryConsume(player);
            }
        }





        // private void OnEnable()
        // {
        //     isAcquirable = false;
        // }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.Return))
        //         TryConsume();
        // }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (!other.TryGetComponent(out WalkingCharacter player))
        //         return;
        //     if (!player.isLocalPlayer)
        //         return;

        //     isAcquirable = true;
        // }
        // private void OnTriggerExit(Collider other)
        // {
        //     if (!other.TryGetComponent(out WalkingCharacter player))
        //         return;
        //     if (!player.isLocalPlayer)
        //         return;

        //     isAcquirable = false;
        // }
        // public void TryConsume()
        // {
        //     if (isAcquirable)
        //     {
        //         gameObject.SetActive(false);
        //         Snowman.Instance.AcquireItem(type);
        //     }
        // }
    }
}