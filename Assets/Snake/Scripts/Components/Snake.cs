using System.Collections.Generic;

sealed class Snake {
    public readonly List<SnakeSegment> Body = new List<SnakeSegment> (256);

    public SnakeDirection Direction = SnakeDirection.Up;

    public bool ShouldGrow;
}