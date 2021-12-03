using MYgame.Scripts.Scenes.GameScenes.Data;
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
        [SerializeField]
        private BuildingRecipeUI _buildingRecipeUI;

        /// <summary>
        /// Set the total number of the bricks
        /// </summary>
        public void UpdateTotalBricksNumber(StaticGlobalDel.EBrickColor color, int number)
        {
            _brickStatusGroupUI.SetNumber(color, number);
            _buildingRecipeUI.SetNumber(color, number);
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
    }
}
