using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(2); // ステージ1
    }


    public void OpenSetting()
    {
        SceneManager.LoadScene(1);
    }
}