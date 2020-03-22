using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace SnakeGame {
    struct Coords {
        public int X;
        public int Y;

        public override string ToString () {
            return $"({X},{Y})";
        }
    }

    enum SnakeDirection {
        Up,
        Right,
        Down,
        Left
    }

    public class MovementProcessing : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem {
        const string SnakeTag = "Player";

        // delay between updates can be changed at runtime.
        float _delay = 0.2f;

        float _nextUpdateTime;

        readonly EcsWorld _world = null;
        readonly EcsFilter<Snake> _snakeFilter = null;
        readonly EcsFilter<SnakeSegment> _snakeSegmentFilter = null;

        void IEcsInitSystem.Init () {
            foreach (var unityObject in GameObject.FindGameObjectsWithTag (SnakeTag)) {
                var tr = unityObject.transform;
                ref var snake = ref _world.NewEntity ().Set<Snake> ();
                snake.Body = new List<EcsComponentRef<SnakeSegment>> (256);
                snake.Direction = SnakeDirection.Up;
                var headEntity = _world.NewEntity ();
                ref var head = ref headEntity.Set<SnakeSegment> ();
                var pos = tr.localPosition;
                head.Coords.X = (int) pos.x;
                head.Coords.Y = (int) pos.y;
                head.Transform = tr;
                snake.Body.Add (headEntity.Ref<SnakeSegment> ());
            }
        }

        void IEcsDestroySystem.Destroy () {
            foreach (var i in _snakeSegmentFilter) {
                _snakeSegmentFilter.GetEntity (i).Destroy ();
            }
        }

        void IEcsRunSystem.Run () {
            if (Time.time < _nextUpdateTime) {
                return;
            }
            _nextUpdateTime = Time.time + _delay;

            foreach (var snakeEntityId in _snakeFilter) {
                ref var snake = ref _snakeFilter.Get1 (snakeEntityId);
                if (snake.ShouldGrow) {
                    // just add new segment to body.
                    snake.ShouldGrow = false;
                    var headEntity = _world.NewEntity ();
                    ref var head = ref headEntity.Set<SnakeSegment> ();
                    head.Coords = GetForwardCoords (snake.Body[snake.Body.Count - 1].Unref ().Coords, snake.Direction);
                    head.Transform = Object.Instantiate (snake.Body[0].Unref ().Transform);
                    head.Transform.localPosition = new Vector3 (head.Coords.X, head.Coords.Y, 0f);
                    var headRef = headEntity.Ref<SnakeSegment> ();
                    snake.Body.Add (headRef);
                } else {
                    // move all body segments to new positions.
                    Coords coords;
                    for (var i = 0; i < snake.Body.Count - 1; i++) {
                        coords = snake.Body[i + 1].Unref ().Coords;
                        ref var segment = ref snake.Body[i].Unref ();
                        segment.Coords = coords;
                        segment.Transform.localPosition = new Vector3 (coords.X, coords.Y, 0f);
                    }
                    ref var head = ref snake.Body[snake.Body.Count - 1].Unref ();
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