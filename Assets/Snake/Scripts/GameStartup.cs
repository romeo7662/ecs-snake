using LeopotamGroup.Ecs;
using UnityEngine;

public class GameStartup : MonoBehaviour {
    EcsWorld _world;

    void OnEnable () {
        _world = new EcsWorld ()
            .AddSystem (new ObstacleProcessing ())
            .AddSystem (new UserInputProcessing ())
            .AddSystem (new MovementProcessing ())
            .AddSystem (new FoodProcessing ())
            .AddSystem (new DeadProcessing ())
            .AddSystem (new ScoreProcessing ());
        _world.Initialize ();
    }

    void Update () {
        _world.RunUpdate ();
    }

    void OnDisable () {
        _world.Destroy ();
    }
}