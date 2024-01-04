using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAConnectFour;

internal class MinMax
{
    // Constant.Computer is MAX
    public static List<object> MaxMove(Board current, int depth, int alpha, int beta)
    {
        List<object> result = [];
        if (current.IsFinal() || depth <= 0)
        {
            result = [current.Evaluate(), current];

            return result;
        }

        int maxMove = int.MinValue;
        int index = 0;

        List<Board> list = current.AllNextMoves(Constant.Computer);
        for (int i = 0; i < list.Count; i++)
        {
            Board move = list[i];
            var eval = MinMove(move, depth - 1, alpha, beta);

            if (maxMove < (int)eval[0])
            {
                maxMove = (int)eval[0];
                index = i;
            }

            alpha = Math.Max(maxMove, alpha);
            if (beta <= alpha)
                break;
        }

        result.Add(maxMove);
        result.Add(list[index]);
        return result;
    }

    // Constant.Human is MIN
    public static List<object> MinMove(Board current, int depth, int alpha, int beta)
    {
        List<object> result = [];

        if (current.IsFinal() || depth <= 0)
        {
            result = [current.Evaluate(), current];

            return result;
        }

        int minMove = int.MaxValue;
        int index = 0;

        List<Board> list = current.AllNextMoves(Constant.Human);
        for (int i = 0; i < list.Count; i++)
        {
            Board move = list[i];
            var eval = MaxMove(move, depth - 1, alpha, beta);

            if (minMove > (int)eval[0])
            {
                minMove = (int)eval[0];
                index = i;
            }

            beta = Math.Min(minMove, beta);
            if (beta <= alpha)
                break;
        }

        result.Add(minMove);
        result.Add(list[index]);
        return result;
    }

    // Constant.Computer is Maximizer
    public static Pair MiniMax(Board currentBoard, int depth, bool maximizingPlayer, int alpha, int beta)
    {
        if (currentBoard.IsFinal() || depth <= 0)
            return new(currentBoard.Evaluate(), currentBoard);

        if (maximizingPlayer)
        {
            List<Board> moves = currentBoard.AllNextMoves(Constant.Computer);
            Pair maxMove = new(int.MinValue);

            foreach (var move in moves)
            {
                Pair eval = MiniMax(move, depth - 1, false, alpha, beta);

                if (maxMove.Value < eval.Value)
                    maxMove = new(eval);

                alpha = Math.Max(maxMove.Value, alpha);
                if (beta <= alpha)
                    break;
            }

            return maxMove;
        }

        else
        {
            List<Board> moves = currentBoard.AllNextMoves(Constant.Human);
            Pair minMove = new(int.MaxValue);

            foreach (var move in moves)
            {
                Pair eval = MiniMax(move, depth - 1, true, alpha, beta);

                if (minMove.Value > eval.Value)
                    minMove = new(eval);

                beta = Math.Min(minMove.Value, beta);
                if (beta <= alpha)
                    break;
            }

            return minMove;
        }
    }
}
