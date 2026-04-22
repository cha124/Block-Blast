using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    private bool isLaunched = false;
    public Transform paddle;

    private TrailRenderer trail;

    // ▼ 強化関連
    private bool isStrong = false;
    private float strongTimer = 0f;
    public float strongDuration = 5f;

    private SpriteRenderer sr;
    public Color normalColor = Color.white;
    public Color strongColor = Color.red;

    private Vector2 prevVelocity;

    // ▼ 範囲
    public float limitX = 6f;
    public float limitY = 9f;
    public AudioClip hitSound_1;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();

        speed = DifficultyManager.ballSpeed;

        trail.emitting = false;
    }

    void Update()
    {
        trail.emitting = isLaunched;

        // ▼ パドル追従
        if (!isLaunched)
        {
            if (paddle == null)
            {
                Debug.LogError("paddle が設定されていません");
                return;
            }

            transform.position = paddle.position + new Vector3(0, 0.6f, 0);
            rb.linearVelocity = Vector2.zero;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Launch();
            }
        }

        // ▼ 強化時間
        if (isStrong)
        {
            strongTimer += Time.deltaTime;

            if (strongTimer >= strongDuration)
            {
                isStrong = false;
                sr.color = normalColor;
            }
        }

        // ▼ 範囲外で削除
        if (IsOutOfBounds())
        {
            RemoveBall();
        }
    }

    void FixedUpdate()
    {
        prevVelocity = rb.linearVelocity;

        if (rb.linearVelocity.magnitude > 0.01f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ▼ ブロック貫通
        if (collision.gameObject.CompareTag("Block") && isStrong)
        {
            rb.linearVelocity = prevVelocity.normalized * speed;
            return;
        }

        // ▼ パドル反射
        if (collision.gameObject.CompareTag("Paddle") ||
            collision.gameObject.CompareTag("TopPaddle"))
        {
            AudioSource.PlayClipAtPoint(hitSound_1, transform.position);

            float diff = transform.position.x - collision.transform.position.x;
            float width = collision.collider.bounds.size.x;
            float t = diff / width;

            float maxAngle = 75f;
            float angle = t * maxAngle;

            float minAngle = 20f;
            if (Mathf.Abs(angle) < minAngle)
            {
                angle = Mathf.Sign(angle) * minAngle;
            }

            angle += Random.Range(-5f, 5f);

            float rad = angle * Mathf.Deg2Rad;

            // ▼ ★ここが重要
            float yDir = 1f;

            // 上パドルなら下方向へ
            if (collision.gameObject.CompareTag("TopPaddle"))
            {
                yDir = -1f;
            }

            Vector2 dir = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad) * yDir);

            rb.linearVelocity = dir.normalized * speed;
        }
    }

    void Launch()
    {
        isLaunched = true;

        float x = Random.Range(-0.5f, 0.5f);
        rb.linearVelocity = new Vector2(x, 1).normalized * speed;
    }

    bool IsOutOfBounds()
    {
        return transform.position.y < -limitY ||
               transform.position.y > limitY ||
               transform.position.x < -limitX ||
               transform.position.x > limitX;
    }

    void RemoveBall()
    {
        Destroy(gameObject);
    }

    // ▼ 外部用
    public void SetLaunched(bool value)
    {
        isLaunched = value;
    }

    public void SetPaddle(Transform p)
    {
        paddle = p;
    }

    public void ActivateStrong()
    {
        isStrong = true;
        strongTimer = 0f;
        sr.color = strongColor;
    }

    public bool IsStrong()
    {
        return isStrong;
    }
}