using System.Collections.Generic;
using LeopotamGroup.Ecs;

sealed class Snake : IEcsComponent {
    public readonly List<SnakeSegment> Body = new List<SnakeSegment> (256);

    public SnakeDirection Direction = SnakeDirection.Up;

    public bool ShouldGrow;
}