using LeopotamGroup.Ecs;
using UnityEngine;

sealed class FoodProcessing : IEcsSystem, IEcsInitSystem, IEcsUpdateSystem {
    const string FoodTag = "Respawn";

    // TODO: get correct world size from scene.
    const int WorldWidth = 24;

    const int WorldHeight = 15;

    [EcsWorld]
    EcsWorld _world;

    [EcsFilterInclude (typeof (Food))]
    EcsFilter _foodFilter;

    [EcsFilterInclude (typeof (Snake))]
    EcsFilter _snakeFilter;

    [EcsIndex (typeof (Food))]
    int _foodId;

    [EcsIndex (typeof (Snake))]
    int _snakeId;

    void IEcsInitSystem.Initialize () {
        foreach (var unityObject in GameObject.FindGameObjectsWithTag (FoodTag)) {
            var tr = unityObject.transform;
            var entity = _world.CreateEntity ();

            var food = _world.AddComponent<Food> (entity);
            food.Coords.X = (int) tr.localPosition.x;
            food.Coords.Y = (int) tr.localPosition.y;
            food.Transform = tr;
        }
    }

    void IEcsInitSystem.Destroy () { }

    void IEcsUpdateSystem.Update () {
        foreach (var snakeEntity in _snakeFilter.Entities) {
            var snake = _world.GetComponent<Snake> (snakeEntity, _snakeId);
            var snakeCoords = snake.Body[snake.Body.Count - 1].Coords;
            foreach (var foodEntity in _foodFilter.Entities) {
                var food = _world.GetComponent<Food> (foodEntity, _foodId);
                if (food.Coords.X == snakeCoords.X && food.Coords.Y == snakeCoords.Y) {
                    snake.ShouldGrow = true;
                    _world.SendEvent (new ScoreChanged ());
                    food.Coords.X = Random.Range (1, WorldWidth);
                    food.Coords.Y = Random.Range (1, WorldHeight);
                    food.Transform.localPosition = new Vector3 (food.Coords.X, food.Coords.Y, 0f);
                }
            }
        }
    }
}