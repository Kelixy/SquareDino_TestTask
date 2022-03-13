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

        public List<Enemy> EnemiesOnScene { get; private set; }
        private PoolOfObjects<IEnemy> _poolOfEnemies;
        private int _currentLevel;
        private int _killedEnemiesCount;

        public void Initialize()
        {
            EnemiesOnScene = new List<Enemy>();
            _poolOfEnemies = new PoolOfObjects<IEnemy>(enemyPrefab, poolTransform);
            SetEnemies();
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
                EnemiesOnScene.Add((Enemy)newEnemy);
            }
        }

        private IEnemy CreateEnemy(Transform enemyPoint)
        {
            var enemy = _poolOfEnemies.Get();
            enemy.SetStartParams(enemyPoint.position, enemyPoint.rotation);
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

        public void GoToNextLevel()
        {
            _killedEnemiesCount = 0;
            _currentLevel++;
            ControllersManager.Instance.HeroController.MoveToNextPoint();
        }
    }
}
