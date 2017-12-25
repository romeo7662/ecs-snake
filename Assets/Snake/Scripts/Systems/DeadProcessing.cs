using LeopotamGroup.Ecs;
using UnityEngine;

sealed class DeadProcessing : IEcsSystem, IEcsUpdateSystem {
    [EcsWorld]
    EcsWorld _world;

    [EcsFilter (typeof (Snake))]
    EcsFilter _snakeFilter;

    [EcsFilter (typeof (SnakeSegment))]
    EcsFilter _snakeSegmentFilter;

    [EcsFilter (typeof (Obstacle))]
    EcsFilter _obstacleFilter;

    [EcsIndex (typeof (Snake))]
    int _snakeId;

    [EcsIndex (typeof (SnakeSegment))]
    int _snakeSegmentId;

    [EcsIndex (typeof (Obstacle))]
    int _obstacleId;

    void IEcsUpdateSystem.Update () {
        foreach (var snakeEntity in _snakeFilter.Entities) {
            var snake = _world.GetComponent<Snake> (snakeEntity, _snakeId);
            var snakeHead = snake.Body[snake.Body.Count - 1];
            var snakeCoords = snakeHead.Coords;
            foreach (var obstacleEntity in _obstacleFilter.Entities) {
                var obstacle = _world.GetComponent<Obstacle> (obstacleEntity, _obstacleId);
                if (snakeCoords.X == obstacle.Coords.X && snakeCoords.Y == obstacle.Coords.Y) {
                    _world.RemoveEntity (snakeEntity);
                    Debug.Log ("Snake killed");
                }
            }

            foreach (var segmentEntity in _snakeSegmentFilter.Entities) {
                var segment = _world.GetComponent<SnakeSegment> (segmentEntity, _snakeSegmentId);
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