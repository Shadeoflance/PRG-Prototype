using UnityEngine;

class UIButtonActions : MonoBehaviour
{
    public void StartTutorial()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}