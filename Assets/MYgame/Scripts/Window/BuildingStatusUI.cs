using System.Collections.Generic;
using MYgame.Scripts.Scenes.GameScenes.Data;
using UnityEngine;

namespace MYgame.Scripts.Window
{
    /// <summary>
    /// The status of the building
    /// </summary>
    public class BuildingStatusUI : MonoBehaviour
    {
        [SerializeField]
        private BrickStatusUI[] _brickStatusUIs;

        private readonly Dictionary<StaticGlobalDel.EBrickColor, BrickStatusUI>
            _brickStatusUIMap =
                new Dictionary<StaticGlobalDel.EBrickColor, BrickStatusUI>();

        private void Awake()
        {
            foreach (var ui in _brickStatusUIs) {
                ui.Inactivate();
                _brickStatusUIMap[ui.color] = ui;
            }
        }

        public void SetMaxBrickAmount(BrickAmount[] maxBrickAmounts)
        {
            foreach (var ui in _brickStatusUIs)
                ui.Inactivate();

            foreach (var brickAmount in maxBrickAmounts) {
                var color = brickAmount.color;
                var ui = _brickStatusUIMap[color];
                ui.SetMaxNumber(brickAmount.amount);
                ui.SetNumber(0);
                ui.Activate();
            }
        }

        public void SetNumber(StaticGlobalDel.EBrickColor color, int number)
        {
            var ui = _brickStatusUIMap[color];
            if (ui.gameObject.activeSelf)
                ui.SetNumber(number);
        }
    }
}
