using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum MaraveState { Idle, Cutscene }
public enum ActionState { None = 0, CarryWatercan = 1, Water = 2, Pet = 3, KneeDrop = 4 }

public class MaraveController : MonoBehaviour
{
    //Values
    private float moveSpeed = 20f;
    private Rigidbody2D rb;
    private SpriteRenderer rend;
    private Animator animator;
    private ParticleSystem particleSys;
    private ActionState prevActionState;

    [HideInInspector] public MaraveState state;
    private ActionState actionState;
    public bool Mode_grosBiscotos = true;

    private void Start()
    {
        state = MaraveState.Idle;
        actionState = ActionState.None;
        prevActionState = actionState;
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        particleSys = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        InputManagement();
        ChangeAnimator();
        if (Keyboard.current.enterKey.isPressed)
            UnlockWatercan();
        if (Keyboard.current.backspaceKey.isPressed)
            RemoveWatercan();
        if (Keyboard.current.rKey.isPressed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ChangeAnimator()
    {
        bool pressedAction = ActionPressedButton(out bool release);
        if (pressedAction && actionState == ActionState.CarryWatercan)
        {
            actionState = ActionState.Water;
        }
        else if(release && actionState == ActionState.Water)
        {
            actionState = ActionState.CarryWatercan;
        }

        if (actionState == ActionState.Water && !particleSys.isPlaying)
            particleSys.Play();
        else if (particleSys.isPlaying)
            particleSys.Stop();
        
        if (actionState != prevActionState)
            animator.SetTrigger("ChangingAction");
        animator.SetInteger("Action", (int)actionState);

        prevActionState = actionState;
    }

    private void InputManagement()
    {

        Vector2 keyboardDirection;
        float upValue = (Keyboard.current.wKey.isPressed || Keyboard.current.zKey.isPressed) ? 1f : 0f;
        float downValue = Keyboard.current.sKey.isPressed ? 1f : 0f;
        float leftValue = (Keyboard.current.qKey.isPressed || Keyboard.current.aKey.isPressed) ? 1f : 0f;
        float rightValue = Keyboard.current.dKey.isPressed ? 1f : 0f;
        keyboardDirection.x = rightValue - leftValue;
        keyboardDirection.y = upValue - downValue;
        if(keyboardDirection.x != 0f || keyboardDirection.y != 0f)
        {
            OnMove(keyboardDirection);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    private void OnMove(Vector2 inputValue)
    {
        if(state == MaraveState.Idle)
        {
            if(inputValue.x != 0f)
            {
                bool right = inputValue.x >= 0f;
                rend.flipX = !right;
            }
            animator.SetFloat("Speed", inputValue.magnitude);

            Move(inputValue);
        }
    }

    public bool ActionPress()
    {
        return Keyboard.current.spaceKey.isPressed;
    }

    private bool pressedLastFrame = false;
    public bool ActionPressedButton(out bool release)
    {
        release = false;
        bool res = false;
        if(!pressedLastFrame && ActionPress())
        {
            res = true;
            pressedLastFrame = true;
        }
        else if(pressedLastFrame && !ActionPress())
        {
            release = true;
        }
        pressedLastFrame = ActionPress();
        return res;
    }

    public Vector2 Position2D()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public void UnlockWatercan()
    {
        actionState = ActionState.CarryWatercan;
    }

    public void RemoveWatercan()
    {
        actionState = ActionState.None;
    }

    public void Pet(bool right)
    {
        rend.flipX = !right;
        actionState = ActionState.Pet;
        state = MaraveState.Cutscene;
        StartCoroutine(DelayBeforeComingBackToIdle(4f));
        animator.SetTrigger("ChangingAction");
    }

    public void ElbowDrop(bool right)
    {
        rend.flipX = !right;
        actionState = ActionState.KneeDrop;
        state = MaraveState.Cutscene;
        StartCoroutine(DelayBeforeComingBackToIdle(4f));
        animator.SetTrigger("ChangingAction");
    }

    private IEnumerator DelayBeforeComingBackToIdle(float duration)
    {
        yield return new WaitForSeconds(duration);
        state = MaraveState.Idle;
    }

    //moveValue must have a norm <= 1
    private void Move(Vector2 moveValue)
    {
        if (moveValue.sqrMagnitude > 1f)
            moveValue.Normalize();

        rb.MovePosition(Position2D() + moveValue * Time.deltaTime * moveSpeed);
    }

}
