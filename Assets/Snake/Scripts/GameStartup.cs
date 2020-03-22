using System.Collections;
using Leopotam.Ecs;
using UnityEngine;

namespace SnakeGame {
    public class GameStartup : MonoBehaviour {
        EcsWorld _world;
        EcsSystems _systems;

        void Start () {
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
#endif
            _systems
                // systems.
                .Add (new ObstacleProcessing ())
                .Add (new UserInputProcessing ())
                .Add (new MovementProcessing ())
                .Add (new FoodProcessing ())
                .Add (new DeadProcessing ())
                .Add (new ScoreProcessing ())
                // one-frames ScoreChangeEvent will be removed here.
                .OneFrame<ScoreChangeEvent> ()
                // init.
                .Init ();
        }

        void Update () {
            _systems?.Run ();
        }

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                _systems = null;
                _world.Destroy ();
                _world = null;
            }
        }
    }
}