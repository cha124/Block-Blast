using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject restartButton;
    private bool isGameOver = false;

    public GameObject ballPrefab;
    public Transform paddle;

    public int life = 3;
    public static int sceneIndex;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }

        SpawnBall();
    }

    void Update()
    {
        if (isGameOver) return;

        // ▼ ステージクリア
        if (GameObject.FindGameObjectsWithTag("Block").Length == 0)
        {
            NextStage();
        }

        // ▼ ボールが無いとき
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
        {
            life--;

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
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void NextStage()
    {
        Debug.Log("Stage Clear!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}