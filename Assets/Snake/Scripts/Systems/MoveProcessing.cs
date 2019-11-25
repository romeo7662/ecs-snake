using Leopotam.Ecs;
using UnityEngine;

namespace SnakeGame {
    struct Coords {
        public int X;
        public int Y;
    }

    enum SnakeDirection {
        Up,
        Right,
        Down,
        Left
    }

    sealed class MovementProcessing : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem {
        const string SnakeTag = "Player";
        readonly EcsFilter<Snake> _snakeFilter = null;
        readonly EcsFilter<SnakeSegment> _snakeSegmentFilter = null;

        readonly EcsWorld _world = null;

        // delay between updates can be changed at runtime.
        readonly float _delay = 0.2f;

        float _nextUpdateTime;

        void IEcsDestroySystem.Destroy () {
            foreach (var i in _snakeSegmentFilter) _snakeSegmentFilter.Entities[i].Destroy ();
        }

        void IEcsInitSystem.Init () {
            foreach (var unityObject in GameObject.FindGameObjectsWithTag (SnakeTag)) {
                var tr = unityObject.transform;
                _world.NewEntityWith<Snake> (out var snake);
                _world.NewEntityWith<SnakeSegment> (out var head);
                var pos = tr.localPosition;
                head.Coords.X = (int) pos.x;
                head.Coords.Y = (int) pos.y;
                head.Transform = tr;
                snake.Body.Add (head);
            }
        }

        void IEcsRunSystem.Run () {
            if (Time.time < _nextUpdateTime) return;
            _nextUpdateTime = Time.time + _delay;

            foreach (var snakeEntityId in _snakeFilter) {
                var snake = _snakeFilter.Get1[snakeEntityId];
                if (snake.ShouldGrow) {
                    // just add new segment to body.
                    snake.ShouldGrow = false;
                    _world.NewEntityWith<SnakeSegment> (out var head);
                    head.Coords = GetForwardCoords (snake.Body[snake.Body.Count - 1].Coords, snake.Direction);
                    head.Transform = Object.Instantiate (snake.Body[0].Transform.gameObject).transform;
                    head.Transform.localPosition = new Vector3 (head.Coords.X, head.Coords.Y, 0f);
                    snake.Body.Add (head);
                } else {
                    // move all body segments to new positions.
                    Coords coords;
                    for (var i = 0; i < snake.Body.Count - 1; i++) {
                        coords = snake.Body[i + 1].Coords;
                        snake.Body[i].Coords = coords;
                        snake.Body[i].Transform.localPosition = new Vector3 (coords.X, coords.Y, 0f);
                    }
                    var head = snake.Body[snake.Body.Count - 1];
                    coords = GetForwardCoords (head.Coords, snake.Direction);
                    head.Coords = coords;
                    head.Transform.localPosition = new Vector3 (coords.X, coords.Y, 0f);
                }
            }
        }

        static Coords GetForwardCoords (Coords coords, SnakeDirection direction) {
            switch (direction) {
                case SnakeDirection.Up:
                    coords.Y++;
                    break;
                case SnakeDirection.Right:
                    coords.X++;
                    break;
                case SnakeDirection.Down:
                    coords.Y--;
                    break;
                case SnakeDirection.Left:
                    coords.X--;
                    break;
            }
            return coords;
        }
    }
}