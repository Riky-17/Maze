using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderInvoker : MonoBehaviour
{
    bool firstUpdate = true;

    void Update()
    {
        if (firstUpdate)
        {
            firstUpdate = false;
            SceneLoader.LoadingScreenCallBack();
        }
    }
}
