using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
        if (transform.position.y > 9f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block") || other.CompareTag("unbreak"))
        {
            Block block = other.GetComponent<Block>();

            if (block != null)
            {
                block.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }
}