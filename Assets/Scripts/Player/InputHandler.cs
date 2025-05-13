using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _attackKey = KeyCode.F;

    private string _horizontalAxis = "Horizontal";

    public event Action<Vector2> MoveCommand;
    public event Action JumpCommand;
    public event Action ActionCommand;

    private void Update()
    {
        Movement();
        Jump();
        Attack();
    }

    private void Movement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw(_horizontalAxis), 0);
        MoveCommand?.Invoke(moveInput);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(_jumpKey))
            JumpCommand?.Invoke();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(_attackKey))
            ActionCommand?.Invoke();
    }
}