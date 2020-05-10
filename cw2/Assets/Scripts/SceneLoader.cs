using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to load scenes by Index
/// </summary>
public class SceneLoader : MonoBehaviour
{
    // Scene index to load
    public int sceneId;

    // Loads the scene
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneId);
    }
}
