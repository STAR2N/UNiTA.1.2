using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ImageData")]
public class CharacterImageData : ScriptableObject {
    public List<Sprite> ImageList;
}