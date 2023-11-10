using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLManager : MonoBehaviour
{
    void Start()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        // WebGLInput.captureAllKeyboardInput = false;
#endif
    }
}
