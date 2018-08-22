using Leopotam.Ecs;
using UnityEngine;

[EcsInject]
public class ObstacleProcessing : IEcsInitSystem {
    const string ObstacleTag = "Finish";

    EcsWorld _world = null;

    EcsFilter<Obstacle> _obstacles = null;

    void IEcsInitSystem.Initialize () {
        foreach (var unityObject in GameObject.FindGameObjectsWithTag (ObstacleTag)) {
            var tr = unityObject.transform;
            var obstacle = _world.CreateEntityWith<Obstacle> ();
            obstacle.Coords.X = (int) tr.localPosition.x;
            obstacle.Coords.Y = (int) tr.localPosition.y;
            obstacle.Transform = tr;
        }
    }

    void IEcsInitSystem.Destroy () {
        for (var i = 0; i < _obstacles.EntitiesCount; i++) {
            _obstacles.Components1[i].Transform = null;
            _world.RemoveEntity (_obstacles.Entities[i]);
        }
    }
}