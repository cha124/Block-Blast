using UnityEngine;

public class VerticalPaddlePair : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody2D rb;

    // ▼ 操作キー
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;

    // ▼ もう片方のパドル
    public Transform pairedPaddle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float y = Input.GetAxis("Vertical");
        // ▼ 自分を動かす
        rb.linearVelocity = new Vector2(0, y * speed);

        // ▼ もう片方を追従させる（位置だけコピー）
        if (pairedPaddle != null)
        {
            Vector3 pos = pairedPaddle.position;
            pos.y = transform.position.y;
            pairedPaddle.position = pos;
        }
    }
}