using LeopotamGroup.Ecs;
using UnityEngine;

sealed class UserInputProcessing : IEcsSystem, IEcsUpdateSystem {
    [EcsWorld]
    EcsWorld _world;

    [EcsFilterInclude (typeof (Snake))]
    EcsFilter _snakeFilter;

    [EcsIndex (typeof (Snake))]
    int _snakeId;

    void IEcsUpdateSystem.Update () {
        var x = Input.GetAxis ("Horizontal");
        var y = Input.GetAxis ("Vertical");
        if (new Vector2 (x, y).sqrMagnitude > 0.01f) {
            SnakeDirection direction;
            if (Mathf.Abs (x) > Mathf.Abs (y)) {
                direction = x > 0f ? SnakeDirection.Right : SnakeDirection.Left;
            } else {
                direction = y > 0f ? SnakeDirection.Up : SnakeDirection.Down;
            }
            foreach (var snakeEntity in _snakeFilter.Entities) {
                var snake = _world.GetComponent<Snake> (snakeEntity, _snakeId);
                if (!AreDirectionsOpposite (direction, snake.Direction)) {
                    snake.Direction = direction;
                }
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
        return (a == SnakeDirection.Up && b == SnakeDirection.Down) || (a == SnakeDirection.Right && b == SnakeDirection.Left);
    }
}