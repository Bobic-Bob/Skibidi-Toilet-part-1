using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class EnemyAnimator : MonoBehaviour
{

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayClick()
    {
        _animator.SetTrigger("click");
    }
}
