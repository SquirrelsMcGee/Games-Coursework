using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public int sceneId;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneId);
    }
}
