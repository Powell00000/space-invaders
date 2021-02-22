using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUiScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var uiScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("UI");
        if (!uiScene.IsValid())
            UnityEngine.SceneManagement.SceneManager.LoadScene("UI", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

}
