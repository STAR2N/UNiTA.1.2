using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Game.Player;
using Game;

public class LetterTree : MonoBehaviour
{
    private bool IsCloselyTree;
    [SerializeField]   private GameObject PlusButton;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out WalkingCharacter player))
            return;
        if (!player.isLocalPlayer)
            return;
        if (LetterMaster.Instance.IsDone || !(LetterMaster.Instance.CurrentIndex < 8))
        {
            PlusButton.SetActive(false);
            return;
        }

        IsCloselyTree = true;
        PlusButton.SetActive(IsCloselyTree);
    }

    private void OnTriggerStay(Collider other)
    {
        if (LetterMaster.Instance.IsDone || !(LetterMaster.Instance.CurrentIndex < 8))
        {
            PlusButton.SetActive(false);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out WalkingCharacter player))
            return;
        if (!player.isLocalPlayer)
            return;

        IsCloselyTree = false;
        PlusButton.SetActive(IsCloselyTree);
    }

    private void Start()
    {
        IsCloselyTree = false;
        PlusButton.SetActive(IsCloselyTree);
    }
}
