using Arman.UIManagement;
using GeoGuessr.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GeoGuessr.Presentation
{
    public class MainMenuWindow : MainWindow
    {
        [SerializeField] TMP_Dropdown _levelsDropDown;
        [SerializeField] TMP_Dropdown _playModeDropDown;

        private GameTransitionManager _transitionManager;
        private BoardsDatabase _boardsDatabase;


        public void Setup(BoardsDatabase boardsDatabase ,GameTransitionManager transitionManager)
        {
            _transitionManager = transitionManager; 
            _boardsDatabase = boardsDatabase;

            _levelsDropDown.ClearOptions();
            _levelsDropDown.AddOptions(boardsDatabase.BoardDefinitions.Select(def => def.Name).ToList());
            _levelsDropDown.value = 0;

            _playModeDropDown.ClearOptions();
            _playModeDropDown.AddOptions(new List<string> { "Single Player", "Two Players", "Player vs AI" } );
            _playModeDropDown.value = 0;
        }


        public void StartLevel()
        {
            
            var levelMode = new LevelMode();

            levelMode.SetBoard(_boardsDatabase.BoardDefinitions[_levelsDropDown.value]);

            switch(_playModeDropDown.value)
            {
                case 0:
                    levelMode.AddLocalPlayer("Player");
                    break;
                case 1:
                    levelMode.AddLocalPlayer("Player1").AddLocalPlayer("Player2");
                    break;
                case 2:
                    levelMode.AddLocalPlayer("Plauer1").AddAiPlayer();
                    break;
            }

            _transitionManager.GoToLevel(levelMode);
        }
    }
}