using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampFall : MonoBehaviour
{
    private bool lampHasFallen = false;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!lampHasFallen)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                float zRotation = (collision.gameObject.transform.position.x > transform.parent.position.x ? 80f : -80f );

                transform.parent.localEulerAngles = new Vector3(transform.parent.localEulerAngles.x, transform.parent.localEulerAngles.y, zRotation);

                // A tester in-game hein
            }
        }
    }
}
