using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration;
using UnityEngine;

namespace SnakeGame {
    sealed class GameStartup : MonoBehaviour {
        EcsSystems _systems;
        EcsWorld _world;

        void OnEnable () {
            _world = new EcsWorld ();
#if UNITY_EDITOR
            EcsWorldObserver.Create (_world);
#endif
            _systems = new EcsSystems (_world)
                .Add (new ObstacleProcessing ())
                .Add (new UserInputProcessing ())
                .Add (new MovementProcessing ())
                .Add (new FoodProcessing ())
                .Add (new DeadProcessing ())
                .Add (new ScoreProcessing ());
            _systems.Init ();
#if UNITY_EDITOR
            EcsSystemsObserver.Create (_systems);
#endif
        }

        void Update () {
            _systems.Run ();
            _world.EndFrame ();
        }

        void OnDisable () {
            _systems.Destroy ();
            _systems = null;
            _world.Destroy ();
            _world = null;
        }
    }
}