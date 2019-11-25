using Leopotam.Ecs;
using UnityEngine;

namespace SnakeGame {
    sealed class SnakeSegment : IEcsAutoReset {
        public Coords Coords;
        public Transform Transform;

        void IEcsAutoReset.Reset () {
            Transform = null;
        }
    }
}