using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MaraveState { Idle, Cutscene }
public enum ActionState { None = 0, CarryWatercan = 1, Water = 2 }

public class MaraveController : MonoBehaviour
{
    //Values
    private float moveSpeed = 15f;
    private Rigidbody2D rb;
    private SpriteRenderer rend;
    private Animator animator;
    private ActionState prevActionState;

    [HideInInspector] public MaraveState state;
    private ActionState actionState;

    private void Start()
    {
        state = MaraveState.Idle;
        actionState = ActionState.None;
        prevActionState = actionState;
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        InputManagement();
        ChangeAnimator();
    }

    private void ChangeAnimator()
    {
        if (Keyboard.current.digit1Key.isPressed)
            actionState = ActionState.CarryWatercan;
        if (Keyboard.current.digit2Key.isPressed)
            actionState = ActionState.Water;

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

    public Vector2 Position2D()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    //moveValue must have a norm <= 1
    private void Move(Vector2 moveValue)
    {
        if (moveValue.sqrMagnitude > 1f)
            moveValue.Normalize();

        rb.MovePosition(Position2D() + moveValue * Time.deltaTime * moveSpeed);
    }

}
