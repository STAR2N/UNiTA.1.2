using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;

public class UI_MiniMap : MonoBehaviour
{
    [SerializeField]
    private RenderTexture MinimapTexture;
    [SerializeField]
    private GameObject AllViewMinimapPanel;
    [SerializeField]
    private GameObject CharViewMinimapPanel;
    [SerializeField]
    private Button ShowMapBtn;
    [SerializeField]
    private bool IsActive_AllViewMinimapPanel = false;
    [SerializeField]
    private bool IsActive_CharViewMinimapPanel = true;
    [SerializeField]
    private bool IsActive_ShowMapBtn = false;

    private int TabCount = 0;
    private GameObject MinimapCamera;
    private Camera MinimapCam_CameraComponent;

    // ================ AllViewMap ================
    public void ShowAllViewMap()
    {
        IsActive_AllViewMinimapPanel = true;
    }
    public void HideAllViewMap()
    {
        IsActive_AllViewMinimapPanel = false;
    }

    // ================ MiniMap ================
    public void ShowMiniMap()
    {
        IsActive_CharViewMinimapPanel = true;
    }
    public void HideMiniMap()
    {
        IsActive_CharViewMinimapPanel = false;
    }

    // ================ MapButton ================
    public void ShowMapButton()
    {
        IsActive_ShowMapBtn = true;
    }
    public void HideMapButton()
    {
        IsActive_ShowMapBtn = false;
    }
    public void IncreaseTabCount()
    {
        ++TabCount;
        TabCount = (TabCount % 3);
        UpdateMinimap();
    }
    public void DecreaseTabCount()
    {
        if (TabCount > 0)
        {
            --TabCount;
        } else {
            TabCount = 2;
        }

        UpdateMinimap();
    }

    private void Awake()
    {
        AllViewMinimapPanel.SetActive(IsActive_AllViewMinimapPanel);
        CharViewMinimapPanel.SetActive(IsActive_CharViewMinimapPanel);
        ShowMapBtn.gameObject.SetActive(IsActive_ShowMapBtn);

        MinimapCamera = GameObject.FindWithTag("MiniMapCamera");
        if (!MinimapCamera)
            return;

        MinimapCam_CameraComponent = MinimapCamera.GetComponent<Camera>();
        if (!MinimapCam_CameraComponent)
            return;

        MinimapCam_CameraComponent.targetTexture = MinimapTexture;
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            IncreaseTabCount();
        }
    }

    private void UpdateMinimap() {
        switch (TabCount)
        {
            case 0:
                ShowMiniMap();
                HideAllViewMap();
                HideMapButton();
                break;
            case 1:
                ShowAllViewMap();
                HideMiniMap();
                HideMapButton();
                break;
            case 2:
                HideMiniMap();
                HideAllViewMap();
                ShowMapButton();
                break;
        }

        AllViewMinimapPanel.SetActive(IsActive_AllViewMinimapPanel);
        CharViewMinimapPanel.SetActive(IsActive_CharViewMinimapPanel);
        ShowMapBtn.gameObject.SetActive(IsActive_ShowMapBtn);
    }
}
