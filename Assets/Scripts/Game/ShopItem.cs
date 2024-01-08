using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(Button))]
public class ShopItem : Upgrade
{

    [Header("Button")]
    [SerializeField] private Button _button;

    [field: Header("Upgrade name")]
    [field: SerializeField] public PlayerUpgrades UpgradeType { get; private set; }

    [field: Header("Its rewarded")]
    [field: SerializeField] public bool ItsRewarded { get; private set; }

    [Space, Header("Text info")]
    [SerializeField] private TextMeshProUGUI _levelText;

    [Header("Not Rewarded")]
    [SerializeField] private TextMeshProUGUI _toiletsCostText;
    [SerializeField] private TextMeshProUGUI _headsCostText;

    private TextMeshProUGUI[] _childs;
    [SerializeField] private Wallet _wallet;

    private void OnValidate()
    {
        _button ??= GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (!ItsRewarded)
        {
            Wallet.CurrenciesChanged += SwitchButtonActivity;
        }
    }

    private void OnDisable()
    {
        if (!ItsRewarded)
        {
            Wallet.CurrenciesChanged -= SwitchButtonActivity;
        }
    }

    private void Start()
    {
        LoadData();
        _childs = transform.GetComponentsInChildren<TextMeshProUGUI>();

        if (ItsRewarded)
        {
            if (!_levelText)
            {
                throw new NullReferenceException();
            }
        }
        else
        {
            if (!_levelText || !_toiletsCostText || !_headsCostText)
            {
                throw new NullReferenceException();
            }
        }

        if (!ItsRewarded)
        {
            if (!_wallet)
            {
                throw new NullReferenceException(nameof(_wallet));
            }
        }

        ShowStats();
    }

    private void LoadData()
    {
        for (int i = 0; i < YandexGame.savesData.UpgradeType.Count; i++)
        {
            if (UpgradeType == YandexGame.savesData.UpgradeType[i])
            {
                CurrentParameter = YandexGame.savesData.CurrentParameter[i];

                ToiletsCost = YandexGame.savesData.ToiletsCost[i];
                HeadsCost = YandexGame.savesData.HeadsCost[i];

                CurrentUpgradeLevel = YandexGame.savesData.CurrentUpgradeLevel[i];
            }
        }
    }

    public void ShowStats()
    {
        if (ItsRewarded)
        {
            if (CurrentUpgradeLevel < MaxUpgradeLevel)
            {
                _levelText.text = $"{CurrentUpgradeLevel}/{MaxUpgradeLevel}";
            }
            else
            {
                _levelText.text = $"MAX";
                _button.interactable = false;
                SwitchChildTextAlpha(false);
            }
        }
        else
        {
            if (CurrentUpgradeLevel < MaxUpgradeLevel)
            {
                _toiletsCostText.text = $"{ToiletsCost}";
                _headsCostText.text = $"{HeadsCost}";
                _levelText.text = $"{CurrentUpgradeLevel}/{MaxUpgradeLevel}";
            }
            else
            {
                _toiletsCostText.text = $"MAX";
                _headsCostText.text = $"MAX";
                _levelText.text = $"MAX";
                _button.interactable = false;
                SwitchChildTextAlpha(false);
            }

            SwitchButtonActivity();
        }
    }

    private void SwitchButtonActivity()
    {
        if (ItsRewarded)
        {
            return;
        }

        if (CurrentUpgradeLevel >= MaxUpgradeLevel || _wallet.Toilets < ToiletsCost || _wallet.Heads < HeadsCost)
        {
            _button.interactable = false;
            SwitchChildTextAlpha(false);
        }
        else
        {
            _button.interactable = true;
            SwitchChildTextAlpha(true);
        }
    }

    private void SwitchChildTextAlpha(bool isFullVisible)
    {
        if (isFullVisible)
        {
            for (int i = 0; i < _childs.Length; i++)
            {
                _childs[i].color = new Color(_childs[i].color.r, _childs[i].color.g, _childs[i].color.b, 1f);
            }
        }
        else
        {
            for (int i = 0; i < _childs.Length; i++)
            {
                _childs[i].color = new Color(_childs[i].color.r, _childs[i].color.g, _childs[i].color.b, 0.5f);
            }
        }
    }
}
