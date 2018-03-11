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

    [EcsFilterInclude (typeof (ScoreChangeEvent))]
    EcsFilter _scoreFilter;

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
        for (var snakeEntityId = 0; snakeEntityId < _snakeFilter.EntitiesCount; snakeEntityId++) {
            var snake = _world.GetComponent<Snake> (_snakeFilter.Entities[snakeEntityId]);
            var snakeCoords = snake.Body[snake.Body.Count - 1].Coords;
            for (var foodEntityId = 0; foodEntityId < _foodFilter.EntitiesCount; foodEntityId++) {
                var food = _world.GetComponent<Food> (_foodFilter.Entities[foodEntityId]);
                if (food.Coords.X == snakeCoords.X && food.Coords.Y == snakeCoords.Y) {
                    snake.ShouldGrow = true;

                    // create score change event.
                    for (var scoreEntityId = 0; scoreEntityId < _scoreFilter.EntitiesCount; scoreEntityId++) {
                        _world.UpdateComponent<ScoreChangeEvent> (_scoreFilter.Entities[scoreEntityId]);
                    }

                    food.Coords.X = Random.Range (1, WorldWidth);
                    food.Coords.Y = Random.Range (1, WorldHeight);
                    food.Transform.localPosition = new Vector3 (food.Coords.X, food.Coords.Y, 0f);
                }
            }
        }
    }
}