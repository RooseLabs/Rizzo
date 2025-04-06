using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationStateController : MonoBehaviour
{
    private Animator _animator;

    private float _idleTime;
    private float _idleAnimationTimer;

    private GameObject[] _weapons;

    private static class Parameter
    {
        public static readonly int Velocity = Animator.StringToHash("Velocity");
        public static readonly int IdleGroom = Animator.StringToHash("IdleGroom");
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _weapons = GameObject.FindGameObjectsWithTag("Weapon");
        _idleAnimationTimer = Random.Range(10f, 20f);
    }

    public void UpdateAnimation(Vector2 moveInput, Vector2 rgVelocity)
    {
        if (rgVelocity.magnitude == 0f)
        {
            _animator.SetFloat(Parameter.Velocity, 0f);
            _idleTime += Time.deltaTime;
            if (_idleTime >= _idleAnimationTimer)
            {
                _animator.SetTrigger(Parameter.IdleGroom);
                _idleTime = -139f / 30f; // Compensate for the animation length (Idle_Groom is 139 frames at 30 FPS)
                _idleAnimationTimer = Random.Range(10f, 20f);
            }
        }
        else
        {
            _animator.SetFloat(Parameter.Velocity, moveInput.magnitude);
            _idleTime = 0f;
        }
    }

    public void PlayAnimation(int animationHash)
    {
        _animator.Play(animationHash);
    }

    public void PlayAnimation(string animationName)
    {
        int hash = Animator.StringToHash(animationName);
        if (hash == -1)
        {
            Debug.LogWarning("Animation " + animationName + " not found.");
            return;
        }
        PlayAnimation(hash);
    }

    public void HideWeapons()
    {
        foreach (GameObject weapon in _weapons)
        {
            weapon.SetActive(false);
        }
    }
}

public static class PlayerAnimation
{
    public static readonly int Dodge = Animator.StringToHash("Dodge");
}