using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour
{
    private static PopupManager instance;

    [SerializeField]
    GameObject container;

    Coroutine hideCoroutine;

    public static void Show(string popup)
    {
        if (instance == null)
            return;
        instance.ShowInternal(popup);
    }
    public static void Hide() => HideAfter(0);
    public static void HideAfter(float time)
    {
        if (instance == null)
            return;
        instance.HideInternal(time);
    }

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        HideAll();
    }

    private void OnDisable()
    {
        if (instance == this)
            instance = null;
    }

    private void ShowInternal(string popup)
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        foreach(Transform child in container.transform)
        {
            child.gameObject.SetActive(child.gameObject.name == popup);
        }
    }
    private void HideAll()
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        foreach (Transform child in container.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    private void HideInternal(float time)
    {
        if (time <= 0)
        {
            HideAll();
            return;
        }

        IEnumerator Await()
        {
            yield return new WaitForSeconds(time);
            HideAll();
        }
        hideCoroutine = StartCoroutine(Await());
    }
}
