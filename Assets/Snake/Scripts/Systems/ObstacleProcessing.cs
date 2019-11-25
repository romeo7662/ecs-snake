using Leopotam.Ecs;
using UnityEngine;

namespace SnakeGame {
    sealed class ObstacleProcessing : IEcsInitSystem, IEcsDestroySystem {
        const string ObstacleTag = "Finish";
        readonly EcsFilter<Obstacle> _obstacles = null;

        readonly EcsWorld _world = null;

        void IEcsDestroySystem.Destroy () {
            foreach (var i in _obstacles) _obstacles.Entities[i].Destroy ();
        }

        void IEcsInitSystem.Init () {
            foreach (var unityObject in GameObject.FindGameObjectsWithTag (ObstacleTag)) {
                var tr = unityObject.transform;
                _world.NewEntityWith<Obstacle> (out var obstacle);
                var pos = tr.localPosition;
                obstacle.Coords.X = (int) pos.x;
                obstacle.Coords.Y = (int) pos.y;
            }
        }
    }
}