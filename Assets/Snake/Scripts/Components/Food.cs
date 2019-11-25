using Leopotam.Ecs;
using UnityEngine;

namespace SnakeGame {
    sealed class Food : IEcsAutoReset {
        public Coords Coords;
        public Transform Transform;

        void IEcsAutoReset.Reset () {
            Transform = null;
        }
    }
}