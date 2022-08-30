namespace Diamond_Square.Models
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int Height { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point(double x, double y, int height)
        {
            Height = height;
        }
    }
}