using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAConnectFour;

internal class Board
{

    public char[,] Grid { get; set; }
    public List<int> TopPieceIndex { get; set; }
    // private HashSet<Board> _checked = [];
    public int Width { get; set; }
    public int Height { get; set; }
    public int PiecesToWin { get; set; }
    public int BallsCount { get; set; }
    public int LastColumnPlayed { get; set; }
    public Board(int width, int height, int piecesToWIn)
    {
        BallsCount = 0;

        Width = width;
        Height = height;
        PiecesToWin = piecesToWIn;

        Grid = new char[Width, Height];
        TopPieceIndex = Enumerable.Repeat(Height, Width).ToList();

        for (int i = 0; i < Grid.GetLength(0); i++)
            for (int j = 0; j < Grid.GetLength(1); j++)
                Grid[i, j] = ' ';
    }

    public Board(Board board)
    {
        Width = board.Width;
        Height = board.Height;
        PiecesToWin = board.PiecesToWin;
        BallsCount = board.BallsCount;

        LastColumnPlayed = board.LastColumnPlayed;

        TopPieceIndex = [];
        TopPieceIndex.AddRange(board.TopPieceIndex);

        Grid = new char[Width, Height];

        for (int i = 0; i < Grid.GetLength(0); i++)
            for (int j = 0; j < Grid.GetLength(1); j++)
                Grid[i, j] = board.Grid[i, j];
    }

    public List<Board> AllNextMoves(char nextPlayer)
    {
        List<Board> nextBoards = [];

        for (int i = 0; i < Width; i++)
        {
            if (TopPieceIndex[i] != 0)
            {
                Board nextBoard = new(this);
                nextBoard.Play(nextPlayer, i);

                nextBoards.Add(nextBoard);
            }

            if (CheckHeight(i) && LastColumnPlayed != i && Grid[TopPieceIndex[i], i] != nextPlayer)
            {
                Board nextBoard = new(this);
                nextBoard.Erase(nextPlayer, i);

                nextBoards.Add(nextBoard);
            }

            for (int j = i + 1; j < Width; j++)
            {
                if (CheckHeight(i) && CheckHeight(j) && Grid[TopPieceIndex[i], i] != Grid[TopPieceIndex[j], j])
                {
                    Board nextBoard = new(this);
                    nextBoard.Swap(i, j);

                    nextBoards.Add(nextBoard);
                }
            }
        }

        return nextBoards;
    }

    public bool Play(char player, int column)
    {
        if (TopPieceIndex[column] != 0)
        {
            TopPieceIndex[column]--;
            LastColumnPlayed = column;

            Grid[TopPieceIndex[column], column] = player;
            BallsCount++;

            return true;
        }

        return false;
    }

    public bool Swap(int column1, int column2)
    {
        if (CheckHeight(column1) && CheckHeight(column2)
            && Grid[TopPieceIndex[column1], column1] != Grid[TopPieceIndex[column2], column2])
        {
            (Grid[TopPieceIndex[column2], column2], Grid[TopPieceIndex[column1], column1])
            = (Grid[TopPieceIndex[column1], column1], Grid[TopPieceIndex[column2], column2]);
            return true;
        }

        return false;
    }

    public bool Erase(char player, int column)
    {
        if (CheckHeight(column) && Grid[TopPieceIndex[column], column] != player)
        {
            Grid[TopPieceIndex[column], column] = ' ';
            TopPieceIndex[column]++;
            BallsCount--;

            return true;
        }

        return false;
    }

    public int Evaluate()
    {
        int countOS = MaxConnectedBalls(Constant.Computer);

        int countXS = MaxConnectedBalls(Constant.Human);

        if (IsWin(Constant.Computer)) return int.MaxValue;

        else if (IsWin(Constant.Human)) return int.MinValue;

        else if (IsDraw() || countOS == countXS) return 0;

        else if (countOS > countXS) return countOS;

        else if (countOS < countXS) return -countXS;

        return 0;
    }

