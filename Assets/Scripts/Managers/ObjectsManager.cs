using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Helpers;
using UnityEngine;
using Views;
using Random = UnityEngine.Random;

namespace Managers
{
    public class ObjectsManager : BaseManager<ObjectsManager>
    {
        [SerializeField]
        private GameObject _gameObjectPrefab;
        [SerializeField]
        private int _gameObjectInstancesCount;
        [SerializeField]
        private GameObject _mainGameObjectPrefab;
        [SerializeField]
        private List<Vector3> _targetPoints = new List<Vector3>();
        [SerializeField]
        private float _mainGameObjectMovementDuration = 1f;
        [SerializeField]
        private float _mainGameObjectMovementDelaySeconds = 3f;

        private List<Vector3> _tempTargetPoints = new List<Vector3>();

        private List<GameObject> _gameObjectInstances = new List<GameObject>();
        private GameObject _mainGameObjectInstance;
        private readonly Vector3 MainGameObjectInitialPosition = new Vector3(0, 0, 0);
        private TweenerCore<Vector3, Vector3, VectorOptions> _doLocalMove;

        private const int GAME_OBJECTS_RANDOM_POSITION_X_MIN = -10;
        private const int GAME_OBJECTS_RANDOM_POSITION_X_MAX = 10;
        private const int GAME_OBJECTS_RANDOM_POSITION_Y_MIN = 0;
        private const int GAME_OBJECTS_RANDOM_POSITION_Y_MAX = 0;
        private const int GAME_OBJECTS_RANDOM_POSITION_Z_MIN = -10;
        private const int GAME_OBJECTS_RANDOM_POSITION_Z_MAX = 10;

        public override void Initialize()
        {
        }
        public override void UnInitialize()
        {
        }

        public override void Subscribe()
        {
        }
        protected override async void SubscribeAsync()
        {
            await TaskHelper.WaitUntil(() => CollisionDetectionManager.Instance != null);

            CollisionDetectionManager.Instance.TriggerEnter += CollisionDetectionManagerOnTriggerEnter;

            InstantiateGameObjects();

            InstantiateMainGameObject();

            var mainGameObjectMovementCoroutine = StartCoroutine(InvokeActionAfterDelayCoroutine(() =>
                {
                    _tempTargetPoints = new List<Vector3>(_targetPoints); // will be used in TryMoveToNextTargetPoint
                    TryMoveToNextTargetPoint();
                },
                _mainGameObjectMovementDelaySeconds));
        }
        public override void UnSubscribe()
        {
        }

        private void InstantiateGameObjects()
        {
            for (var i = 0; i < _gameObjectInstancesCount; i++)
            {
                var randomPosition = GetRandomPosition(
                    GAME_OBJECTS_RANDOM_POSITION_X_MIN, GAME_OBJECTS_RANDOM_POSITION_X_MAX,
                    GAME_OBJECTS_RANDOM_POSITION_Y_MIN, GAME_OBJECTS_RANDOM_POSITION_Y_MAX,
                    GAME_OBJECTS_RANDOM_POSITION_Z_MIN, GAME_OBJECTS_RANDOM_POSITION_Z_MAX);
                while (_gameObjectInstances.Any(go => go.transform.position == randomPosition) ||
                       randomPosition == MainGameObjectInitialPosition)
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

        private void InstantiateMainGameObject()
        {
            _mainGameObjectInstance = Instantiate(_mainGameObjectPrefab, MainGameObjectInitialPosition, Quaternion.identity);
        }

        private void TryMoveToNextTargetPoint()
        {
            if (_mainGameObjectInstance == null)
            {
                return;
            }
            if (!_tempTargetPoints.Any())
            {
                if (ExplosionManager.Instance != null)
                {
                    ExplosionManager.Instance.Explode(_mainGameObjectInstance);
                }

                return;
            }

            var targetPoint = _tempTargetPoints.FirstOrDefault();
            _tempTargetPoints.Remove(targetPoint);
            _doLocalMove = _mainGameObjectInstance.transform.DOLocalMove(
                targetPoint, _mainGameObjectMovementDuration);
            _doLocalMove.OnComplete(TryMoveToNextTargetPoint);
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

        private void TryHandleVaultPlayerCollisionEnter(IView view1, IView view2)
        {
            var gameObjectView = view1 as GameObjectView;
            var mainGameObjectView = view2 as MainGameObjectView;

            if (gameObjectView == null ||
                mainGameObjectView == null)
            {
                return;
            }

            _doLocalMove.Kill();
            ExplosionManager.Instance.Explode(_mainGameObjectInstance);
        }

        private void CollisionDetectionManagerOnTriggerEnter(IView view1, IView view2)
        {
            TryHandleVaultPlayerCollisionEnter(view1, view2);
        }

        private IEnumerator InvokeActionAfterDelayCoroutine(Action action, float delaySeconds = 1f)
        {
            yield return new WaitForSeconds(delaySeconds);

            action?.Invoke();
        }
    }
}
