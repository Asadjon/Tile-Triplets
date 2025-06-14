using System;

namespace Assets.Scripts
{
    public enum Shape { Circle, Square, Pentagon }
    public enum Animal { Cat, Dog, Rabbit}
    public enum FrameColor { Red, Green, Blue }

    [Serializable]
    public struct TileType
    {
        public Shape shape;
        public Animal animal;
        public FrameColor color;

        public TileType(Shape shape, Animal animal, FrameColor color)
        {
            this.shape = shape;
            this.animal = animal;
            this.color = color;
        }

        public readonly bool Equals(TileType other) =>
            shape == other.shape && animal == other.animal && color == other.color;
    }
}
