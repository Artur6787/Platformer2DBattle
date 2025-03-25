using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private const string IsJumping = "isJumping";
    private const string IsRunning = "isRunning";
    private const string IsAttacking = "isAttacking";
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateJumpState(bool isJumping)
    {
        _animator.SetBool(IsJumping, isJumping);
    }

    public void UpdateRunState(bool isRunning)
    {
        _animator.SetBool(IsRunning, isRunning);
    }

    public void SetAttackState(bool isAttacking)
    {
        _animator.SetBool(IsAttacking, isAttacking);
    }
}