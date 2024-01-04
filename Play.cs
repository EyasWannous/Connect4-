using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAConnectFour;

internal class Play(Board playBoard)
{
    public Board PlayBoard { get; set; } = playBoard;

    public void Start()
    {
        Console.WriteLine(PlayBoard);

        while (true)
        {
            HumanPlay();
            Console.WriteLine(PlayBoard);

            if (Draw())
                break;

            if (Wins(Constant.Human))
                break;

            ComputerPlay();
            Write.ComputerTurn();
            Console.WriteLine(PlayBoard);

            if (Draw())
                break;

            if (Wins(Constant.Computer))
                break;
        }
    }

    private void ComputerPlay()
    {
        int alpha = int.MinValue;
        int beta = int.MaxValue;
        int depth = 7;

        // PlayBoard = MinMax.MiniMax(PlayBoard, depth, true, alpha, beta).Board;

        List<object> temp = MinMax.MaxMove(PlayBoard, depth, alpha, beta);
        PlayBoard = (Board)temp[1];
    }

    private bool HumanPlay()
    {
        int column;
        string move;

        Write.EnterColumn();
        string? key = Console.ReadLine();

        while (key is null
            || !int.TryParse(key, out column)
            || !PlayBoard.IsPlayedColumnValid(column))
        {
            Console.WriteLine();
            Write.InvalidColumn();
            Write.EnterColumn();
            key = Console.ReadLine();
        }

        Write.ChooseMove();
        string? key1 = Console.ReadLine();

        while (key1 is null
            || !CheckMove(key1.ToLower()))
        {
            Console.WriteLine();
            Write.InvalidMove();
            Write.ChooseMove();
            key1 = Console.ReadLine();
        }

        move = key1.ToLower();
        Console.WriteLine();

        return move switch
        {
            "a" => Add(column),
            "d" => Delete(column),
            "r" => Repalce(column),
            _ => throw new NotImplementedException(),
        };
    }

    private bool Add(int column)
    {
        if (PlayBoard.IsPlayedColumnValid(column))
        {
            if (PlayBoard.Play(Constant.Human, column - 1))
                return true;

            else
                Write.InvalidColumnInAdd(column);
        }

        else
            Write.InvalidRange(PlayBoard.Width);

        return false;
    }

    private bool Delete(int column)
    {
        if (PlayBoard.IsPlayedColumnValid(column))
        {
            if (PlayBoard.Erase(Constant.Human, column - 1))
                return true;

            else
                Write.InvalidColumnInDelete(column);
        }

        else
            Write.InvalidRange(PlayBoard.Width);

        return false;
    }

    private bool Swap(int column, int replacedColumn)
    {
        if (PlayBoard.IsPlayedColumnValid(column) && PlayBoard.IsPlayedColumnValid(replacedColumn))
        {
            if (PlayBoard.Swap(column - 1, replacedColumn - 1))
                return true;

            else
                Write.InvalidColumnInReplace(column, replacedColumn);
        }

        else
            Write.InvalidRange(PlayBoard.Width);

        return false;
    }

    private bool Repalce(int column)
    {
        int replacedColumn;
        Write.ChooseColumn();
        string? key = Console.ReadLine();

        while (key is null
            || !int.TryParse(key, out replacedColumn)
            || !PlayBoard.IsPlayedColumnValid(replacedColumn))
        {
            Console.WriteLine();
            Write.InvalidColumn();
            Write.ChooseColumn();
            key = Console.ReadLine();
        }
        Console.WriteLine();

        Swap(column, replacedColumn);
        return true;
    }

    private bool CheckMove(string key) => key == "a" || key == "d" || key == "r";

    private bool Draw()
    {
        if (PlayBoard.IsDraw())
        {
            Write.Draw();
            return true;
        }

        return false;
    }

    private bool Wins(char winner)
    {
        if (PlayBoard.IsWin(winner))
        {
            Write.Wins(winner == Constant.Computer ? "Computer" : "Human");
            return true;
        }

        return false;
    }
}
