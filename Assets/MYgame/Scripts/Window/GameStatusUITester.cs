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
        private Sprite _curImageSprite;
        [SerializeField]
        private Sprite _nextImageSprite;
        [SerializeField]
        private BrickAmount[] _brickAmounts;

        private int _number;

        private void Start()
        {
            SetUI();
            DOTween.To(
                    () => 0, x => { }, 0, 0.5f)
                .OnStepComplete(UpdateNumber)
                .SetLoops(-1);
        }

        private void SetUI()
        {
            _statusUI.SetBuildingRecipe(_curImageSprite, _brickAmounts, _nextImageSprite);
        }

        private void UpdateNumber()
        {
            _number += 1;
            _statusUI.UpdateTotalBricksNumber(StaticGlobalDel.EBrickColor.eRed, _number);
            _statusUI.UpdateTotalBricksNumber(StaticGlobalDel.EBrickColor.eYellow, _number * 2);
            _statusUI.UpdateTotalBricksNumber(StaticGlobalDel.EBrickColor.eGreen, _number * 3);
            _statusUI.UpdateTotalBricksNumber(StaticGlobalDel.EBrickColor.eWhite, _number * 4);
        }
    }
}
