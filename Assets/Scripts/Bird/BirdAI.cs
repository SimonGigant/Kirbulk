using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAI : MonoBehaviour
{
    [SerializeField] GameObject birdDeathParticles;

    private Vector3 direction;

    private float speed;
    private float timeElapsed;

    void Start()
    {
        speed = Random.Range(2f, 4f);

        if (transform.position.x > 0) // RIGHT to LEFT
        {
            GetComponent<SpriteRenderer>().flipX = true;
            direction = Vector3.left;
        }
        else // LEFT to RIGHT
        {
            direction = Vector3.right;
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > 2f)
        {
            timeElapsed -= 2f;
            
            direction = (direction + Vector3.up * Random.Range(-0.4f, 0.4f)).normalized;
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, direction);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(birdDeathParticles, transform.position, Quaternion.identity);
            
            if (!SoundManager.IsEventPlayingOnGameObject("Play_SFX_Butterfly_Explosion", gameObject))
                SoundManager.PlaySound("Play_SFX_Butterfly_Explosion", gameObject);

            Destroy(gameObject);
        }
    }
}
