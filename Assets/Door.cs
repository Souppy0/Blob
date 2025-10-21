using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorSceneTransition : MonoBehaviour
{
    [Header("End")]
    public string nextSceneName = "End"; // Make sure this matches your build settings

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered the door. Loading next scene...");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
