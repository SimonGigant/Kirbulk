using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShake : MonoBehaviour
{
    [SerializeField] private float duration = 0.05f;
    [SerializeField] private float shakeMagnitude = 10f;
    [SerializeField] private AnimationCurve curve;

    private bool isShaking = false;

    public void Shake()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeUI());
        }
    }

    public void RotateShake()
    {
        if (!isShaking)
        {
            StartCoroutine(RotateUI());
        }
    }
    public void RotateShake(float _duration, float _magnitude)
    {
        if (!isShaking)
        {
            duration = _duration;
            shakeMagnitude = _magnitude;
            StartCoroutine(RotateUI());
        }
    }

    private IEnumerator ShakeUI()
    {
        isShaking = true;
        float counter = 0f;
        Vector3 positionChangeShake = Vector3.zero;
        for(; ; )
        {
            if (Gamefeel.Instance.IsInFreeze())
            {
                yield return null;
            }
            counter += Time.deltaTime;
            Vector2 change2D = Random.insideUnitCircle * curve.Evaluate(counter / duration) * shakeMagnitude;
            change2D.x = 0f;
            Vector3 change = new Vector3(change2D.x, change2D.y, 0f);
            transform.position += change - positionChangeShake;
            positionChangeShake = change;

            if (counter > duration)
            {
                transform.position -= positionChangeShake;
                break;
            }
            else
            {
                yield return null;
            }
        }
        isShaking = false;
    }

    private IEnumerator RotateUI()
    {
        isShaking = true;
        float counter = 0f;
        Vector3 positionChangeShake = Vector3.zero;
        for (; ; )
        {
            if (Gamefeel.Instance.IsInFreeze())
            {
                yield return null;
            }
            counter += Time.deltaTime;
            float rotationChange = curve.Evaluate(counter / duration) * shakeMagnitude;
            Vector3 change = new Vector3(0f, 0f, rotationChange);
            transform.localEulerAngles += change - positionChangeShake;
            positionChangeShake = change;

            if (counter > duration)
            {
                transform.position -= positionChangeShake;
                break;
            }
            else
            {
                yield return null;
            }
        }
        isShaking = false;
    }
}
