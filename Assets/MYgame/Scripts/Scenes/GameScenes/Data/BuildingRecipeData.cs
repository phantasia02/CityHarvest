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
        [SerializeField]
        private GameObject _Prefab3DMode;

        public Sprite buildingSprite => _buildingSprite;
        public BrickAmount[] brickAmounts => _brickAmounts;
        public GameObject Prefab3DMode => _Prefab3DMode;
    }

    [Serializable]
    public class BrickAmount
    {
        [SerializeField]
        private StaticGlobalDel.EBrickColor _color;
        [SerializeField]
        private int _amount;

        public StaticGlobalDel.EBrickColor color
        {
            set => _color = value;
            get => _color;
        }

        public int amount
        {
            set => _amount = value;
            get => _amount;
        }
}
}
