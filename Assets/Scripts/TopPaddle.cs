using UnityEngine;

public class TopPaddle : MonoBehaviour
{
    public Transform bottomPaddle; // 下パドル参照
    public float yOffset = 8f;     // 上にどれだけ離すか
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }
    void Update()
    {
        if (bottomPaddle == null) return;

        // Xだけ連動、Yは固定
        transform.position = new Vector3(
            bottomPaddle.position.x,
            yOffset,
            transform.position.z
        );

        if (Paddle.isPowered_large_paddle)
        {
            // 最初の1回だけ2倍にする
            if (transform.localScale == originalScale)
            {
                transform.localScale = originalScale * 2f;
            }
        }else{
                transform.localScale = originalScale;
            }
    }
}