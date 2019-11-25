using Leopotam.Ecs;
using UnityEngine;

namespace SnakeGame {
    sealed class FoodProcessing : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem {
        const string FoodTag = "Respawn";

        // TODO: get correct world size from scene.
        const int WorldWidth = 24;
        const int WorldHeight = 15;
        readonly EcsWorld _world = null;
        readonly EcsFilter<Food> _foodFilter = null;
        readonly EcsFilter<Snake> _snakeFilter = null;

        void IEcsDestroySystem.Destroy () {
            foreach (var i in _foodFilter) _foodFilter.Entities[i].Destroy ();
        }

        void IEcsInitSystem.Init () {
            foreach (var unityObject in GameObject.FindGameObjectsWithTag (FoodTag)) {
                var tr = unityObject.transform;
                _world.NewEntityWith<Food> (out var food);
                var pos = tr.localPosition;
                food.Coords.X = (int) pos.x;
                food.Coords.Y = (int) pos.y;
                food.Transform = tr;
            }
        }

        void IEcsRunSystem.Run () {
            foreach (var snakeEntityId in _snakeFilter) {
                var snake = _snakeFilter.Get1[snakeEntityId];
                var snakeCoords = snake.Body[snake.Body.Count - 1].Coords;
                foreach (var foodEntityId in _foodFilter) {
                    var food = _foodFilter.Get1[foodEntityId];
                    if (food.Coords.X == snakeCoords.X && food.Coords.Y == snakeCoords.Y) {
                        snake.ShouldGrow = true;

                        // create score change event.
                        _world.NewEntityWith<ScoreChangeEvent> (out var changeScore);
                        changeScore.Amount = 1;

                        // respawn food at new position.
                        food.Coords.X = Random.Range (1, WorldWidth);
                        food.Coords.Y = Random.Range (1, WorldHeight);
                        food.Transform.localPosition = new Vector3 (food.Coords.X, food.Coords.Y, 0f);
                    }
                }
            }
        }
    }
}