using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode attackKey = KeyCode.F;

    private Vector2 _moveCommand;

    public event Action<Vector2> OnMoveCommand;
    public event Action OnJumpCommand;
    public event Action OnActionCommand;

    private void Update()
    {
        _moveCommand = Vector2.zero;

        if (Input.GetKey(moveLeftKey))
        {
            _moveCommand.x = -1;
        }
        else if (Input.GetKey(moveRightKey))
        {
            _moveCommand.x = 1;
        }

        OnMoveCommand?.Invoke(_moveCommand);

        if (Input.GetKeyDown(jumpKey))
        {
            OnJumpCommand?.Invoke();
        }

        if (Input.GetKeyDown(attackKey))
        {
            OnActionCommand?.Invoke();
        }
    }
}