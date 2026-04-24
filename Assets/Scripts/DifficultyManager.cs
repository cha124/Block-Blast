using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DifficultyManager : MonoBehaviour
{
    public static float ballSpeed = 10f;
    public static float dropRate = 0.3f;



    public void SetEasy()
    {
        DifficultyManager.ballSpeed = 10f;
        DifficultyManager.dropRate = 0.3f;
    }

    public void SetNormal()
    {
        DifficultyManager.ballSpeed = 15f;
        DifficultyManager.dropRate = 0.2f;
    }

    public void SetHard()
    {
        DifficultyManager.ballSpeed = 20f;
        DifficultyManager.dropRate = 0.1f;
    }

    public void SetExtreme()
    {
        DifficultyManager.ballSpeed = 30f;
        DifficultyManager.dropRate = 0.0f;
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}