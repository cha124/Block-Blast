using UnityEngine;

public class TopPaddle : MonoBehaviour
{
    public Transform bottomPaddle; // 下パドル参照
    public float yOffset = 8f;     // 上にどれだけ離すか

    void Update()
    {
        if (bottomPaddle == null) return;

        // Xだけ連動、Yは固定
        transform.position = new Vector3(
            bottomPaddle.position.x,
            yOffset,
            transform.position.z
        );
    }
}