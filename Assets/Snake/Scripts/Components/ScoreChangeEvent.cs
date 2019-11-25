using Leopotam.Ecs;

namespace SnakeGame {
    sealed class ScoreChangeEvent : IEcsOneFrame {
        public int Amount;
    }
}