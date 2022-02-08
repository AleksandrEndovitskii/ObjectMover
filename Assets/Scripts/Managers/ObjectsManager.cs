using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class ObjectsManager : BaseManager<ObjectsManager>
    {
        [SerializeField]
        private GameObject _gameObjectPrefab;
        [SerializeField]
        private int _gameObjectInstancesCount;

        private List<GameObject> _gameObjectInstances = new List<GameObject>();

        private const int GAME_OBJECTS_RANDOM_POSITION_X_MIN = -10;
        private const int GAME_OBJECTS_RANDOM_POSITION_X_MAX = 10;
        private const int GAME_OBJECTS_RANDOM_POSITION_Y_MIN = 0;
        private const int GAME_OBJECTS_RANDOM_POSITION_Y_MAX = 0;
        private const int GAME_OBJECTS_RANDOM_POSITION_Z_MIN = -10;
        private const int GAME_OBJECTS_RANDOM_POSITION_Z_MAX = 10;

        public override void Initialize()
        {
            for (var i = 0; i < _gameObjectInstancesCount; i++)
            {
                var randomPosition = GetRandomPosition(
                    GAME_OBJECTS_RANDOM_POSITION_X_MIN, GAME_OBJECTS_RANDOM_POSITION_X_MAX,
                    GAME_OBJECTS_RANDOM_POSITION_Y_MIN,GAME_OBJECTS_RANDOM_POSITION_Y_MAX,
                    GAME_OBJECTS_RANDOM_POSITION_Z_MIN, GAME_OBJECTS_RANDOM_POSITION_Z_MAX);
                while (_gameObjectInstances.Any(go => go.transform.position == randomPosition))
                {
                    randomPosition = GetRandomPosition(
                        GAME_OBJECTS_RANDOM_POSITION_X_MIN, GAME_OBJECTS_RANDOM_POSITION_X_MAX,
                        GAME_OBJECTS_RANDOM_POSITION_Y_MIN, GAME_OBJECTS_RANDOM_POSITION_Y_MAX,
                        GAME_OBJECTS_RANDOM_POSITION_Z_MIN, GAME_OBJECTS_RANDOM_POSITION_Z_MAX);
                }
                var gameObjectInstance = Instantiate(_gameObjectPrefab, randomPosition, Quaternion.identity);
                _gameObjectInstances.Add(gameObjectInstance);
            }
        }

        private static Vector3 GetRandomPosition(int randomPositionXMin, int randomPositionXMax,
            int randomPositionYMin, int randomPositionYMax,
            int randomPositionZMin, int randomPositionZMax)
        {
            var randomPositionX = Random.Range(randomPositionXMin, randomPositionXMax);
            var randomPositionY = Random.Range(randomPositionYMin, randomPositionYMax);
            var randomPositionZ = Random.Range(randomPositionZMin, randomPositionZMax);
            var randomPosition = new Vector3(randomPositionX, randomPositionY, randomPositionZ);
            return randomPosition;
        }

        public override void UnInitialize()
        {
        }

        public override void Subscribe()
        {
        }
        public override void UnSubscribe()
        {
        }
    }
}
