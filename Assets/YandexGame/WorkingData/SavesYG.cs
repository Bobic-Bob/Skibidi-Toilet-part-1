using System.Collections.Generic;
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Ваши сохранения

        #region AUDIO
        public bool Music = true;
        public bool Sound = true;
        #endregion

        #region PLAYER_STATS
        public int ClickPower = 1;
        public int ClickMultiplier = 1;

        public int AutoClickPower = 1;
        public int AutoClickSpeed = 0;
        public int AutoClickMultiplier = 1;

        public int TotalClickMultiplier = 0;
        public int TotalAutoClickMultiplier = 0;
        #endregion

        #region WALLET_STATS
        //public WalletStats WalletStats = new WalletStats();
        public int Toilets = 0;
        public int Heads = 0;
        #endregion

        #region SHOP_ITEMS_STATS
        //public List<ShopItemsStats> ShopItemsStats = new List<ShopItemsStats>();

        public List<PlayerUpgrades> UpgradeType = new List<PlayerUpgrades>();
        public List<int> CurrentParameter = new List<int>();
        public List<int> ToiletsCost = new List<int>();
        public List<int> HeadsCost = new List<int>();
        public List<int> CurrentUpgradeLevel = new List<int>();
        #endregion

        #region KILLED_ENEMIES
        public List<bool> KilledEnemies = new List<bool>();
        #endregion


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {

        }
    }
}
