using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

namespace SnakeGame {
    sealed class DeadProcessing : IEcsRunSystem {
        readonly EcsFilter<Obstacle> _obstacleFilter = null;
        readonly EcsFilter<Snake> _snakeFilter = null;
        readonly EcsFilter<SnakeSegment> _snakeSegmentFilter = null;

        void IEcsRunSystem.Run () {
            foreach (var snakeIdx in _snakeFilter) {
                var snakeEntity = _snakeFilter.Entities[snakeIdx];
                var snake = _snakeFilter.Get1[snakeIdx];
                var snakeHead = snake.Body[snake.Body.Count - 1];
                var snakeCoords = snakeHead.Coords;
                foreach (var obstacleIdx in _obstacleFilter) {
                    var obstacle = _obstacleFilter.Get1[obstacleIdx];
                    if (snakeCoords.X == obstacle.Coords.X && snakeCoords.Y == obstacle.Coords.Y) {
                        snake.Body.Clear ();
                        snakeEntity.Destroy ();
                        Debug.Log ("Snake killed");
                    }
                }
                foreach (var snakeSegmentIdx in _snakeSegmentFilter) {
                    var segment = _snakeSegmentFilter.Get1[snakeSegmentIdx];
                    if (segment.Coords.X == snakeCoords.X && segment.Coords.Y == snakeCoords.Y && segment != snakeHead) {
                        snake.Body.Clear ();
                        snakeEntity.Destroy ();
                        Debug.Log ("Snake killed");
                        break;
                    }
                }
            }
            if (_snakeFilter.IsEmpty ()) {
                // no snakes - exit.
                Application.Quit ();
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
            }
        }
    }
}