namespace OPR.lb1
{
    public class Point<T>
    {
        public T x, y;
        public int Number;

        public Point()
        {
            this.x = default(T);
            this.y = default(T);
            Number = 0;
        }

        public Point(T x, T y)
        {
            this.x = x;
            this.y = y;
            Number = 0;
        }

        public Point(T x, T y, int number) : this(x, y)
        {
            Number = number;
        }
    }

    public class SquarePoint : Point<float>
    {
        public SquarePoint()
        {
            x = default(float);
            y = default(float);
        }

        public SquarePoint(float x, float y, int number) : base(x, y, number) { }

        public SquarePoint(float x, float y) : base(x, y) { }
    }
}
