using Utility;

namespace ChineseChess
{
    public class ChineseChessBoard : MonoBehaviourSingle<ChineseChessBoard>
    {
        private ChessPiece[,] _chessBoard;  //存储棋子信息
        private int[,] _board;  //存储位置信息

        private ChineseChessBoard()
        {
            InitChessBoard();
            InitBoard();
        }

        private void InitChessBoard()
        {
            _chessBoard = new ChessPiece[9, 10]; 
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _chessBoard[i, j] = null;   // 初始时没有棋子
                }
            }
        }

        private void InitBoard()
        {
            _board = new int[9, 10]; 
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _board[i, j] = 0;   // 初始时没有棋子
                }
            }
        }

        public void SetChessPiece(int x, int y, ChessPiece piece)
        {
            _chessBoard[x, y] = piece;
        }

        public ChessPiece GetChessPiece(int x, int y)
        {
            return _chessBoard[x, y];
        }
    }
}