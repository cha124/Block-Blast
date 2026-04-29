using UnityEngine;
using System.Collections.Generic;

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

    // ▼ 狙い撃ち表示
    [Header("Aim Preview")]
    public LineRenderer aimLine;          // 点線表示用
    public LayerMask aimMask;             // Block / Wall などを入れる
    public float aimMinAngle = 15f;       // 上下に対する下限
    public float aimMaxAngle = 75f;       // 上下に対する上限
    public int aimMaxBounces = 5;         // 何回反射先まで予測するか
    public float aimRayLength = 30f;      // 予測距離
    public float aimOffset = 0.05f;       // 自分自身への誤ヒット回避

    // ▼ ブロック反射角のデフォルト
    [Header("Block Reflection")]
    public float defaultBlockMinAngle = 15f;
    public float defaultBlockMaxAngle = 75f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();

        if(GameManager.sceneIndex != 8){
            speed = DifficultyManager.ballSpeed;
        }else{
            speed = DifficultyManager.ballSpeed*0.7f;
        }
        

        trail.emitting = false;

        if (aimLine != null)
        {
            aimLine.positionCount = 0;
            aimLine.enabled = false;
        }
    }

    void Update()
    {
        trail.emitting = isLaunched;

        // ▼ 発射前：パドル追従 + 狙い線表示
        if (!isLaunched)
        {
            if (paddle == null)
            {
                Debug.LogError("paddle が設定されていません");
                return;
            }

            if (GameManager.sceneIndex != 8)
            {
                transform.position = paddle.position + new Vector3(0, 0.6f, 0);
            }
            else
            {
                transform.position = paddle.position * 0.9f;
            }

            rb.linearVelocity = Vector2.zero;

            UpdateAimLine();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Launch();
            }
        }
        else
        {
            if (aimLine != null) aimLine.enabled = false;
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

        // ▼ ブロック反射角制御
        if (collision.gameObject.CompareTag("Block"))
        {
            if (collision.contactCount > 0)
            {
                Vector2 normal = collision.contacts[0].normal;
                Vector2 dir = Vector2.Reflect(prevVelocity.normalized, normal).normalized;

                Block block = collision.gameObject.GetComponent<Block>();

                float minA = defaultBlockMinAngle;
                float maxA = defaultBlockMaxAngle;

                if (block != null)
                {
                    minA = block.reflectMinAngle;
                    maxA = block.reflectMaxAngle;
                }

                dir = ClampDirectionFromVertical(dir, minA, maxA);
                rb.linearVelocity = dir * speed;
            }

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

            float yDir = 1f;
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

        Vector2 dir = GetAimDirection();
        rb.linearVelocity = dir * speed;

        if (aimLine != null)
        {
            aimLine.enabled = false;
        }
    }

    Vector2 GetAimDirection()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            return Vector2.up;
        }

        Vector3 mouseWorld = cam.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z)
        );

        Vector2 origin = transform.position;
        Vector2 dir = (Vector2)mouseWorld - origin;
        // ▼ 上下方向の制御（下には撃たない）
        float ySign = 1f;
        if (paddle != null && paddle.CompareTag("TopPaddle"))
        {
            ySign = -1f;
        }

        dir.Normalize();

        // ▼ 完全水平を防ぐ（←重要）
        if (Mathf.Abs(dir.y) < 0.05f)
        {
            dir.y = 0.05f * ySign;
            dir.Normalize();
        }

        // ▼ 上方向に補正
        dir.y = Mathf.Abs(dir.y) * ySign;

        return dir.normalized;
    }

    void UpdateAimLine()
    {
        if (aimLine == null) return;

        Vector2 origin = transform.position;
        Vector2 dir = GetAimDirection();

        List<Vector3> points = new List<Vector3>();
        points.Add(origin);

        Vector2 currentOrigin = origin;
        Vector2 currentDir = dir;

        float remainingDistance = aimRayLength;

        for (int i = 0; i < aimMaxBounces; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentOrigin, currentDir, remainingDistance, aimMask);

            if (hit.collider != null)
            {
                points.Add(hit.point);

                remainingDistance -= hit.distance;
                if (remainingDistance <= 0f) break;

                currentOrigin = hit.point + currentDir * aimOffset;
                currentDir = Vector2.Reflect(currentDir, hit.normal).normalized;
            }
            else
            {
                points.Add(currentOrigin + currentDir * remainingDistance);
                break;
            }
        }

        aimLine.enabled = true;
        aimLine.positionCount = points.Count;
        aimLine.SetPositions(points.ToArray());
    }

    Vector2 ClampDirectionFromVertical(Vector2 dir, float minAngle, float maxAngle)
    {
        float xSign = Mathf.Sign(dir.x);
        float ySign = Mathf.Sign(dir.y);

        if (xSign == 0f) xSign = 1f;
        if (ySign == 0f) ySign = 1f;

        float angle = Mathf.Atan2(Mathf.Abs(dir.x), Mathf.Abs(dir.y)) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(rad) * xSign, Mathf.Cos(rad) * ySign).normalized;
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