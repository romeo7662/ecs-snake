using Leopotam.Ecs;
using UnityEngine;

namespace SnakeGame {
    sealed class DeadProcessing : IEcsRunSystem {
        readonly EcsFilter<Snake> _snakeFilter = null;
        readonly EcsFilter<SnakeSegment> _snakeSegmentFilter = null;
        readonly EcsFilter<Obstacle> _obstacleFilter = null;

        void IEcsRunSystem.Run () {
            foreach (var snakeIdx in _snakeFilter) {
                ref var snakeEntity = ref _snakeFilter.GetEntity(snakeIdx);
                ref var snake = ref _snakeFilter.Get1 (snakeIdx);
                var snakeHeadRef = snake.Body[snake.Body.Count - 1];
                ref var snakeHead = ref snakeHeadRef.Unref ();
                var snakeCoords = snakeHead.Coords;
                foreach (var obstacleIdx in _obstacleFilter) {
                    ref var obstacle = ref _obstacleFilter.Get1 (obstacleIdx);
                    if (snakeCoords.X == obstacle.Coords.X && snakeCoords.Y == obstacle.Coords.Y) {
                        snakeEntity.Destroy ();
                        Debug.Log ("Snake killed");
                    }
                }
                foreach (var snakeSegmentIdx in _snakeSegmentFilter) {
                    ref var segment = ref _snakeSegmentFilter.Get1 (snakeSegmentIdx);
                    var segmentRef = _snakeSegmentFilter.Get1Ref (snakeSegmentIdx);
                    if (segment.Coords.X == snakeCoords.X && segment.Coords.Y == snakeCoords.Y && segmentRef != snakeHeadRef) {
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
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }
}