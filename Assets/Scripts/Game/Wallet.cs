using System;
using UnityEngine;
using YG;

public class Wallet : MonoBehaviour
{

    [field: SerializeField] public int Toilets { get; private set; }
    [field: SerializeField] public int Heads { get; private set; }

    public static event Action<Currencies, int> ShowCurrency;

    public static event Action CurrenciesChanged;

    private void Start()
    {
        LoadData();
        ShowCurrencies();
    }

    private void LoadData()
    {
        Toilets = YandexGame.savesData.Toilets;
        Heads = YandexGame.savesData.Heads;
    }

    public void AddCurrency(int addToilets, int addHeads)
    {
        if (addToilets < 1 && addHeads < 1)
        {
            throw new ArgumentOutOfRangeException($"Can't add to wallet less then 1 of any currency");
        }
        else
        {
            if (addToilets < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(addToilets));
            }
            else
            {
                if (Toilets + addToilets < int.MaxValue && Toilets + addToilets > 0)
                {
                    Toilets += addToilets;
                }
                else
                {
                    Toilets = int.MaxValue;
                }

            }

            if (addHeads < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(addHeads));
            }
            else
            {
                if (Heads + addHeads < int.MaxValue && Heads + addHeads > 0)
                {
                    Heads += addHeads;
                }
                else
                {
                    Heads = int.MaxValue;
                }

            }

            CurrenciesChanged?.Invoke();
            SaveData();
        }
    }

    public void Spend(int costToilets, int costHeads)
    {
        if (costToilets > Toilets || costHeads > Heads)
        {
            throw new ArgumentOutOfRangeException();
        }
        Toilets -= costToilets;
        Heads -= costHeads;

        CurrenciesChanged?.Invoke();
        SaveData();
    }

    private void SaveData()
    {
        YandexGame.savesData.Toilets = Toilets;
        YandexGame.savesData.Heads = Heads;

        ShowCurrencies();

        YandexGame.SaveProgress();
    }

    public bool IsEnough(int costToilets, int costHeads)
    {
        if (costToilets < 0 || costHeads < 0)
        {
            throw new ArgumentOutOfRangeException();
        }
        return costToilets <= Toilets && costHeads <= Heads;
    }

    private void ShowCurrencies()
    {
        ShowCurrency?.Invoke(Currencies.Toilets, Toilets);
        ShowCurrency?.Invoke(Currencies.Heads, Heads);
    }
}
