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
        /// Set the number of the bricks
        /// </summary>
        public void SetNumber(StaticGlobalDel.EBrickColor color, int number)
        {
            _brickStatusGroupUI.SetNumber(color, number);
        }
    }
}
