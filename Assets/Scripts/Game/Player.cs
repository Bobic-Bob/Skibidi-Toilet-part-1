using UnityEngine;
using System;
using System.Collections;
using YG;

public class Player : MonoBehaviour
{

    private Coroutine _coroutine;

    [field: SerializeField] private Enemy _currentEnemy;
    [field: SerializeField] public Wallet Wallet { get; private set; }

    [field: Space, Header("Click")]

    [field: SerializeField, Min(1)] public int ClickPower { get; private set; }
    [field: SerializeField, Min(1)] public int ClickMultiplier { get; private set; }

    [field: Space, Header("Auto Click")]

    [field: SerializeField, Min(1)] public int AutoClickPower { get; private set; }
    [field: SerializeField, Min(0)] public int AutoClickSpeed { get; private set; }
    [field: SerializeField, Min(1)] public int AutoClickMultiplier { get; private set; }

    [field: Space, Header("Self Multiplier")]

    [field: SerializeField, Min(0)] public int TotalClickMultiplier { get; private set; }
    [field: SerializeField, Min(0)] public int TotalAutoClickMultiplier { get; private set; }

    public static event Action<Player> SaveStats;

    public Player()
    {
        ClickPower = 1;
        ClickMultiplier = 1;

        AutoClickPower = 1;
        AutoClickSpeed = 0;
        AutoClickMultiplier = 1;

        TotalClickMultiplier = 0;
        TotalAutoClickMultiplier = 0;
    }

    private void OnEnable()
    {
        Enemy.Clicked += ClickDamage;
        Enemy.Drop += Wallet.AddCurrency;
        Enemy.Born += SetEnemy;
        Shop.Upgraded += PowerUp;
    }

    private void OnDisable()
    {
        Enemy.Clicked -= ClickDamage;
        Enemy.Drop -= Wallet.AddCurrency;
        Enemy.Born -= SetEnemy;
        Shop.Upgraded -= PowerUp;
    }

    public void Initialize()
    {
        LoadData();

        Wallet = gameObject.transform.GetComponentInChildren<Wallet>();
        if (!Wallet)
        {
            throw new NullReferenceException(nameof(Wallet));
        }
    }

    private void LoadData()
    {
        ClickPower = YandexGame.savesData.ClickPower;
        ClickMultiplier = YandexGame.savesData.ClickMultiplier;

        AutoClickPower = YandexGame.savesData.AutoClickPower;
        AutoClickSpeed = YandexGame.savesData.AutoClickSpeed;
        AutoClickMultiplier = YandexGame.savesData.AutoClickMultiplier;

        TotalClickMultiplier = YandexGame.savesData.TotalClickMultiplier;
        TotalAutoClickMultiplier = YandexGame.savesData.TotalAutoClickMultiplier;
    }

    public int ClickDamage()
    {
        return ClickPower * (ClickMultiplier + TotalClickMultiplier);
    }

    public int AutoClickDamage()
    {
        return AutoClickPower * (AutoClickMultiplier + TotalAutoClickMultiplier);
    }

    // метод автоклика
    private IEnumerator AutoDamage()
    {
        while (_currentEnemy)
        {
            yield return new WaitForSeconds(AutoClickSpeed);
            _currentEnemy.TakeDamage(AutoClickDamage());
        }

        StopAutoClick();
    }

    private void StopAutoClick()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private void SetEnemy()
    {
        _currentEnemy = FindFirstObjectByType<Enemy>();

        if (_currentEnemy == null)
        {
            throw new NullReferenceException(nameof(_currentEnemy));
        }

        if (AutoClickSpeed > 0 && _coroutine == null)
        {
            _coroutine = StartCoroutine(AutoDamage());
        }
    }

    private void PowerUp(PlayerUpgrades powerName, int newPowerParam)
    {
        if (newPowerParam >= 0)
        {
            switch (powerName)
            {
                case PlayerUpgrades.ClickPower:
                    ClickPower = newPowerParam;
                    YandexGame.savesData.ClickPower = ClickPower;
                    break;
                case PlayerUpgrades.ClickMultiplier:
                    ClickMultiplier = newPowerParam;
                    YandexGame.savesData.ClickMultiplier = ClickMultiplier;
                    break;

                case PlayerUpgrades.AutoClickPower:
                    AutoClickPower = newPowerParam;
                    YandexGame.savesData.AutoClickPower = AutoClickPower;
                    break;
                case PlayerUpgrades.AutoClickSpeed:
                    AutoClickSpeed = newPowerParam;
                    YandexGame.savesData.AutoClickSpeed = AutoClickSpeed;
                    StopAutoClick();
                    if (AutoClickSpeed > 0)
                    {
                        _coroutine = StartCoroutine(AutoDamage());
                    }
                    break;
                case PlayerUpgrades.AutoClickMultiplier:
                    AutoClickMultiplier = newPowerParam;
                    YandexGame.savesData.AutoClickMultiplier = AutoClickMultiplier;
                    break;

                case PlayerUpgrades.TotalClickMultiplier:
                    TotalClickMultiplier = newPowerParam;
                    YandexGame.savesData.TotalClickMultiplier = TotalClickMultiplier;
                    break;
                case PlayerUpgrades.TotalAutoClickMultiplier:
                    TotalAutoClickMultiplier = newPowerParam;
                    YandexGame.savesData.TotalAutoClickMultiplier = TotalAutoClickMultiplier;
                    break;

                default: throw new ArgumentException(nameof(powerName));
            }
            SaveStats?.Invoke(this);

            YandexGame.SaveProgress();
        }
        else
        {
            throw new ArgumentException(nameof(newPowerParam));
        }
    }
}
