using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAConnectFour;

internal class Test
{

    public Test()
    {
        Board board = new(4, 4, 3);
        Console.WriteLine("Hello, World!");

        board.Play('o', 1);
        board.Play('x', 1);
        board.Play('o', 2);
        board.Play('x', 2);
        board.Play('x', 3);
        board.Play('x', 3);

        Console.WriteLine("board:");
        Console.WriteLine(board);
        Console.WriteLine("****************");
        Console.WriteLine("is win for x? " + board.IsWin('x'));
        Console.WriteLine("****************");

        List<Board> next = board.AllNextMoves('x');
        int i = 1;

        foreach (var b in next)
        {
            Console.WriteLine(i + ": (" + b.Evaluate() + ")");
            Console.WriteLine(b);
            Console.WriteLine("is win for x? ");
            i++;
        }
    }
}