using GameOfLife;
using Spectre.Console;
using Array = GameOfLife.Array;

//ConsoleInteropService.Configure(ConsoleInteropServiceConfiguration.Default);

await Game.Run();

public class Game
{
    static Pattern.Name PromptPattern() =>
        AnsiConsole.Prompt(
            new SelectionPrompt<Pattern.Name>()
                .Title("Select [green]Pattern[/]:")
                .AddChoices(Enum.GetValues<Pattern.Name>()));
    public static async Task Run()
    {
        long Generation = 1; // Grid Height and Width

        var patternName = PromptPattern();
        var pattern = Pattern.Resolve(patternName);
        Console.WriteLine(pattern.Length);
        Console.WriteLine(pattern[0].Length);
        var speed = 300;

        DrawWithCanvas(pattern);

        AnsiConsole.MarkupLine($"•Speed: [green]{speed}ms[/]");
        AnsiConsole.MarkupLine($"•Pattern: [green]{patternName}[/]");
        AnsiConsole.MarkupLine($"•Generation: [green]{Generation}[/]");

        await Task.Delay(300);

        for (int i = 0; i < 100; i++)
        {
            Console.Clear();

            var nextGrid = ResolveNextGrid(pattern);

            DrawWithCanvas(nextGrid);

            AnsiConsole.MarkupLine($"•Speed: [green]{speed}ms[/]");
            AnsiConsole.MarkupLine($"•Pattern: [green]{patternName}[/]");
            AnsiConsole.MarkupLine($"•Generation: [green]{Generation}[/]");
            Generation++;

            pattern = nextGrid;
            await Task.Delay(300);
        }

        Console.ReadLine();
    }

    private static int[][] ResolveNextGrid(int[][] xs)
    {
        const int DEAD = 0;
        const int ALIVE = 1;

        var ys = Array.Create2D(xs.Length, xs[0].Length);

        for (int i = 0; i < xs.Length; i++)
        {
            for (int j = 0; j < xs[i].Length; j++)
            {
                var state = xs[i][j];

                var neighbors = CountNeighbors(xs, i, j);

                if (state is DEAD && neighbors is 0)
                {
                    ys[i][j] = DEAD; // Stay dead
                }
                else if (state is ALIVE && neighbors <= 1)
                {
                    ys[i][j] = DEAD; // Solitude
                }
                else if (state is ALIVE && neighbors >= 4)
                {
                    ys[i][j] = DEAD; // Overpopulation
                }
                else if (state is DEAD && neighbors is 3)
                {
                    ys[i][j] = ALIVE; // Reproduction
                }
                else
                {
                    ys[i][j] = state; // Stay alive
                }
            }
        }

        return ys;
    }

    private static int CountNeighbors(int[][] xs, int row, int col)
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

        /*
        sum += xs[i - 1][j - 1];
        sum += xs[i - 1][j];
        sum += xs[i - 1][j + 1];

        sum += xs[i][j - 1];
        sum += xs[i][j + 1];

        sum += xs[i + 1][j - 1];
        sum += xs[i + 1][j];
        sum += xs[i + 1][j + 1];
        */

        /*
        [-1][-1], [-1][0], [-1][1],
        [0][-1], [0][0], [0][1],
        [1][-1], [1][0], [1][1],
         */
    }

    private static void Draw(int[][] xs)
    {
        for (int i = 0; i < xs.Length; i++)
        {
            for (int j = 0; j < xs[i].Length; j++)
            {
                Console.Write(xs[i][j] + " ");
            }

            Console.WriteLine();
        }
    }

    private static void DrawWithCanvas(int[][] xs)
    {
        var canvas = new Canvas(xs.Length, xs[0].Length);

        for (int i = 0; i < xs.Length; i++)
        {
            for (int j = 0; j < xs[i].Length; j++)
            {
                var color = ResolveColor(xs[i][j]);

                // j is the X coordinate (column)
                // i is the Y coordinate (row)
                canvas.SetPixel(j, i, color);
            }
        }

        AnsiConsole.Write(canvas);

        static Color ResolveColor(int value) => value switch
        {
            0 => Color.White,
            1 => Color.Black,
            _ => throw new InvalidOperationException("Unexpected cell value")
        };
    }
}
