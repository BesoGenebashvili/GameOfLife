namespace GameOfLife;

public static class Array
{
    public static int[,] Create2D(int width, int height) =>
        new int[width, height];

    public static void FillWithRandomBits(this int[,] xs)
    {
        FillRandomly(0, 2);

        void FillRandomly(int min, int max)
        {
            var random = new Random();

            for (int i = 0; i < xs.GetLength(0); i++)
                for (int j = 0; j < xs.GetLength(1); j++)
                    xs[i, j] = random.Next(min, max);
        }
    }
}