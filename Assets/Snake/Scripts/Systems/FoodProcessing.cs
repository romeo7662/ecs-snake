using LeopotamGroup.Ecs;
using UnityEngine;

sealed class FoodProcessing : IEcsInitSystem, IEcsRunSystem {
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

    void IEcsInitSystem.Initialize () {
        foreach (var unityObject in GameObject.FindGameObjectsWithTag (FoodTag)) {
            var tr = unityObject.transform;
            var food = _world.CreateEntityWith<Food> ();
            food.Coords.X = (int) tr.localPosition.x;
            food.Coords.Y = (int) tr.localPosition.y;
            food.Transform = tr;
        }
    }

    void IEcsInitSystem.Destroy () { }

    void IEcsRunSystem.Run () {
        foreach (var snakeEntity in _snakeFilter.Entities) {
            var snake = _world.GetComponent<Snake> (snakeEntity);
            var snakeCoords = snake.Body[snake.Body.Count - 1].Coords;
            foreach (var foodEntity in _foodFilter.Entities) {
                var food = _world.GetComponent<Food> (foodEntity);
                if (food.Coords.X == snakeCoords.X && food.Coords.Y == snakeCoords.Y) {
                    snake.ShouldGrow = true;

                    // create score change event.
                    _world.CreateEntityWith<ScoreChangeEvent> ();

                    food.Coords.X = Random.Range (1, WorldWidth);
                    food.Coords.Y = Random.Range (1, WorldHeight);
                    food.Transform.localPosition = new Vector3 (food.Coords.X, food.Coords.Y, 0f);
                }
            }
        }
    }
}