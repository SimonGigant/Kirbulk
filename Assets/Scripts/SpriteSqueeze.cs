using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteSqueeze : MonoBehaviour
{
    enum SqueezeDirection
    {
        HORIZONTAL,
        VERTICAL
    }

    [SerializeField] SqueezeDirection squeezeDirection = SqueezeDirection.HORIZONTAL;

    [SerializeField] AnimationCurve animationCurve; 

    [SerializeField] float squeezeDuration = 1f;

    private bool isSqueezing = false;
    
    void Update()
    {
        // DEBUG: Input used for test only
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isSqueezing)
        {
            Squeeze();
        }
    }

    public void Squeeze()
    {
        StartCoroutine(DoSqueeze());
    }

    IEnumerator DoSqueeze()
    {
        float timeElapsed = 0f;

        while (timeElapsed < squeezeDuration)
        {
            float squeezeScale = animationCurve.Evaluate(timeElapsed / squeezeDuration);

            switch (squeezeDirection)
            {
                case SqueezeDirection.HORIZONTAL:
                    transform.localScale = new Vector3(transform.localScale.x, squeezeScale, transform.localScale.z);
                    break;

                case SqueezeDirection.VERTICAL:
                    transform.localScale = new Vector3(squeezeScale, transform.localScale.y, transform.localScale.z);
                    break;
            }

            timeElapsed += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield break;
    }
}
