using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlash : MonoBehaviour
{
    public enum TypeOfFlash
    {
        FLASH,
        BLINK
    }

    public TypeOfFlash typeOfFlash = TypeOfFlash.FLASH;

    // Flash variables

    public Color flashColor = Color.red; // Color RGBA
    public float flashDuration = 0.2f; // seconds

    // Blink variables

    public float blinkDuration = 0.1f; // seconds

    // Shake variables

    public bool shouldShake = true;
    public float shakeSpeed = 80f; //how fast it shakes
    public float shakeStrength = 0.1f; //how much it shakes
    


    private SpriteRenderer spriteRenderer;

    private bool isFlashing = false;
    private bool isBlinking = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        /*
        // DEBUG: Input used for test only
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flash();
        }
        */

        // DEBUG: Input used for test only
        /*if (Keyboard.current.spaceKey.wasPressedThisFrame && !isFlashing && !isBlinking)
        {
            Flash();
        }*/
    }

    /// <summary>
    /// Calls the DoFlash() or DoBlink() functions to make the current GameObject's SpriteRenderer flash or blink
    /// Call this function from the CharacterController Interaction Input (or from wherever you want)
    /// </summary>
    public void Flash()
    {
        switch (typeOfFlash)
        {
            case TypeOfFlash.FLASH:
                StartCoroutine(DoFlash());
                break;

            case TypeOfFlash.BLINK:
                StartCoroutine(DoBlink());
                break;
        }
    }

    /// <summary>
    /// Make the character flash with color
    /// </summary>
    IEnumerator DoFlash()
    {
        if (flashDuration <= 0f)
        {
            Debug.LogError("The flash duration must be higher than 0");
            yield break;
        }

        isFlashing = true;

        Color originalColor = spriteRenderer.color;

        spriteRenderer.color = flashColor;

        float timeElasped = 0f;

        if (shouldShake)
        {
            Vector3 originalPosition = transform.position;

            while (timeElasped < flashDuration)
            {
                timeElasped += Time.deltaTime;

                if ((int)(timeElasped * 100) % 2 == 0)
                {
                    transform.position = originalPosition
                                       + new Vector3(Mathf.Sin(Time.time * shakeSpeed) * shakeStrength,
                                                     Mathf.Sin(Time.time * shakeSpeed) * shakeStrength,
                                                     0);
                }
                else
                {
                    transform.position = originalPosition
                                       + new Vector3(-(Mathf.Sin(Time.time * shakeSpeed)) * shakeStrength,
                                                     (Mathf.Sin(Time.time * shakeSpeed)) * shakeStrength,
                                                     0);
                }

                yield return new WaitForEndOfFrame();
            }

            transform.position = originalPosition;
        }
        else
        {
            yield return new WaitForSeconds(flashDuration);
        }
        
        spriteRenderer.color = originalColor;
        
        isFlashing = false;
    }

    /// <summary>
    /// Make the character blink with opacity
    /// </summary>
    IEnumerator DoBlink()
    {
        if (blinkDuration <= 0f)
        {
            Debug.LogError("The blink duration must be higher than 0");
            yield break;
        }

        isFlashing = true;

        Color originalColor = spriteRenderer.color;

        spriteRenderer.color = new Color(spriteRenderer.color.r,
                                         spriteRenderer.color.g,
                                         spriteRenderer.color.b,
                                         0);

        yield return new WaitForSeconds(blinkDuration);

        spriteRenderer.color = originalColor;
        
        isFlashing = false;
    }
}

[CustomEditor(typeof(SpriteFlash))]
public class SpriteFlashEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SpriteFlash t = target as SpriteFlash;

        t.typeOfFlash = (SpriteFlash.TypeOfFlash)EditorGUILayout.EnumPopup("Type of Flash", t.typeOfFlash);

        switch (t.typeOfFlash)
        {
            case SpriteFlash.TypeOfFlash.FLASH:
                t.flashColor = EditorGUILayout.ColorField("Flash Color", t.flashColor);
                t.flashDuration = EditorGUILayout.FloatField("Flash Duration", t.flashDuration);
                t.shouldShake = EditorGUILayout.Toggle("Should Shake", t.shouldShake);

                if (t.shouldShake)
                {
                    t.shakeSpeed = EditorGUILayout.FloatField("Shake Speed", t.shakeSpeed);
                    t.shakeStrength = EditorGUILayout.FloatField("Shake Strength", t.shakeStrength);
                }

                break;

            case SpriteFlash.TypeOfFlash.BLINK:
                t.blinkDuration = EditorGUILayout.FloatField("Blink Duration", t.blinkDuration);
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(t);
        }

    }
}

