namespace Example2048;

internal class Program
{
    static void Main(string[] args)
    {
        var board = new Controller();

        var data = new BaseValue[,]
        {
            { 2, 2, 2, 4 },
            { 2, 0, 2048, 2048 },
            { 8, 8, 2, 8 },
            { 2, 2, 512, 512 }
        };
        
        PrintBoardWithBorders(data);

        board.Initialize(data);

        board.Move(MoveDirection.Left);
        
        Console.WriteLine("Result:");
        
        PrintBoardWithBorders(board.BoardData);
        
        Console.ReadLine();
    }
    
    public static void PrintBoardWithBorders(BaseValue[,] board)
    {
        var rows = board.GetLength(0);
        var cols = board.GetLength(1);

        var horizontalLine = new string('-', cols * 7 + cols + 1);

        Console.WriteLine(horizontalLine);
        for (var i = 0; i < rows; i++)
        {
            Console.Write("|");
            for (var j = 0; j < cols; j++)
            {
                Console.Write($"{board[i, j],6} |");
            }
            Console.WriteLine();
            Console.WriteLine(horizontalLine);
        }
    }
}