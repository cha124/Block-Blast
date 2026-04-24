using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(2); // ステージ1
    }

    public void OpenSetting()
    {
        SceneManager.LoadScene(1);
    }
}