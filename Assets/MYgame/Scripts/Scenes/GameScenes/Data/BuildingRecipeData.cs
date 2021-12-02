using System;
using UnityEngine;

namespace MYgame.Scripts.Scenes.GameScenes.Data
{
    [CreateAssetMenu(
        fileName = "BuildingRecipe",
        menuName = "Data/Building Recipe")]
    public class BuildingRecipeData : ScriptableObject
    {
        [SerializeField]
        private Sprite _buildingSprite;
        [SerializeField]
        private BrickAmount[] _brickAmounts;

        public Sprite buildingSprite => _buildingSprite;
        public BrickAmount[] brickAmounts => _brickAmounts;
    }

    [Serializable]
    public class BrickAmount
    {
        [SerializeField]
        private StaticGlobalDel.EBrickColor _color;
        [SerializeField]
        private int _amount;

        public StaticGlobalDel.EBrickColor color => _color;
        public int amount => _amount;
    }
}
