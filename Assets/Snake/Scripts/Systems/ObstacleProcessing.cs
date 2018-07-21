using Leopotam.Ecs;
using UnityEngine;

[EcsInject]
public class ObstacleProcessing : IEcsInitSystem {
    const string ObstacleTag = "Finish";

    EcsWorld _world = null;

    void IEcsInitSystem.Initialize () {
        foreach (var unityObject in GameObject.FindGameObjectsWithTag (ObstacleTag)) {
            var tr = unityObject.transform;
            var obstacle = _world.CreateEntityWith<Obstacle> ();
            obstacle.Coords.X = (int) tr.localPosition.x;
            obstacle.Coords.Y = (int) tr.localPosition.y;
            obstacle.Transform = tr;
        }
    }

    void IEcsInitSystem.Destroy () { }
}