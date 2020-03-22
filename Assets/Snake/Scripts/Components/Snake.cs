using System.Collections.Generic;
using Leopotam.Ecs;

namespace SnakeGame {
    struct Snake {
        public List<EcsComponentRef<SnakeSegment>> Body;
        public SnakeDirection Direction;
        public bool ShouldGrow;
    }
}