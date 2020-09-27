using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampFall : MonoBehaviour
{
    private bool lampHasFallen = false;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!lampHasFallen)
            {
                float zRotation = (collision.gameObject.transform.position.x > transform.parent.position.x ? 93.5f : -93.5f);

                transform.parent.localEulerAngles = new Vector3(transform.parent.localEulerAngles.x, transform.parent.localEulerAngles.y, zRotation);

                lampHasFallen = true;
                
                if (!SoundManager.IsEventPlayingOnGameObject("Play_SFX_Lamp_Ground_Impact", gameObject))
                    SoundManager.PlaySound("Play_SFX_Lamp_Ground_Impact", gameObject);
            }

            GetComponent<SpriteSqueeze>().Squeeze();

            if (!SoundManager.IsEventPlayingOnGameObject("Play_SFX_Lamp_Bump", gameObject))
                SoundManager.PlaySound("Play_SFX_Lamp_Bump", gameObject);
        }
    }
}
