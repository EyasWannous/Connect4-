using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAConnectFour;

internal class Pair(int value)
{
    public int Value { get; set; } = value;
    public Board? Board { get; set; }

    public Pair(int value, Board board) : this(value) => Board = new(board);

    public Pair(Pair pair) : this(pair.Value)
    {
        if (pair.Board is not null)
            Board = pair.Board;
    }
}