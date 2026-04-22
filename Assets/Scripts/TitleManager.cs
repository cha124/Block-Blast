using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public AudioClip bgm;
    
    void Start()
    {
        AudioSource.PlayClipAtPoint(bgm, transform.position);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2); // ステージ1
    }

    public void OpenSetting()
    {
        SceneManager.LoadScene(0); // ステージ1
    }
}