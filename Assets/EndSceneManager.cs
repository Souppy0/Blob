using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    public void onReplayclick()
    {
        SceneManager.LoadScene("Game");

    }
public void onExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
