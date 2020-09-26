using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MaraveState { Idle, Cutscene }

public class MaraveController : MonoBehaviour
{
    //Values
    private float moveSpeed = 15f;
    private Rigidbody2D rb;
    private SpriteRenderer rend;

    [HideInInspector] public MaraveState state;

    private void Start()
    {
        state = MaraveState.Idle;
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        
    }

    private void Update()
    {
        InputManagement();
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
            OnMove(keyboardDirection);
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

            Move(inputValue);
        }
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
