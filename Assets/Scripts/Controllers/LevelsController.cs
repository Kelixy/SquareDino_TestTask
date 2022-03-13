using System;
using System.Collections.Generic;
using Pools;
using SceneObjects;
using UnityEngine;

namespace Controllers
{
    public class LevelsController : MonoBehaviour
    {
        [Serializable]
        private struct Level
        {
            [SerializeField] private Transform[] enemyPointsOnLevel;
            [Range(0,6)][SerializeField] private int countOfEnemiesOnLvl;
            public Transform[] EnemyPointsOnLevel => enemyPointsOnLevel;
            public int CountOfEnemiesOnLvl => countOfEnemiesOnLvl;
        }
    
        [SerializeField] private Transform poolTransform;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Level[] levels;

        private List<IEnemy> EnemiesOnScene { get; set; }
        private PoolOfObjects<IEnemy> _poolOfEnemies;
        private int _currentLevel;
        private int _killedEnemiesCount;
        private GameController _gameController;

        public void Initialize()
        {
            EnemiesOnScene = new List<IEnemy>();
            _poolOfEnemies = new PoolOfObjects<IEnemy>(enemyPrefab, poolTransform);
            SetEnemies();
            _gameController = ControllersManager.Instance.GameController;
            _gameController.OnWayPointReached += SwitchOffKilledEnemies;
            _gameController.OnLevelSucceed += GoToNextLevel;
        }

        private void SetEnemies()
        {
            for (var i = 0; i < levels.Length; i++)
            {
                SetEnemiesOnLevel(i);
            }
        }
        
        private void SetEnemiesOnLevel(int lvlIndex)
        {
            for (var i = 0; i < levels[lvlIndex].CountOfEnemiesOnLvl; i++)
            {
                var newEnemy = CreateEnemy(levels[lvlIndex].EnemyPointsOnLevel[i]);
                if (!EnemiesOnScene.Contains(newEnemy))
                    EnemiesOnScene.Add(newEnemy);
            }
        }

        private IEnemy CreateEnemy(Transform enemyPoint)
        {
            var enemy = _poolOfEnemies.Get();
            enemy.SetStartParams(enemyPoint.position, enemyPoint.rotation);
            enemy.Switch(true);
            return enemy;
        }

        public void PlusOneEnemyKilled()
        {
            _killedEnemiesCount++;
        }

        public bool CheckIfLevelAimReached()
        {
            var enemiesOnCurrentLvl = levels[_currentLevel].CountOfEnemiesOnLvl;
            var aimReached = _killedEnemiesCount >= enemiesOnCurrentLvl;
            return aimReached;
        }

        public bool CheckIfLastLevel() => _currentLevel >= levels.Length;

        private void SwitchOffKilledEnemies()
        {
            for (var i = 0; i < EnemiesOnScene.Count; i++)
            {
                if (!EnemiesOnScene[i].IsKilled) continue;
                _poolOfEnemies.Put(EnemiesOnScene[i]);
                EnemiesOnScene[i].Switch(false);
            }
        }

        private void GoToNextLevel()
        {
            _killedEnemiesCount = 0;
            _currentLevel++;
        }
    }
}
