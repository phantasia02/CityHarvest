using System;
using MYgame.Scripts.Scenes.GameScenes.Data;
using UnityEngine;
using UnityEngine.Events;

namespace MYgame.Scripts.Window
{
    /// <summary>
    /// The ui for the game status
    /// </summary>
    public class GameStatusUI : MonoBehaviour
    {
        [SerializeField]
        private BrickStatusGroupUI _brickStatusGroupUI;
        [SerializeField]
        private BuildingRecipeUI _buildingRecipeUI;
        [SerializeField]
        private Timer _timer;

        /// <summary>
        /// Set the total number of the bricks
        /// </summary>
        public void UpdateTotalBricksNumber(StaticGlobalDel.EBrickColor color, int number)
        {
            _buildingRecipeUI.SetNumber(color, number);
        }

        /// <summary>
        /// Increase the number of the specified brick
        /// </summary>
        /// <param name="color">The color of the brick</param>
        public void IncreaseBrickNumber(StaticGlobalDel.EBrickColor color)
        {
            _brickStatusGroupUI.IncreaseNumber(color);
        }

        /// <summary>
        /// Set the building recipe
        /// </summary>
        /// <param name="curSprite">The sprite of the current building target</param>
        /// <param name="targetBrickAmounts">The target amount of bricks</param>
        /// <param name="nextSprite">The sprite of the next building target</param>
        public void SetBuildingRecipe(
            Sprite curSprite, BrickAmount[] targetBrickAmounts, Sprite nextSprite)
        {
            _buildingRecipeUI.SetBuildingRecipe(
                curSprite, targetBrickAmounts, nextSprite);
        }

        /// <summary>
        /// Start the timer
        /// </summary>
        /// <param name="onTimesUp">The callback when the times is up</param>
        public void StartTimer(Action onTimesUp)
        {
            _timer.StartTimer(onTimesUp);
        }
    }
}
