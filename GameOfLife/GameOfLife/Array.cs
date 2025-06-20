namespace GameOfLife;

public static class Array
{
    public static int[][] Create2D(int rows, int cols)
    {
        int[][] xs = new int[rows][];

        for (int i = 0; i < rows; i++)
        {
            xs[i] = new int[cols];
        }

        return xs;
    }

    public static void FillRandomly(this int[][] xs, int min, int max)
    {
        var random = new Random();

        for (int i = 0; i < xs.Length; i++)
        {
            for (int j = 0; j < xs[i].Length; j++)
            {
                xs[i][j] = random.Next(min, max);
            }
        }
    }

    public static void FillWithRandomBits(this int[][] xs) =>
        FillRandomly(xs, 0, 2);
}