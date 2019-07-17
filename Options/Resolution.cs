namespace RubiksCube3D.Options
{
    public class Resolution
    {
        public int  Width { get; set; }
        public int Height { get; set; }
        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}