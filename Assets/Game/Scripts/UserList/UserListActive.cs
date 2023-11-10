using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserListActive : MonoBehaviour
{
    [SerializeField] private GameObject UserList;
    [SerializeField] private GameObject UserListButton;
    private CanvasGroup UserListCanvasGroup;
    private bool IsActive;

    private void Start()
    {
        IsActive = false;
        UserListCanvasGroup = UserList.GetComponent<CanvasGroup>();
    }

    public void ChangeUserListActive()
    {
        IsActive = !IsActive;

        if (IsActive)
        {
            ShowUserList();
        }
        else
        {
            HideUserList();
        }

        UserListButton.SetActive(!IsActive);
    }

    private void ShowUserList()
    {
        UserListCanvasGroup.interactable = true;
        UserListCanvasGroup.blocksRaycasts = true;
        UserListCanvasGroup.alpha = 1.0f;
    }

    private void HideUserList()
    {
        UserListCanvasGroup.interactable = false;
        UserListCanvasGroup.blocksRaycasts = false;
        UserListCanvasGroup.alpha = 0.0f;
    }
}
