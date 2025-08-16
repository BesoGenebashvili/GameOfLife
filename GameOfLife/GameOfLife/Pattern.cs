namespace GameOfLife;

public static class Pattern
{
    public enum Name : byte
    {
        Random,
        Bunnies,
        Glider,
        Blinkers,
        Toad,
        LWSS,
        Diamond,
        BulletHeptomino
    }

    public static int[,] Resolve(
        int gridSize,
        Name pattern)
    {
        var cleanGrid = Array.Create2D(gridSize, gridSize);

        return pattern switch
        {
            Name.Random => Random(cleanGrid),
            var p => ResolveCenteredPattern(p, cleanGrid),
        };

        static int[,] Random(int[,] cleanGrid)
        {
            cleanGrid.FillWithRandomBits();
            return cleanGrid;
        }

        static int[,] ResolveCenteredPattern(Name pattern, int[,] cleanGrid)
        {
            var patternData = pattern switch
            {
                Name.Bunnies => Bunnies,
                Name.Glider => Glider,
                Name.Blinkers => Blinkers,
                Name.Toad => Toad,
                Name.LWSS => LWSS,
                Name.Diamond => Diamond,
                Name.BulletHeptomino => BulletHeptomino,
                _ => throw new NotSupportedException("Pattern not supported")
            };

            return CenterPattern(cleanGrid, patternData);
        }
    }

    static int[,] CenterPattern(int[,] cleanGrid, int[,] pattern)
    {
        int rowOffset = (cleanGrid.GetLength(0) - pattern.GetLength(0)) / 2;
        int colOffset = (cleanGrid.GetLength(1) - pattern.GetLength(1)) / 2;

        for (int i = 0; i < pattern.GetLength(0); i++)
        {
            for (int j = 0; j < pattern.GetLength(1); j++)
            {
                cleanGrid[i + rowOffset, j + colOffset] = pattern[i, j];
            }
        }

        return cleanGrid;
    }


    /// <summary>
    /// This is a parent of rabbits and was found independently by Robert Wainwright and Andrew Trevorrow.
    /// https://playgameoflife.com/lexicon/bunnies
    /// </summary>
    private static int[,] Bunnies =>
        new int[,]
        {
            {1,0,0,0,0,0,1,0},
            {0,0,1,0,0,0,1,0},
            {0,0,1,0,0,1,0,1},
            {0,1,0,1,0,0,0,0}
        };

    private static int[,] Glider =>
        new int[,]
        {
            {0,1,0},
            {0,0,1},
            {1,1,1}
        };

    private static int[,] Blinkers =>
        new int[,]
        {
            {0,1,1,1,0,0,0,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,0,0,0,1,1,1,0}
        };

    private static int[,] Toad =>
    new int[,]
        {
            {0,1,1,1},
            {1,1,1,0}
        };

    private static int[,] LWSS =>
        new int[,]
        {
            {0,1,0,0,1},
            {1,0,0,0,0},
            {1,0,0,0,1},
            {1,1,1,1,0}
        };

    private static int[,] Diamond =>
        new int[,]
        {
            {0,1,0},
            {1,1,1},
            {1,0,1},
            {0,1,0}
        };

    private static int[,] BulletHeptomino =>
        new int[,]
        {
            {0,1,0},
            {1,1,1},
            {1,1,1}
        };
}
