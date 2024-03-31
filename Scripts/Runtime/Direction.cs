namespace Chinchillada.GridGraphs
{
    using System;
    using UnityEngine;

    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public static class DirectionExtensions
    {
        public static Vector2Int ToVector2(this Direction direction)
        {
            return direction switch
            {
                Direction.North => new Vector2Int(0, -1),
                Direction.East  => new Vector2Int(1, 0),
                Direction.South => new Vector2Int(0, 1),
                Direction.West  => new Vector2Int(-1, 0),
                _               => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}