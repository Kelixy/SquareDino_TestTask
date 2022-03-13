using System;
using System.Collections.Generic;
using System.Linq;
using Models;
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
            public List<IEnemy> EnemiesOnScene { get; set; }
        }
    
        [SerializeField] private Transform poolTransform;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Level[] levels;

        private PoolOfObjects<IEnemy> _poolOfEnemies;
        private GameController _gameController;
        private int _currentLevel;

        public void Initialize()
        {
            _poolOfEnemies = new PoolOfObjects<IEnemy>(enemyPrefab, poolTransform);
            
            InitializeEnemiesLists();
            SetEnemies();
            
            _gameController = ControllersManager.Instance.GameController;
            _gameController.OnLevelSucceed += () => { _currentLevel++; };
            _gameController.OnWayPointReached += () =>
            {
                if (_currentLevel > 0 && _currentLevel <= levels.Length)
                    SwitchOffEnemiesOnLvl(_currentLevel-1);
            };
        }
        
        private void InitializeEnemiesLists()
        {
            for (var i = 0; i < levels.Length; i++)
            {
                levels[i].EnemiesOnScene = new List<IEnemy>();
            }
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
                levels[lvlIndex].EnemiesOnScene.Add(newEnemy);
            }
        }

        private IEnemy CreateEnemy(Transform enemyPoint)
        {
            var enemy = _poolOfEnemies.Get();
            enemy.SetStartParams(enemyPoint.position, enemyPoint.rotation);
            enemy.Switch(true);
            return enemy;
        }

        public bool CheckIfLevelAimReached()
        {
            var enemiesOnCurrentLvl = levels[_currentLevel].CountOfEnemiesOnLvl;
            var killedEnemiesOnCurrentLvl = levels[_currentLevel].EnemiesOnScene.Count(x => x.IsKilled);
            var aimReached = killedEnemiesOnCurrentLvl >= enemiesOnCurrentLvl;
            return aimReached;
        }

        public bool CheckIfLastLevel() => _currentLevel >= levels.Length;

        private void SwitchOffEnemiesOnLvl(int lvl)
        {
            for (var i = 0; i < levels[lvl].EnemiesOnScene.Count; i++)
            {
                if (!levels[lvl].EnemiesOnScene[i].IsKilled) continue;
                _poolOfEnemies.Put(levels[lvl].EnemiesOnScene[i]);
                levels[lvl].EnemiesOnScene[i].Switch(false);
            }
        }
    }
}
