using UnityEngine;

public class Block : MonoBehaviour
{
    public int maxHp = 1;
    private int hp;

    public Sprite[] damageSprites;
    public AudioSource audioSource;
    public AudioClip hitSound_1;
    public AudioClip hitSound_2;

    private SpriteRenderer sr;

    // ▼ アイテム関連
    public GameObject[] itemPrefabs;
    [Range(0f, 1f)]
    public float dropChance = 0.3f;

    // ▼ ブロックごとの反射角
    [Header("Ball Reflection")]
    [Range(0f, 89f)]
    public float reflectMinAngle = 15f;

    [Range(0f, 89f)]
    public float reflectMaxAngle = 75f;

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
        AudioSource.PlayClipAtPoint(hitSound_1, transform.position);

        hp -= damage;

        if (damageSprites != null && damageSprites.Length > 0)
        {
            int index = maxHp - hp - 1;

            if (index >= 0 && index < damageSprites.Length)
            {
                sr.sprite = damageSprites[index];
            }
        }

        if (hp <= 0)
        {
            if (maxHp == 2)
            {
                AudioSource.PlayClipAtPoint(hitSound_2, transform.position);
            }

            TryDropItem();
            Destroy(gameObject);
        }
    }
}