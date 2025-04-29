using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class AnimationHandler : MonoBehaviour
{
    private static readonly int s_isJumpingHash = Animator.StringToHash("isJumping");
    private static readonly int s_isRunningHash = Animator.StringToHash("isRunning");
    private static readonly int s_isAttackingHash = Animator.StringToHash("isAttacking");
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateJumpState(bool isJumping)
    {
        _animator.SetBool(s_isJumpingHash, isJumping);
    }

    public void UpdateRunState(bool isRunning)
    {
        _animator.SetBool(s_isRunningHash, isRunning);
    }

    public void SetAttackState(bool isAttacking)
    {
        _animator.SetBool(s_isAttackingHash, isAttacking);
    }
}