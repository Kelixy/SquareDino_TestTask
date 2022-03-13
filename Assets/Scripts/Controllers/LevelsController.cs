using System;
using SceneObjects;
using UnityEngine;

namespace Controllers
{
    public class LevelsController : MonoBehaviour
    {
        [Serializable]
        private struct Level
        {
            [SerializeField] private Enemy[] enemiesOnLevel;
            public Enemy[] EnemiesOnLevel => enemiesOnLevel;
        }
    
        [SerializeField] private Level[] levels;
        private int _currentLevel;
        private int _killedEnemiesCount;

        public void PlusOneEnemyKilled()
        {
            _killedEnemiesCount++;
        }

        public bool CheckIfLevelAimReached()
        {
            return _killedEnemiesCount >= levels[_currentLevel].EnemiesOnLevel.Length;
        }

        public bool CheckIfLastLevel() => _currentLevel >= levels.Length;

        public void GoToNextLevel()
        {
            _killedEnemiesCount = 0;
            _currentLevel++;
            ControllersManager.Instance.HeroController.MoveToNextPoint(_currentLevel);
        }
    }
}
