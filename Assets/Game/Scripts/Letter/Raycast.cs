using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;


public class Raycast : NetworkBehaviour
{
    public bool IsLetterActive { get; set; }
    private string ClickObjectName;

    private void Start()
    {
        IsLetterActive = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (!IsLetterActive)
                {
                    ClickObjectName = hit.collider.gameObject.name;

                    if (ClickObjectName == "PlusButton" && LetterMaster.Instance.CurrentIndex < 8)
                    {

                        IsLetterActive = true;
                        OnClickPlusButton();
                    }
                    else if(ClickObjectName.Contains("LetterCard"))
                    {
                        IsLetterActive = true;
                        OnClickLetterCard(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    private void OnClickPlusButton()
    {
        LetterMaster.Instance.CmdChangeRandImage();
    }
    private void OnClickLetterCard(GameObject ClickCardObj)
    {
        // 편지 카드 눌렀을 때 기능

        LetterMaster.Instance.CompleteLetterUI.GetComponentInChildren<Image>().sprite = LetterMaster.Instance.LetterFront_BgImg[ClickCardObj.GetComponent<Letter>().ImgIdx];
        LetterMaster.Instance.CompleteLetterUI.GetComponentInChildren<TextMeshProUGUI>().text = ClickCardObj.GetComponent<Letter>().Content;

        LetterMaster.Instance.CompleteLetterUI.GetComponentInChildren<CanvasGroup>().alpha = 1.0f;
        LetterMaster.Instance.CompleteLetterUI.GetComponentInChildren<CanvasGroup>().interactable = true;
        LetterMaster.Instance.CompleteLetterUI.GetComponentInChildren<CanvasGroup>().blocksRaycasts = true;
    }
}
