using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private Camera minimapCamera;
    [SerializeField]
    private float zoomMin = 1.0f;
    [SerializeField]
    private float zoomMax = 30.0f;
    [SerializeField]
    private float zoomOneStep = 1.0f;
    [SerializeField]
    private TextMeshProUGUI textMapName;
    private void Awake()
    {
        textMapName.text = SceneManager.GetActiveScene().name;
    }

    public void ZoomIn()
    {
        minimapCamera.orthographicSize = Mathf.Max(minimapCamera.orthographicSize - zoomOneStep, zoomMin);
    }
    public void ZoomOut()
    {
        minimapCamera.orthographicSize = Mathf.Min(minimapCamera.orthographicSize + zoomOneStep, zoomMax);
    }
}
