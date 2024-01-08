using UnityEngine;
using System;
using System.Collections.Generic;
using YG;

public class Shop : MonoBehaviour
{

    [field: SerializeField] public List<ShopItem> Items { get; private set; }

    private Wallet _playerWallet;

    public static event Action<PlayerUpgrades, int> Upgraded;

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += VideoBuy;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= VideoBuy;
    }

    public void Initialize()
    {
        _playerWallet = FindFirstObjectByType<Player>().Wallet;
        if (!_playerWallet)
        {
            throw new ArgumentNullException(nameof(_playerWallet));
        }
        SetShopItems();
    }

    private void SetShopItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            ShopItem child = transform.GetChild(i).GetComponent<ShopItem>();
            if (child)
            {
                if (transform.childCount > 0)
                {
                    bool repeated = false;
                    for (int j = 0; j < Items.Count; j++)
                    {
                        if (Items[j].UpgradeType == child.UpgradeType)
                        {
                            repeated = true;
                            break;
                        }
                    }

                    if (!repeated)
                    {
                        Items.Add(child);
                    }
                }
                else
                {
                    Items.Add(child);
                }
            }
        }
    }

    public void Buy(ShopItem item)
    {
        if (item.ItsRewarded)
        {
            if (item.CanUpgrade())
            {
                item.LevelUp();
                Upgraded(item.UpgradeType, item.CurrentParameter);
                item.ShowStats();
            }
        }
        else
        {
            if (item.CanUpgrade() && _playerWallet.IsEnough(item.ToiletsCost, item.HeadsCost))
            {
                if (Upgraded != null)
                {
                    _playerWallet.Spend(item.ToiletsCost, item.HeadsCost);
                    item.LevelUp();
                    Upgraded(item.UpgradeType, item.CurrentParameter);
                    item.ShowStats();
                }
            }
        }

        SaveShopItemData(item);
    }

    #region SAVE
    private void SaveShopItemData(ShopItem item)
    {
        if (YandexGame.savesData.UpgradeType.Count > 0)
        {
            bool repeated = false;
            for (int i = 0; i < YandexGame.savesData.UpgradeType.Count; i++)
            {
                if (YandexGame.savesData.UpgradeType[i] == item.UpgradeType)
                {
                    RemoveShopItemFieldsFromSave(i);
                    AddShopItemFieldsToSave(item);
                    repeated = true;
                    break;
                }
            }

            if (!repeated)
            {
                AddShopItemFieldsToSave(item);
            }
        }
        else
        {
            AddShopItemFieldsToSave(item);
        }

        YandexGame.SaveProgress();
    }

    private void AddShopItemFieldsToSave(ShopItem item)
    {
        YandexGame.savesData.UpgradeType.Add(item.UpgradeType);

        YandexGame.savesData.CurrentParameter.Add(item.CurrentParameter);
        YandexGame.savesData.ToiletsCost.Add(item.ToiletsCost);
        YandexGame.savesData.HeadsCost.Add(item.HeadsCost);
        YandexGame.savesData.CurrentUpgradeLevel.Add(item.CurrentUpgradeLevel);
    }

    private void RemoveShopItemFieldsFromSave(int i)
    {
        YandexGame.savesData.UpgradeType.Remove(YandexGame.savesData.UpgradeType[i]);

        YandexGame.savesData.CurrentParameter.Remove(YandexGame.savesData.CurrentParameter[i]);
        YandexGame.savesData.ToiletsCost.Remove(YandexGame.savesData.ToiletsCost[i]);
        YandexGame.savesData.HeadsCost.Remove(YandexGame.savesData.HeadsCost[i]);
        YandexGame.savesData.CurrentUpgradeLevel.Remove(YandexGame.savesData.CurrentUpgradeLevel[i]);
    }
    #endregion

    private void VideoBuy(int id)
    {
        if (id < Items.Count && id >= 0)
        {
            Buy(Items[id]);
        }
        else
        {
            throw new ArgumentException(nameof(id));
        }
    }
}
