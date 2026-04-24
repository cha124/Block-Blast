using UnityEngine;

 public enum ItemType
    {
        Bullet,
        SplittingBall,
        StrongBall,
        LargePaddle
    
    }
public class Item : MonoBehaviour
{

    public ItemType itemType;

    public float fallSpeed = 2f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * fallSpeed;
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Paddle"))
        {
            Paddle p = other.GetComponent<Paddle>();

            if (p != null)
            {
                p.ActivatePower(itemType);
            }

            Destroy(gameObject);
        }
    }
}