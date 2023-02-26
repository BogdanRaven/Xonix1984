using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Create/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private int _fieldHeight;
        [SerializeField] private int _fieldWidth;
        [SerializeField] private int _waterSpace;
        [SerializeField] private int _numberPlayerLives;
        [SerializeField] private int _numberWaterEnemies;
        [SerializeField] private int _numberLandEnemies;
        [SerializeField] private int _percentOfLandToWin;
        [SerializeField] private int _gameDuration;
        [SerializeField] private int _startLevel;

        public int FieldHeight => _fieldHeight;
        public int FieldWidth => _fieldWidth;
        public int WaterSpace => _waterSpace;
        public int NumberPlayerLives => _numberPlayerLives;
        public int NumberLandEnemies => _numberLandEnemies;
        public int NumberWaterEnemies => _numberWaterEnemies;
        public int PercentOfLandToWin => _percentOfLandToWin;

        public int GameDuration => _gameDuration;

        public int StartLevel => _startLevel;

        public GameConfig(int fieldHeight, int fieldWidth, int waterSpace, int percentOfLandToWin, int numberPlayerLives, int numberLandEnemies,
            int numberWaterEnemies, int startLevel, int gameDuration)
        {
            _fieldHeight = fieldHeight;
            _fieldWidth = fieldWidth;
            _waterSpace = waterSpace;
            _numberPlayerLives = numberPlayerLives;
            _numberLandEnemies = numberLandEnemies;
            _numberWaterEnemies = numberWaterEnemies;
            _percentOfLandToWin = percentOfLandToWin;
            _startLevel = startLevel;
            _gameDuration = gameDuration;
        }
    }
}