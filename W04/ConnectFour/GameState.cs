
namespace ConnectFour;

public class GameState
{
    public enum WinState
    {
        No_Winner,
        Player1_Wins,
        Player2_Wins,
        Tie
    }

    private int[,] board = new int[6, 7];

    public int PlayerTurn { get; private set; } = 1;
    public int CurrentTurn { get; private set; }
    public WinState Result { get; private set; } = WinState.No_Winner;

    public int GetCell(int row, int column)
    {
        return board[row, column];
    }

    public bool IsColumnDisabled(int column)
    {
        return Result != WinState.No_Winner ||
               board[0, column] != 0;
    }

    public void ResetBoard()
    {
        board = new int[6, 7];
        PlayerTurn = 1;
        CurrentTurn = 0;
        Result = WinState.No_Winner;
    }

    public int PlayPiece(int column)
    {
        if (column < 0 || column >= 7)
            throw new ArgumentException("Invalid column.");

        if (Result != WinState.No_Winner)
            throw new ArgumentException("Game is over.");

        if (board[0, column] != 0)
            throw new ArgumentException("Column is full.");

        int player = PlayerTurn;

        for (int row = 5; row >= 0; row--)
        {
            if (board[row, column] != 0)
                continue;

            board[row, column] = player;
            CurrentTurn++;

            if (HasWon(row, column, player))
            {
                Result = player == 1
                    ? WinState.Player1_Wins
                    : WinState.Player2_Wins;
            }
            else if (CurrentTurn == 42)
            {
                Result = WinState.Tie;
            }
            else
            {
                PlayerTurn = player == 1 ? 2 : 1;
            }

            return row;
        }

        throw new ArgumentException("Column is full.");
    }

    public WinState CheckForWin()
    {
        return Result;
    }

    private bool HasWon(int row, int column, int player)
    {
        int[,] directions =
        {
            { 0, 1 },
            { 1, 0 },
            { 1, 1 },
            { 1, -1 }
        };

        for (int i = 0; i < 4; i++)
        {
            int dr = directions[i, 0];
            int dc = directions[i, 1];

            int count = 1;

            count += CountPieces(row, column, dr, dc, player);
            count += CountPieces(row, column, -dr, -dc, player);

            if (count >= 4)
                return true;
        }

        return false;
    }

    private int CountPieces(
        int row, int column, int dr, int dc, int player)
    {
        int count = 0;

        row += dr;
        column += dc;

        while (row >= 0 && row < 6 &&
               column >= 0 && column < 7 &&
               board[row, column] == player)
        {
            count++;
            row += dr;
            column += dc;
        }

        return count;
    }
}
