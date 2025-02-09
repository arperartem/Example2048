namespace Example2048
{
    public class Controller
    {
        public int Score { get; private set; }
        public bool GameOver { get; private set; }
        
        public BaseValue[,] BoardData { get; private set; }
        private int _rows;
        private int _columns;
        
        private Random _random;
        
        public void Initialize(BaseValue[,] boardData)
        {
            BoardData = boardData;
            
            _rows = BoardData.GetLength(0);
            _columns = BoardData.GetLength(1);
            
            if (_rows <= 1 || _columns <= 1)
                throw new Exception("Invalid board size");
            
            _random = new Random();
        }
        
        public void Reset()
        {
            for (var i = 0; i < _rows; i++)
            {
                for (var j = 0; j < _columns; j++)
                {
                    BoardData[i, j] = 0;
                }
            }
            
            Score = 0;
        }

        public bool Move(MoveDirection direction)
        {
            var moved = false;
            switch (direction)
            {
                case MoveDirection.Left:
                    moved = MoveLeft();
                    break;
                case MoveDirection.Right:
                    moved = MoveRight();
                    break;
                case MoveDirection.Up:
                    moved = MoveUp();
                    break;
                case MoveDirection.Down:
                    moved = MoveDown();
                    break;
            }
            
            if (moved)
            {
                AddRandomTile();
                
                if (!CanMove())
                    GameOver = true;
            }
            
            return moved;
        }
        
        private void AddRandomTile()
        {
            var emptyCells = new List<(int, int)>();
            
            for (var i = 0; i < _rows; i++)
            {
                for (var j = 0; j < _columns; j++)
                {
                    if (BoardData[i, j] == BaseValue.Zero)
                    {
                        emptyCells.Add((i, j));
                    }
                }
            }
            
            if (emptyCells.Count > 0)
            {
                var (i, j) = emptyCells[_random.Next(emptyCells.Count)];
                BoardData[i, j] = _random.NextDouble() < 0.9 ? BaseValue.Two : BaseValue.Four;
            }
            else
            {
                throw new Exception("Board is full");
            }
        }

        #region Move

        private bool MoveLeft()
        {
            var moved = false;
            for (var i = 0; i < _rows; i++)
            {
                var row = new BaseValue[_columns];
                
                for (var j = 0; j < _columns; j++)
                    row[j] = BoardData[i, j];
                
                var newRow = ProcessLine(row);
                for (var j = 0; j < _columns; j++)
                {
                    if (BoardData[i, j] != newRow[j])
                    {
                        moved = true;
                        BoardData[i, j] = newRow[j];
                    }
                }
            }
            return moved;
        }

        private bool MoveRight()
        {
            var moved = false;
            for (var i = 0; i < _rows; i++)
            {
                var row = new BaseValue[_columns];
                
                for (var j = 0; j < _columns; j++)
                    row[j] = BoardData[i, j];
                
                Array.Reverse(row);
                var newRow = ProcessLine(row);
                Array.Reverse(newRow);
                
                for (var j = 0; j < _columns; j++)
                {
                    if (BoardData[i, j] != newRow[j])
                    {
                        moved = true;
                        BoardData[i, j] = newRow[j];
                    }
                }
            }
            return moved;
        }
        
        private bool MoveUp()
        {
            var moved = false;
            for (var j = 0; j < _columns; j++)
            {
                var column = new BaseValue[_rows];
                
                for (var i = 0; i < _rows; i++)
                    column[i] = BoardData[i, j];
                
                var newColumn = ProcessLine(column);
                for (var i = 0; i < _rows; i++)
                {
                    if (BoardData[i, j] != newColumn[i])
                    {
                        moved = true;
                        BoardData[i, j] = newColumn[i];
                    }
                }
            }
            return moved;
        }
        
        private bool MoveDown()
        {
            var moved = false;
            for (var j = 0; j < _columns; j++)
            {
                var column = new BaseValue[_rows];
                for (var i = 0; i < _rows; i++)
                {
                    column[i] = BoardData[i, j];
                }
                Array.Reverse(column);
                var newColumn = ProcessLine(column);
                Array.Reverse(newColumn);
                
                for (var i = 0; i < _rows; i++)
                {
                    if (BoardData[i, j] != newColumn[i])
                    {
                        moved = true;
                        BoardData[i, j] = newColumn[i];
                    }
                }
            }
            return moved;
        }
        
        #endregion
        
        private BaseValue[] ProcessLine(BaseValue[] line)
        {
            // Убираем нули.
            var newLine = line.Where(num => num != BaseValue.Zero).ToArray();
            
            // Сливаем соседние равные элементы.
            for (var i = 0; i < newLine.Length - 1; i++)
            {
                if (newLine[i] == newLine[i + 1])
                {
                    newLine[i] *= BaseValue.Two;
                    Score += newLine[i];
                    newLine[i + 1] = BaseValue.Zero;
                    i++; // Пропускаем следующий элемент, поскольку он уже слит.
                }
            }

            // Убираем нули, возникшие после слияния, и дополняем оставшиеся позиции нулями.
            var result = newLine.Where(num => num != BaseValue.Zero).ToList();
            while (result.Count < line.Length)
                result.Add(BaseValue.Zero);
            
            return result.ToArray();
        }
        
        private bool CanMove()
        {
            for (var i = 0; i < _rows; i++)
            {
                for (var j = 0; j < _columns; j++)
                {
                    if (BoardData[i, j] == BaseValue.Zero)
                        return true;
                }
            }
            
            for (var i = 0; i < _rows; i++)
            {
                for (var j = 0; j < _columns - 1; j++)
                {
                    if (BoardData[i, j] == BoardData[i, j + 1])
                        return true;
                }
            }
            
            for (var j = 0; j < _columns; j++)
            {
                for (var i = 0; i < _rows - 1; i++)
                {
                    if (BoardData[i, j] == BoardData[i + 1, j])
                        return true;
                }
            }
            return false;
        }
    }
}