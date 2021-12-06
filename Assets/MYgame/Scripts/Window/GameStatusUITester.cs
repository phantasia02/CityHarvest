using System;
using DG.Tweening;
using MYgame.Scripts.Scenes.GameScenes.Data;
using UnityEngine;

namespace MYgame.Scripts.Window
{
    /// <summary>
    /// The tester for testing the GameStatusUI
    /// </summary>
    public class GameStatusUITester : MonoBehaviour
    {
        [SerializeField]
        private GameStatusUI _statusUI;
        [SerializeField]
        private BuildingRecipeData[] _recipeDatas;

        private int _number;
        private int _completedRecipeCount;

        private void Start()
        {
            SetBuildingRecipe();
            DOTween.To(
                    () => 0, x => { }, 0, 0.5f)
                .OnStepComplete(UpdateNumber)
                .SetLoops(-1);
            DOTween.To(
                    () => 0, x => { }, 0, 5f)
                .OnStepComplete(SetBuildingRecipe)
                .SetLoops(-1);
            _statusUI.StartTimer(() => Debug.Log("Times up"));
        }

        private void UpdateNumber()
        {
            _number += 1;
            _statusUI.IncreaseBrickNumber(StaticGlobalDel.EBrickColor.eRed);
            _statusUI.IncreaseBrickNumber(StaticGlobalDel.EBrickColor.eYellow);
            _statusUI.IncreaseBrickNumber(StaticGlobalDel.EBrickColor.eGreen);
            _statusUI.IncreaseBrickNumber(StaticGlobalDel.EBrickColor.eWhite);
        }

        private void SetBuildingRecipe()
        {
            var id = _completedRecipeCount % _recipeDatas.Length;
            var data = _recipeDatas[id];
            var nextData = _recipeDatas[(id + 1) % _recipeDatas.Length];
            _statusUI.SetBuildingRecipe(
                data.buildingSprite, data.brickAmounts, nextData.buildingSprite);

            ++_completedRecipeCount;
        }
    }
}
