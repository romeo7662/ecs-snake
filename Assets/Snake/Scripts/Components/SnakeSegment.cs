using LeopotamGroup.Ecs;
using UnityEngine;

sealed class SnakeSegment : IEcsComponent {
    public Coords Coords;
    public Transform Transform;
}