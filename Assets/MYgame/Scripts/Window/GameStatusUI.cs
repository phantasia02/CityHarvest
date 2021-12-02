using System;
using UnityEngine;

namespace MYgame.Scripts.Window
{
    /// <summary>
    /// The ui for the game status
    /// </summary>
    public class GameStatusUI : MonoBehaviour
    {
        [SerializeField]
        private BrickStatusGroupUI _brickStatusGroupUI;

        /// <summary>
        /// Set the total number of the bricks
        /// </summary>
        public void UpdateTotalBricksNumber(StaticGlobalDel.EBrickColor color, int number)
        {
            _brickStatusGroupUI.SetNumber(color, number);
        }
    }
}
