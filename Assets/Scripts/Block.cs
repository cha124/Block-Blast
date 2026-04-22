using UnityEngine;

public class Block : MonoBehaviour
{
    public int maxHp = 1; // 最大耐久
    private int hp;       // 現在の耐久

    public Sprite[] damageSprites; // ダメージ段階ごとの見た目
    public AudioClip hitSound_1;
    public AudioClip hitSound_2;

    private SpriteRenderer sr;

    // ▼ アイテム関連（ここが今回の重要ポイント）
    public GameObject[] itemPrefabs; // 複数のアイテムPrefabを登録
    [Range(0f, 1f)]
    public float dropChance = 0.3f; // ドロップ確率

    void Start()
    {
        hp = maxHp;
        sr = GetComponent<SpriteRenderer>();
        dropChance = DifficultyManager.dropRate;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            // ▼ 強化ボールなら一撃破壊
            if (ball.IsStrong())
            {
                TakeDamage(maxHp);
                return;
            }
        }

        // ▼ 通常ダメージ
        TakeDamage(1);
    }

    void TryDropItem()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0) return;

        if (Random.value < dropChance)
        {
            int index = Random.Range(0, itemPrefabs.Length);
            Instantiate(itemPrefabs[index], transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        // ▼ ヒット音
        AudioSource.PlayClipAtPoint(hitSound_1, transform.position);

        hp -= damage;

        // ▼ 見た目変更
        if (damageSprites != null && damageSprites.Length > 0)
        {
            int index = maxHp - hp - 1;

            if (index >= 0 && index < damageSprites.Length)
            {
                sr.sprite = damageSprites[index];
            }
        }

        // ▼ 破壊処理
        if (hp <= 0)
        {
            if (maxHp == 2)
            {
                AudioSource.PlayClipAtPoint(hitSound_2, transform.position);
            }

            TryDropItem(); // ★破壊時にドロップ
            Destroy(gameObject);
        }
    }
}