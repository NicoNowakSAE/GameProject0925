using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private float chillCD = 2;
    private float curChillCD = 2;

    private float rotationSpeed = 2;

    public void AnimationStep(PlayerInput input)
    {
        _animator.SetFloat("Blend", input.Move.ReadValue<Vector2>().magnitude);

        if(input.Move.ReadValue<Vector2>().x > 0.1f)
        {
            _animator.transform.rotation = Quaternion.Euler(0, 90, 0);
            curChillCD = chillCD;
        }
        else if (input.Move.ReadValue<Vector2>().x < -0.1f)
        {
            _animator.transform.rotation = Quaternion.Euler(0, -90, 0);
            curChillCD = chillCD;
        }
        else
        {
            curChillCD -= Time.deltaTime;
        }

        if(curChillCD <= 0)
        {
            _animator.transform.rotation = Quaternion.Lerp(_animator.transform.rotation, Quaternion.Euler(0, -180, 0), 5 * Time.deltaTime);
        }
        _animator.SetBool("isHappy", curChillCD <= 0);

    }
}
