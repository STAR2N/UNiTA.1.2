using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuUI_SpaceList : MonoBehaviour
{
    [SerializeField] GameObject m_Content;
    [SerializeField] GameObject m_SpacePrefab;


    // Start is called before the first frame update
    void Start()
    {
        CreateSpaceList();
    }

    void CreateSpaceList() 
    {
        var levels = GameNetworkManager.Instance.Levels.Levels;
        foreach(var elem in levels) {
            var space = Instantiate(m_SpacePrefab);
            var MainMenuUI_Space = space.GetComponent<MainMenuUI_Space>();
            MainMenuUI_Space.SetName(elem.Value.Name);
            MainMenuUI_Space.SetImage(elem.Value.Image);
            space.transform.SetParent(m_Content.transform, false);
        }
    }
    
}
