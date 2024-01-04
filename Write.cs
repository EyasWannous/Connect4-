using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAConnectFour;

internal static class Write
{
    public static void InvalidRange(int width) => Console.WriteLine($"Invalid Column: out of range {width} try again");

    public static void InvalidColumnInAdd(int column) => Console.WriteLine($"Invalid Column: Column {column} is full!, try again");

    public static void InvalidColumnInDelete(int column) => Console.WriteLine($"Invalid Column: Column {column} is empty or not valid !, try again");

    public static void InvalidColumnInReplace(int column, int replacedColumn)
        => Console.WriteLine($"Invalid Column: Column {column}  or Invalid Replaced Column: Column {replacedColumn} is empty or not valid !, try again");

    public static void ChooseColumn() => Console.Write("Choose the column you want to replace with it ");

    public static void EnterColumn() => Console.Write("Enter column: ");

    public static void ChooseMove() => Console.Write("Choose move (a, d or r): ");

    public static void InvalidColumn() => Console.WriteLine("Invalid column please enter valid value of column");

    public static void InvalidMove() => Console.WriteLine("Invalid Move");

    public static void Draw() => Console.WriteLine("Draw");

    public static void Wins(string winner) => Console.WriteLine($"{winner} Wins");

    public static void ComputerTurn() => Console.WriteLine("_____Computer Turn_____");
}