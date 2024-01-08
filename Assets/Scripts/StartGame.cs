using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartPlay()
    {
        SceneManager.LoadScene(1);
    }
}
