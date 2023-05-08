using Scene_Management;
using TMPro;
using UnityEngine;

namespace Misc
{
    public class EconomyManager : Singleton<EconomyManager>
    {
        private TMP_Text _goldText;
        private int _currentGold = 0;

        const string CoinAmountText = "Gold Amount Text";

        public void UpdateCurrentGold()
        {
            _currentGold++;
            
            if (_goldText == null)
            {
                _goldText = GameObject.Find(CoinAmountText).GetComponent<TMP_Text>();
            }

            _goldText.text = _currentGold.ToString("D3");
        }
    }
}
