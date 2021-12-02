using MYgame.Scripts.Scenes.GameScenes.Data;
using UnityEngine;
using UnityEngine.UI;

namespace MYgame.Scripts.Window
{
    /// <summary>
    /// The ui for the current building recipe
    /// </summary>
    public class BuildingRecipeUI : MonoBehaviour
    {
        [SerializeField]
        private Image _curBuildingImage;
        [SerializeField]
        private Image _nextBuildingImage;
        [SerializeField]
        private BuildingStatusUI _buildingStatusUI;

        /// <summary>
        /// Set the recipe
        /// </summary>
        public void SetBuildingRecipe(
            Sprite curSprite, BrickAmount[] targetBrickAmounts, Sprite nextSprite)
        {
            _curBuildingImage.sprite = curSprite;
            _nextBuildingImage.sprite = nextSprite;
            _buildingStatusUI.SetMaxBrickAmount(targetBrickAmounts);
        }
    }
}
