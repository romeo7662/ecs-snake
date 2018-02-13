using LeopotamGroup.Ecs;
using UnityEngine;

sealed class DeadProcessing : IEcsRunSystem {
    [EcsWorld]
    EcsWorld _world;

    [EcsFilterInclude (typeof (Snake))]
    EcsFilter _snakeFilter;

    [EcsFilterInclude (typeof (SnakeSegment))]
    EcsFilter _snakeSegmentFilter;

    [EcsFilterInclude (typeof (Obstacle))]
    EcsFilter _obstacleFilter;

    void IEcsRunSystem.Run () {
        foreach (var snakeEntity in _snakeFilter.Entities) {
            var snake = _world.GetComponent<Snake> (snakeEntity);
            var snakeHead = snake.Body[snake.Body.Count - 1];
            var snakeCoords = snakeHead.Coords;
            foreach (var obstacleEntity in _obstacleFilter.Entities) {
                var obstacle = _world.GetComponent<Obstacle> (obstacleEntity);
                if (snakeCoords.X == obstacle.Coords.X && snakeCoords.Y == obstacle.Coords.Y) {
                    _world.RemoveEntity (snakeEntity);
                    Debug.Log ("Snake killed");
                }
            }

            foreach (var segmentEntity in _snakeSegmentFilter.Entities) {
                var segment = _world.GetComponent<SnakeSegment> (segmentEntity);
                if (segment.Coords.X == snakeCoords.X && segment.Coords.Y == snakeCoords.Y && segment != snakeHead) {
                    _world.RemoveEntity (snakeEntity);
                    Debug.Log ("Snake killed");
                    break;
                }
            }
        }
        if (_snakeFilter.Entities.Count == 0) {
            // no snakes - exit.
            Application.Quit ();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}