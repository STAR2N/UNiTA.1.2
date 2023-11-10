using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField] GameObject m_Canvas;
    [SerializeField] Image m_Image;
    [SerializeField] Sprite[] m_ImageList;
    public bool isOn;
    public bool isDone = false;
    private int curIdx = 0;

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

    void Start()
    {
        if(!isOn) {
            Hide();
        } else {
            m_Image.sprite = m_ImageList[0];
            m_Canvas.SetActive(true);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) {
            Next();
        }
    }

    void Next()
    {
        ++curIdx;
        if(curIdx < m_ImageList.Length) {
            m_Image.sprite = m_ImageList[curIdx];
        } else {
            Hide();
        }
    }

    void Hide()
    {
        m_Canvas.SetActive(false);
        isDone = true;
    }
}
