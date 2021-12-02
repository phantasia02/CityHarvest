using UnityEngine;

namespace MYgame.Scripts.Scenes.GameScenes.Data
{
    [CreateAssetMenu(
        menuName = "Data/Stage Data",
        fileName = "StageData")]
    public class StageData : ScriptableObject
    {
        [SerializeField]
        private StaticGlobalDel.EBrickColor[] _brickColors;
        [SerializeField]
        private BuildingRecipeData[] _buildings;

        public StaticGlobalDel.EBrickColor[] brickColors => _brickColors;
        public BuildingRecipeData[] buildings => _buildings;
    }
}
