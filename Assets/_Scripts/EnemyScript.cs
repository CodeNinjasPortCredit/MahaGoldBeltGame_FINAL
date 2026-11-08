using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public int damage = 10;

    private void Update()
    {
        if (player == null) return;

        // Move toward player
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );

        // Face player (2D)
        Vector3 direction = player.position - transform.position;
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(direction.x),
                1,
                1
            );
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();

            //if (health != null)
           // {
             //   health.TakeDamage(damage);
            //}
        }
    }
}