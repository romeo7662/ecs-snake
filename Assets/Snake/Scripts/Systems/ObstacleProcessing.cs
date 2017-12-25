using LeopotamGroup.Ecs;
using UnityEngine;

public class ObstacleProcessing : IEcsSystem, IEcsInitSystem {
    const string ObstacleTag = "Finish";

    [EcsWorld]
    EcsWorld _world;

    void IEcsInitSystem.Initialize () {
        foreach (var unityObject in GameObject.FindGameObjectsWithTag (ObstacleTag)) {
            var tr = unityObject.transform;
            var entity = _world.CreateEntity ();

            var obstacle = _world.AddComponent<Obstacle> (entity);
            obstacle.Coords.X = (int) tr.localPosition.x;
            obstacle.Coords.Y = (int) tr.localPosition.y;
            obstacle.Transform = tr;
        }
    }

    void IEcsInitSystem.Destroy () { }
}