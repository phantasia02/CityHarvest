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

        public StaticGlobalDel.EBrickColor color => _color;

        #region Activation

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Inactivate()
        {
            gameObject.SetActive(false);
        }

        #endregion

        public void SetNumber(int number)
        {
            _text.text = number.ToString();
        }
    }
}
