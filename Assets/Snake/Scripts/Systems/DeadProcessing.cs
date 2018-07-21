using Leopotam.Ecs;
using UnityEngine;

[EcsInject]
sealed class DeadProcessing : IEcsRunSystem {
    EcsWorld _world = null;

    EcsFilter<Snake> _snakeFilter = null;

    EcsFilter<SnakeSegment> _snakeSegmentFilter = null;

    EcsFilter<Obstacle> _obstacleFilter = null;

    void IEcsRunSystem.Run () {
        for (var i = 0; i < _snakeFilter.EntitiesCount; i++) {
            var snakeEntity = _snakeFilter.Entities[i];
            var snake = _snakeFilter.Components1[i];
            var snakeHead = snake.Body[snake.Body.Count - 1];
            var snakeCoords = snakeHead.Coords;
            for (var ii = 0; ii < _obstacleFilter.EntitiesCount; ii++) {
                var obstacle = _obstacleFilter.Components1[ii];
                if (snakeCoords.X == obstacle.Coords.X && snakeCoords.Y == obstacle.Coords.Y) {
                    snake.Body.Clear ();
                    _world.RemoveEntity (snakeEntity);
                    Debug.Log ("Snake killed");
                }
            }
            for (var ii = 0; ii < _snakeSegmentFilter.EntitiesCount; ii++) {
                var segment = _snakeSegmentFilter.Components1[ii];
                if (segment.Coords.X == snakeCoords.X && segment.Coords.Y == snakeCoords.Y && segment != snakeHead) {
                    snake.Body.Clear ();
                    _world.RemoveEntity (snakeEntity);
                    Debug.Log ("Snake killed");
                    break;
                }
            }
        }
        if (_snakeFilter.EntitiesCount == 0) {
            // no snakes - exit.
            Application.Quit ();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}