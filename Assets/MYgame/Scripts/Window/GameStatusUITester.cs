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

        private void Start()
        {
            SetUI();
        }

        private void SetUI()
        {
            _statusUI.SetBuildingRecipe(_curImageSprite, _brickAmounts, _nextImageSprite);
        }
    }
}
