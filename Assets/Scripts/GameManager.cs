using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject restartButton;
    public GameObject titleButton;
    private bool isGameOver = false;

    public GameObject ballPrefab;
    public Transform paddle;

    public static int life = 3;
    public static int sceneIndex;
    public LifeUI lifeUI;

    public static int stage1 = 0;
    public static int stage2 = 0;
    public static int stage3 = 0;
    public static int stage4 = 0;
    public static int stage5 = 0;
    public static int stage6 = 0;
    public static int stage7 = 0;

    void Start()
    {
        life = 3;
        if (lifeUI != null)
        {
            lifeUI.UpdateLife(life);
        }

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        LoadStageData();

        
        restartButton.SetActive(false);
        titleButton.SetActive(false);
        

        SpawnBall();
    }

    void Update()
    {

        if (isGameOver) return;

        if (GameObject.FindGameObjectsWithTag("Block").Length == 0)
        {
            NextGame();
        }

        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
        {
            life--;
            if (lifeUI != null)
            {
                lifeUI.UpdateLife(life);
            }

            Debug.Log("Life: " + life);

            if (life <= 0)
            {
                GameOver();
                return;
            }

            SpawnBall();
        }
    }

    void SpawnBall()
    {
        if (ballPrefab == null || paddle == null)
        {
            Debug.LogError("ballPrefab または paddle が未設定");
            return;
        }

        GameObject newBall = Instantiate(ballPrefab);

        Ball ballScript = newBall.GetComponent<Ball>();
        if (ballScript != null)
        {
            ballScript.SetLaunched(false);
            ballScript.SetPaddle(paddle);
        }

        Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("Game Over");

        if (restartButton != null)
        {
            restartButton.SetActive(true);
            titleButton.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void NextStage()
    {
        NextGame();
    }

    public void NextGame()
    {
        SaveCurrentStageAsCleared();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void SaveCurrentStageAsCleared()
    {
        int stageNumber = SceneManager.GetActiveScene().buildIndex - 1; // stage1=2番目のシーン想定

        switch (stageNumber)
        {
            case 1:
                stage1 = 1;
                PlayerPrefs.SetInt("Stage1", 1);
                break;
            case 2:
                stage2 = 1;
                PlayerPrefs.SetInt("Stage2", 1);
                break;
            case 3:
                stage3 = 1;
                PlayerPrefs.SetInt("Stage3", 1);
                break;
            case 4:
                stage4 = 1;
                PlayerPrefs.SetInt("Stage4", 1);
                break;
            case 5:
                stage5 = 1;
                PlayerPrefs.SetInt("Stage5", 1);
                break;
            case 6:
                stage6 = 1;
                PlayerPrefs.SetInt("Stage6", 1);
                break;
            case 7:
                stage7 = 1;
                PlayerPrefs.SetInt("Stage7", 1);
                break;
        }

        PlayerPrefs.Save();
    }

    void LoadStageData()
    {
        stage1 = PlayerPrefs.GetInt("Stage1", stage1);
        stage2 = PlayerPrefs.GetInt("Stage2", stage2);
        stage3 = PlayerPrefs.GetInt("Stage3", stage3);
        stage4 = PlayerPrefs.GetInt("Stage4", stage4);
        stage5 = PlayerPrefs.GetInt("Stage5", stage5);
        stage6 = PlayerPrefs.GetInt("Stage6", stage6);
        stage7 = PlayerPrefs.GetInt("Stage7", stage7);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}