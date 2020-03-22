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

        void IEcsInitSystem.Init () {
            foreach (var unityObject in GameObject.FindGameObjectsWithTag (FoodTag)) {
                var tr = unityObject.transform;
                ref var food = ref _world.NewEntity ().Set<Food> ();
                var pos = tr.localPosition;
                food.Coords.X = (int) pos.x;
                food.Coords.Y = (int) pos.y;
                food.Transform = tr;
            }
        }

        void IEcsDestroySystem.Destroy () {
            foreach (var i in _foodFilter) {
                _foodFilter.GetEntity (i).Destroy ();
            }
        }

        void IEcsRunSystem.Run () {
            foreach (var snakeEntityId in _snakeFilter) {
                ref var snake = ref _snakeFilter.Get1 (snakeEntityId);
                var snakeCoords = snake.Body[snake.Body.Count - 1].Unref ().Coords;
                foreach (var foodEntityId in _foodFilter) {
                    ref var food = ref _foodFilter.Get1 (foodEntityId);
                    if (food.Coords.X == snakeCoords.X && food.Coords.Y == snakeCoords.Y) {
                        snake.ShouldGrow = true;

                        // create score change event.
                        ref var changeScore = ref _world.NewEntity ().Set<ScoreChangeEvent> ();
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