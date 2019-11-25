using Leopotam.Ecs;
using UnityEngine;

namespace SnakeGame {
    sealed class UserInputProcessing : IEcsRunSystem {
        readonly EcsFilter<Snake> _snakeFilter = null;

        void IEcsRunSystem.Run () {
            var x = Input.GetAxis ("Horizontal");
            var y = Input.GetAxis ("Vertical");
            if (new Vector2 (x, y).sqrMagnitude > 0.01f) {
                SnakeDirection direction;
                if (Mathf.Abs (x) > Mathf.Abs (y))
                    direction = x > 0f ? SnakeDirection.Right : SnakeDirection.Left;
                else
                    direction = y > 0f ? SnakeDirection.Up : SnakeDirection.Down;
                foreach (var i in _snakeFilter) {
                    var snake = _snakeFilter.Get1[i];
                    if (!AreDirectionsOpposite (direction, snake.Direction)) snake.Direction = direction;
                }
            }
        }

        static bool AreDirectionsOpposite (SnakeDirection a, SnakeDirection b) {
            // sort it first for simplify checks.
            if ((int) a > (int) b) {
                var t = a;
                a = b;
                b = t;
            }
            return a == SnakeDirection.Up && b == SnakeDirection.Down || a == SnakeDirection.Right && b == SnakeDirection.Left;
        }
    }
}