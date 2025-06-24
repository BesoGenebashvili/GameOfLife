namespace GameOfLife;

public sealed record GameConfiguration(
    int SpeedInMs,
    Pattern.Name PatternName)
{
    public static GameConfiguration Default { get; } = new(200, Pattern.Name.Random);
};

public static class Game
{
    public static async Task Play(GameConfiguration GameConfiguration)
    {
        var generation = 1;

        var pattern = Pattern.Resolve(GameConfiguration.PatternName);

        AnsiConsoleExtensions.DrawGame(
            pattern,
            GameConfiguration.SpeedInMs,
            GameConfiguration.PatternName,
            generation);

        await Task.Delay(GameConfiguration.SpeedInMs);

        while (true)
        {
            Console.Clear();

            var nextGrid = ResolveNextGeneration(pattern);

            pattern = nextGrid;

            AnsiConsoleExtensions.DrawGame(
                pattern,
                GameConfiguration.SpeedInMs,
                GameConfiguration.PatternName,
                generation++);

            await Task.Delay(GameConfiguration.SpeedInMs);

            if (Console.KeyAvailable)
            {
                Console.ReadKey(true);
                break;
            }
        }
    }

    private static int[][] ResolveNextGeneration(int[][] xs)
    {
        var ys = Array.Create2D(xs.Length, xs[0].Length);

        for (int i = 0; i < xs.Length; i++)
        {
            for (int j = 0; j < xs[i].Length; j++)
            {
                var state = xs[i][j];

                var neighbors = CountNeighbors(xs, i, j);

                ys[i][j] = ResolveNextState(state, neighbors);
            }
        }

        return ys;

        static int ResolveNextState(int currentState, int neighbors)
        {
            const int DEAD = 0;
            const int ALIVE = 1;

            return (currentState, neighbors) switch
            {
                (DEAD, 0) => DEAD, // Stay dead
                (ALIVE, <= 1) => DEAD, // Solitude
                (ALIVE, >= 4) => DEAD, // Overpopulation
                (DEAD, 3) => ALIVE, // Reproduction

                _ => currentState
            };
        }
    }

    /// <summary>
    /// Neighbors are the cells surrounding the current cell ([0, 0]):
    /// [-1][-1], [-1][0], [-1][1],
    /// [0][-1],  [0][0],  [0][1],
    /// [1][-1],  [1][0],  [1][1],
    /// </summary>
    private static int CountNeighbors2(int[][] xs, int row, int col)
    {
        int sum = 0;

        for (int i = row - 1; i < row + 2; i++)
        {
            for (int j = col - 1; j < col + 2; j++)
            {
                // Skip the center cell
                if (i == row && j == col)
                    continue;

                var r = (i + xs.Length) % xs.Length; // Wrap around rows
                var c = (j + xs[0].Length) % xs[0].Length; // Wrap around columns

                sum += xs[r][c];
            }
        }

        return sum;
    }

    private static int CountNeighbors(int[][] xs, int row, int col)
    {
        int sum = 0;
        int rows = xs.Length;
        int cols = xs[0].Length;

        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = col - 1; j <= col + 1; j++)
            {
                // Skip the center cell
                if (i == row && j == col)
                    continue;

                // Only count neighbors within bounds
                if (i >= 0 && i < rows && j >= 0 && j < cols)
                    sum += xs[i][j];
            }
        }

        return sum;
    }
}
