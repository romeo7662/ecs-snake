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
        for (var i = 0; i < _snakeFilter.EntitiesCount; i++) {
            var snakeEntity = _snakeFilter.Entities[i];
            var snake = _world.GetComponent<Snake> (snakeEntity);
            var snakeHead = snake.Body[snake.Body.Count - 1];
            var snakeCoords = snakeHead.Coords;
            for (var ii = 0; ii < _obstacleFilter.EntitiesCount; ii++) {
                var obstacle = _world.GetComponent<Obstacle> (_obstacleFilter.Entities[ii]);
                if (snakeCoords.X == obstacle.Coords.X && snakeCoords.Y == obstacle.Coords.Y) {
                    _world.RemoveEntity (snakeEntity);
                    Debug.Log ("Snake killed");
                }
            }
            for (var ii = 0; ii < _snakeSegmentFilter.EntitiesCount; ii++) {
                var segment = _world.GetComponent<SnakeSegment> (_snakeSegmentFilter.Entities[ii]);
                if (segment.Coords.X == snakeCoords.X && segment.Coords.Y == snakeCoords.Y && segment != snakeHead) {
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