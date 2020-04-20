using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class LoadStageOnClick : MonoBehaviour
{

    public void loadStage(int stageIndex)
    {
        SceneManager.LoadScene(stageIndex);
    }
}
