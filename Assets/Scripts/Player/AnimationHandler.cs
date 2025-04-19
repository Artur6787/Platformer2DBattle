using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsJumpingHash = Animator.StringToHash("isJumping");
    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");
    private static readonly int IsAttackingHash = Animator.StringToHash("isAttacking");
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateJumpState(bool isJumping)
    {
        _animator.SetBool(IsJumpingHash, isJumping);
    }

    public void UpdateRunState(bool isRunning)
    {
        _animator.SetBool(IsRunningHash, isRunning);
    }

    public void SetAttackState(bool isAttacking)
    {
        _animator.SetBool(IsAttackingHash, isAttacking);
    }
}