    private int MaxConnectedBalls(char player)
    {
        int maxAll = 0;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (Grid[i, j] != player)
                    continue;

                int countVertically = Sequence(player, i, j, +1, 0, 0)
                        + Sequence(player, i, j, -1, 0, 0) - 1;

                int countHorizontally = Sequence(player, i, j, 0, +1, 0)
                        + Sequence(player, i, j, 0, -1, 0) - 1;

                int maxVerticallyHorizontally = Math.Max(countVertically, countHorizontally);

                int countDiagonalRight = Sequence(player, i, j, -1, +1, 0)
                        + Sequence(player, i, j, +1, -1, 0) - 1;

                int countDiagonalLeft = Sequence(player, i, j, -1, -1, 0)
                        + Sequence(player, i, j, +1, +1, 0) - 1;

                int maxDiagonallyHorizontally = Math.Max(countDiagonalRight, countDiagonalLeft);

                int maxSequence = Math.Max(maxVerticallyHorizontally, maxDiagonallyHorizontally);

                maxAll = Math.Max(maxAll, maxSequence);
            }
        }

        // Console.WriteLine(maxAll);
        return maxAll;
    }

    private int Sequence(char player, int currentRow, int currentColumn, int moveI, int moveJ, int counter)
    {
        if (CheckSides(currentRow, currentColumn) || Grid[currentRow, currentColumn] != player)
            return counter;

        return 1 + Sequence(player, currentRow + moveI, currentColumn + moveJ, moveI, moveJ, 0);
    }

    private bool CheckSides(int row, int column)
        => row < 0 || column < 0 || row >= Width || column >= Height;

    public bool IsDraw() => BallsCount == Width * Height || (IsWin(Constant.Human) && IsWin(Constant.Computer));

    public bool CheckWin(char player, int column)
        => IsWinVertical(player, column) || IsWinHorizontal(player, column)
            || IsWinInLeftDiagonal(player, column) || IsWinInRightDiagonal(player, column);

    public bool IsWin(char player)
    {
        for (int column = 0; column < Width; column++)
        {
            if (CheckWin(player, column))
                return true;
        }

        return false;
    }

    public bool IsFinal() => IsWin(Constant.Human) || IsWin(Constant.Computer) || IsDraw();

    private bool IsWinVertical(char player, int column)
    {
        int countVertically = Sequence(player, TopPieceIndex[column], column, +1, 0, 0)
                + Sequence(player, TopPieceIndex[column], column, -1, 0, 0) - 1;
        return countVertically == PiecesToWin;
    }

    private bool IsWinHorizontal(char player, int column)
    {
        int countHorizontally = Sequence(player, TopPieceIndex[column], column, 0, +1, 0)
                + Sequence(player, TopPieceIndex[column], column, 0, -1, 0) - 1;
        return countHorizontally == PiecesToWin;
    }

    private bool IsWinInLeftDiagonal(char player, int column)
    {
        int countDiagonalLeft = Sequence(player, TopPieceIndex[column], column, -1, -1, 0)
                + Sequence(player, TopPieceIndex[column], column, +1, +1, 0) - 1;
        return countDiagonalLeft == PiecesToWin;
    }

    private bool IsWinInRightDiagonal(char player, int column)
    {
        int countDiagonalRight = Sequence(player, TopPieceIndex[column], column, -1, +1, 0)
                + Sequence(player, TopPieceIndex[column], column, +1, -1, 0) - 1;
        return countDiagonalRight == PiecesToWin;
    }

    private bool CheckHeight(int column) => TopPieceIndex[column] != Height;

    public bool IsPlayedColumnValid(int column) => column > 0 && column - 1 < Width;

    private static char OtherPlayer(char player) => player == Constant.Human ? Constant.Computer : Constant.Human;

    public override string ToString()
    {
        string boardString = "";

        for (int i = 0; i < Height; i++)
        {
            boardString += " | ";

            for (int j = 0; j < Width; j++)
                boardString += $"{Grid[i, j]} | ";

            boardString += "\n";
        }

        boardString += "\n\n";
        for (int i = 1; i <= Width; i++)
            boardString += $" | {i}";

        boardString += " |";

        return boardString;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            HashCode hash = new();
            hash.Add(Width);
            hash.Add(Height);
            hash.Add(PiecesToWin);
            hash.Add(BallsCount);

            for (int i = 0; i < Height; i++)
                hash.Add(TopPieceIndex[i]);

            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    hash.Add(Grid[i, j]);

            return hash.ToHashCode();
        }
    }
}
