using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    public GameObject stage1Button;
    public GameObject stage2Button;
    public GameObject stage3Button;
    public GameObject stage4Button;
    public GameObject stage5Button;
    public GameObject stage6Button;
    public GameObject stage7Button;

    void Start()
    {
        LoadStageData();
        UpdateStageButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }

    void LoadStageData()
    {
        GameManager.stage1 = PlayerPrefs.GetInt("Stage1", 0);
        GameManager.stage2 = PlayerPrefs.GetInt("Stage2", 0);
        GameManager.stage3 = PlayerPrefs.GetInt("Stage3", 0);
        GameManager.stage4 = PlayerPrefs.GetInt("Stage4", 0);
        GameManager.stage5 = PlayerPrefs.GetInt("Stage5", 0);
        GameManager.stage6 = PlayerPrefs.GetInt("Stage6", 0);
        GameManager.stage7 = PlayerPrefs.GetInt("Stage7", 0);
    }

    void UpdateStageButtons()
    {
        if (stage1Button != null) stage1Button.SetActive(true);
        if (stage2Button != null) stage2Button.SetActive(GameManager.stage1 == 1);
        if (stage3Button != null) stage3Button.SetActive(GameManager.stage2 == 1);
        if (stage4Button != null) stage4Button.SetActive(GameManager.stage3 == 1);
        if (stage5Button != null) stage5Button.SetActive(GameManager.stage4 == 1);
        if (stage6Button != null) stage6Button.SetActive(GameManager.stage5 == 1);
        if (stage7Button != null) stage7Button.SetActive(GameManager.stage6 == 1);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    public void Stage1()
    {
        SceneManager.LoadScene(2);
    }

    public void Stage2()
    {
        SceneManager.LoadScene(3);
    }

    public void Stage3()
    {
        SceneManager.LoadScene(4);
    }

    public void Stage4()
    {
        SceneManager.LoadScene(5);
    }

    public void Stage5()
    {
        SceneManager.LoadScene(6);
    }

    public void Stage6()
    {
        SceneManager.LoadScene(7);
    }

    public void Stage7()
    {
        SceneManager.LoadScene(8);
    }
}