using LeopotamGroup.Ecs;
using UnityEngine;

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

public class MovementProcessing : IEcsSystem, IEcsInitSystem, IEcsUpdateSystem {
    const string SnakeTag = "Player";

    // delay between updates can be changed at runtime.
    float _delay = 0.2f;

    float _nextUpdateTime;

    [EcsWorld]
    EcsWorld _world;

    [EcsFilter (typeof (Snake))]
    EcsFilter _snakeFilter;

    [EcsIndex (typeof (Snake))]
    int _snakeId;

    void IEcsInitSystem.Initialize () {
        foreach (var unityObject in GameObject.FindGameObjectsWithTag (SnakeTag)) {
            var tr = unityObject.transform;

            var snake = _world.AddComponent<Snake> (_world.CreateEntity ());

            var head = _world.AddComponent<SnakeSegment> (_world.CreateEntity ());
            head.Coords.X = (int) tr.localPosition.x;
            head.Coords.Y = (int) tr.localPosition.y;
            head.Transform = tr;
            snake.Body.Add (head);
        }
    }

    void IEcsInitSystem.Destroy () { }

    void IEcsUpdateSystem.Update () {
        if (Time.time < _nextUpdateTime) {
            return;
        }
        _nextUpdateTime = Time.time + _delay;

        foreach (var snakeEntity in _snakeFilter.Entities) {
            var snake = _world.GetComponent<Snake> (snakeEntity, _snakeId);
            SnakeSegment head;
            if (snake.ShouldGrow) {
                // just add new segment to body.
                snake.ShouldGrow = false;
                head = _world.AddComponent<SnakeSegment> (_world.CreateEntity ());
                head.Coords = GetForwardCoords (snake.Body[snake.Body.Count - 1].Coords, snake.Direction);
                head.Transform = GameObject.Instantiate (snake.Body[0].Transform.gameObject).transform;
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
                head = snake.Body[snake.Body.Count - 1];
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