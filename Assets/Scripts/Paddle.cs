using UnityEngine;//using UnityEngine;は初めに書く。はじめのうちは何も考えずつけてOK

public class Paddle : MonoBehaviour//MonoBehaviourは基本的には継承(クラス名の後ろに: MonoBehaviourをつける)する。はじめのうちは何も考えずつけてOK
{
    public float speed = 10f;//速さを設定
    private Rigidbody2D rb;//物理挙動を扱うコンポーネントRidgebody2D

    //円軌道のパドル
    public Transform center;   // 回転の中心
    public float speed_angle = 100f; // 回転速度
   
    
    public GameObject bulletPrefab;
    public GameObject ballPrefab;
    public Transform firePoint;

    public Sprite normalSprite;
    public Sprite poweredSprite;

    private SpriteRenderer sr;

    private bool isPowered_bullet = false;
    private bool isPowered_strong_ball = false;
    private bool isPowered_splitting_ball = false;
    public static bool isPowered_large_paddle = false;
    public float fireInterval;
    private float timer = 0f;
    public AudioClip hitSound;
    private int bullet_counts = 0;
    public int max_bullt_counts = 15;
    private Vector3 originalScale;

    public static float largeTimer = 5f;
    public static float largeDuration = 5f; // 持続時間
    

    void Start()//ゲーム開始時に一度だけ呼ばれる
    {
        rb = GetComponent<Rigidbody2D>();//Ridgebody2Dを取得
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    void Update()//毎フレーム実行される
    {
        if(GameManager.sceneIndex != 8)
        {
            float x = Input.GetAxis("Horizontal");//左右キー入力を取得　←またはAでxに-1が代入　→またはDでxに1が代入　何も押していないときxに0が代入
            rb.linearVelocity = new Vector2(x * speed, 0);//Vector2(横方向の速度,縦方向の速度)となる。今回は縦方向の速度は0

        }else{
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.RotateAround(center.position, Vector3.forward, speed_angle * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.RotateAround(center.position, Vector3.forward, -speed_angle * Time.deltaTime);

            }
        }
       
        if (isPowered_bullet)
        {
            timer += Time.deltaTime;

            if (timer >= fireInterval && bullet_counts <= max_bullt_counts)
            {
                Fire();
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
                timer = 0f;
                bullet_counts++;
            }else if(bullet_counts > max_bullt_counts)
            {
                isPowered_bullet = false;
                sr.sprite = normalSprite;
                bullet_counts = 0;
            }
        }

        if (isPowered_splitting_ball)
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            foreach (GameObject ball in balls)
            {
                Rigidbody2D rbBall = ball.GetComponent<Rigidbody2D>();
                Ball ballScript = ball.GetComponent<Ball>();
                // 元ボールのpaddleを取得
                Transform paddleTransform = ballScript.paddle;

                if (rbBall == null || ballScript == null) continue;

                Vector2 velocity = rbBall.linearVelocity;

                // 速度がほぼ0のときの保険
                if (velocity.magnitude < 0.1f)
                {
                    velocity = Vector2.up;
                }

                Vector2 dir = velocity.normalized;

                // 角度をずらす
                Vector2 newDir1 = Quaternion.Euler(0, 0, 20f) * dir;
                Vector2 newDir2 = Quaternion.Euler(0, 0, -20f) * dir;

                // 新しいボール生成
                GameObject ball1 = Instantiate(ballPrefab, ball.transform.position, Quaternion.identity);
                GameObject ball2 = Instantiate(ballPrefab, ball.transform.position, Quaternion.identity);

                Rigidbody2D rb1 = ball1.GetComponent<Rigidbody2D>();
                Rigidbody2D rb2 = ball2.GetComponent<Rigidbody2D>();

                Ball b1 = ball1.GetComponent<Ball>();
                Ball b2 = ball2.GetComponent<Ball>();

                if (rb1 != null && b1 != null)
                {
                    rb1.linearVelocity = newDir1.normalized * b1.speed;
                    b1.SetLaunched(true);

                    b1.paddle = paddleTransform; // ★追加
                }

                if (rb2 != null && b2 != null)
                {
                    rb2.linearVelocity = newDir2.normalized * b2.speed;
                    b2.SetLaunched(true);

                    b2.paddle = paddleTransform; // ★追加
                }
            }

            isPowered_splitting_ball = false;
        }

        if (isPowered_large_paddle)
        {
            // 最初の1回だけ2倍にする
            if (transform.localScale == originalScale)
            {
                transform.localScale = originalScale * 2f;
            }

            largeTimer += Time.deltaTime;

            if (largeTimer >= largeDuration)
            {
                transform.localScale = originalScale;
                isPowered_large_paddle = false;
                largeTimer = 0f;
            }
        }

    }

    void Fire()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }

    public void ActivatePower(ItemType type)
    {
        switch (type)
        {
            case ItemType.Bullet:
                isPowered_bullet = true;
                sr.sprite = poweredSprite;
                break;

            case ItemType.SplittingBall:
                isPowered_splitting_ball = true;
                break;

            case ItemType.StrongBall:
                GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

                foreach (GameObject ball in balls)
                {
                    Ball b = ball.GetComponent<Ball>();
                    if (b != null)
                    {
                        b.ActivateStrong();
                    }
                }
                break;

            case ItemType.LargePaddle:
                largeTimer -= 5;
                isPowered_large_paddle = true;
                break;
        }
    }

}