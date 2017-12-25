using LeopotamGroup.Ecs;
using UnityEngine;

sealed class Food : IEcsComponent {
    public Coords Coords;
    public Transform Transform;
}