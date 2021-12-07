using UnityEngine;
using LanKuDot.UnityToolBox;

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
        [SerializeField]
        private TweenHDRColorEaseCurve _creatarchitecture;

        public StaticGlobalDel.EBrickColor[] brickColors => _brickColors;
        public BuildingRecipeData[] buildings => _buildings;
        public TweenHDRColorEaseCurve creatarchitecture => _creatarchitecture;
    }
}
