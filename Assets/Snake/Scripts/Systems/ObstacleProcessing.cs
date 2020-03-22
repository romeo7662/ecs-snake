using Leopotam.Ecs;
using UnityEngine;

namespace SnakeGame {
    public class ObstacleProcessing : IEcsInitSystem, IEcsDestroySystem {
        const string ObstacleTag = "Finish";

        readonly EcsWorld _world = null;
        readonly EcsFilter<Obstacle> _obstacles = null;

        void IEcsInitSystem.Init () {
            foreach (var unityObject in GameObject.FindGameObjectsWithTag (ObstacleTag)) {
                var tr = unityObject.transform;
                ref var obstacle = ref _world.NewEntity ().Set<Obstacle> ();
                var pos = tr.localPosition;
                obstacle.Coords.X = (int) pos.x;
                obstacle.Coords.Y = (int) pos.y;
                obstacle.Transform = tr;
            }
        }

        void IEcsDestroySystem.Destroy () {
            foreach (var i in _obstacles) {
                _obstacles.GetEntity (i).Destroy ();
            }
        }
    }
}