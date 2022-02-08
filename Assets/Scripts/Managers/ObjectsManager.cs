using System.Collections.Generic;
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

        public override void Initialize()
        {
            for (var i = 0; i < _gameObjectInstancesCount; i++)
            {
                var gameObjectInstance = Instantiate(_gameObjectPrefab);
                _gameObjectInstances.Add(gameObjectInstance);
            }
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
