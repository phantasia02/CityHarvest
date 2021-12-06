using System.Collections.Generic;
using UnityEngine;

namespace MYgame.Scripts.Window
{
    /// <summary>
    /// The ui for the brick status
    /// </summary>
    public class BrickStatusGroupUI : MonoBehaviour
    {
        [SerializeField]
        private BrickStatusUI[] _brickUIs;

        private readonly
            Dictionary<StaticGlobalDel.EBrickColor, BrickStatusUI> _brickColorUIMap =
                new Dictionary<StaticGlobalDel.EBrickColor, BrickStatusUI>();

        private void Awake()
        {
            foreach (var ui in _brickUIs) {
                _brickColorUIMap[ui.color] = ui;
                ui.SetNumber(0);
            }
        }

        /// <summary>
        /// Set the number of the bricks
        /// </summary>
        /// <param name="color">The color of the brick</param>
        public void IncreaseNumber(StaticGlobalDel.EBrickColor color)
        {
            _brickColorUIMap[color].IncreaseNumber();
        }
    }
}
