using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Collider2D), typeof(Animator), typeof(AudioSource))]
public class Enemy : MonoBehaviour
{

    [field: Space, Header("Health")]
    [field: SerializeField, Min(1)] public int MaxHealth { get; private set; }
    public int Health { get; private set; }

    [SerializeField] private Slider _hpSlider;
    private TextMeshProUGUI _hpText;

    [field: Space, Header("Drop")]
    [field: SerializeField, Min(0)] public int DropToilets { get; private set; }
    [field: SerializeField, Min(0)] public int DropHeads { get; private set; }

    [Space, Header("Particles")]
    [SerializeField] private ParticleSystem _particleSystem;

    private EnemyAnimator _animator;

    private AudioSource _audioSource;

    public static event Func<int> Clicked;
    public static event Action<int, int> Drop;
    public static event Action Born;
    public static event Action Die;

    public Enemy()
    {
        MaxHealth = 1;
        Health = MaxHealth;
        DropToilets = 0;
        DropHeads = 0;
    }

    public Enemy(int maxHealth, int dropToilets, int dropHeads = 1)
    {
        if (maxHealth > 0)
        {
            MaxHealth = maxHealth;
        }
        else
        {
            MaxHealth = 1;
            throw new ArgumentException($"MaxHealth can't be less then 1");
        }

        Health = MaxHealth;

        if (dropToilets >= 0)
        {
            DropToilets = dropToilets;
        }
        else
        {
            DropToilets = 0;
            throw new ArgumentException($"DropToilets can't be less then 0");
        }

        if (dropHeads >= 0)
        {
            DropHeads = dropHeads;
        }
        else
        {
            DropHeads = 0;
            throw new ArgumentException($"DropHeads can't be less then 0");
        }
    }

    private void OnValidate()
    {
        _particleSystem ??= GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        Health = MaxHealth;
        _animator = GetComponent<EnemyAnimator>();
        _audioSource = GetComponent<AudioSource>();
        _hpSlider = FindFirstObjectByType<Slider>();

        if (_hpSlider)
        {
            _hpSlider.minValue = 0;
            _hpSlider.maxValue = MaxHealth;
            _hpSlider.value = Health;

            _hpText = _hpSlider.GetComponentInChildren<TextMeshProUGUI>();

            if (_hpText)
            {
                _hpText.text = $"{Health}/{MaxHealth}";
            }
            else
            {
                throw new NullReferenceException(nameof(_hpText));
            }
        }
        else
        {
            throw new NullReferenceException(nameof(_hpSlider));
        }

        Born();
    }

    private void OnMouseDown()
    {
        if (_animator != null)
        {
            _animator.PlayClick();
        }

        if (_particleSystem != null)
        {
            _particleSystem.Play();
        }

        if (Clicked != null)
        {
            TakeDamage(Clicked());
        }
    }

    private void DeadDrop()
    {
        Health = 0;

        if (Drop != null)
        {
            Drop(DropToilets, DropHeads);
        }

        if (Die != null)
        {
            Die();
        }

        if (_particleSystem != null)
        {
            _particleSystem.Stop();
        }

        Dead();
    }

    public void TakeDamage(int damage)
    {
        if (damage >= 0)
        {
            if (Health <= damage)
            {
                DeadDrop();
            }
            else
            {
                if (_audioSource != null)
                {
                    _audioSource.pitch = UnityEngine.Random.Range(0.5f, 1.5f);
                    _audioSource.Play();
                }

                Health -= damage;
            }

            _hpSlider.value = Health;
            _hpText.text = $"{MaxHealth}/{Health}";
        }
        else
        {
            throw new ArgumentException($"Damage can't be less then 0");
        }
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
