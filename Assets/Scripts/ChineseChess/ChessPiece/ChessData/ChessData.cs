namespace ChineseChess
{
    public enum ChessState
    {
        CanMove,
        SelfChoose,
        AnotherChoose,
        CanNotMove,
    }
    public class ChessData
    {
        public ChessPieceType Type;
        public ChessState State;
        public int Side;
        public int X;
        public int Y;

        public void SetData(ChessPieceType type,ChessState state, int side, int x, int y)
        {
            Type = type;
            State = state;
            Side = side;
            X = x;
            Y = y;
        }
    }
}