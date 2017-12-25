using LeopotamGroup.Ecs;
using UnityEngine;

sealed class Obstacle : IEcsComponent {
    public Coords Coords;
    public Transform Transform;
}