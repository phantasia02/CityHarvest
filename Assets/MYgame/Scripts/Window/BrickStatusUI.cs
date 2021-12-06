using TMPro;
using UnityEngine;

namespace MYgame.Scripts.Window
{
    /// <summary>
    /// The status of the single brick
    /// </summary>
    public class BrickStatusUI : MonoBehaviour
    {
        [SerializeField]
        private StaticGlobalDel.EBrickColor _color;
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private bool _showMaxNum;

        private int _maxNumber;
        private int _currentNumber = 0;

        public StaticGlobalDel.EBrickColor color => _color;

        #region Activation

        public void Activate()
        {
            if (!_showMaxNum)
                _currentNumber = 0;
            gameObject.SetActive(true);
        }

        public void Inactivate()
        {
            gameObject.SetActive(false);
        }

        #endregion

        public void SetMaxNumber(int number)
        {
            _maxNumber = number;
        }

        public void SetNumber(int number)
        {
            _text.text =
                _showMaxNum ?
                    $"{Mathf.Min(number, _maxNumber)}/{_maxNumber}" :
                    $"{number}";
        }

        public void IncreaseNumber()
        {
            ++_currentNumber;
            _text.text = _currentNumber.ToString();
        }
    }
}
