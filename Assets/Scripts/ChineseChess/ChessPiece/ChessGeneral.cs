using UnityEngine;

namespace ChineseChess
{
    public class ChessGeneral:ChessPiece
    {
        public ChessGeneral(ChessData data) : base(data)
        {
            
        }

        protected override void MoveTo(ChessData data)
        {
            if (Mathf.Abs(Data.X - data.X) <= 1 && Mathf.Abs(Data.Y - data.Y) <= 1 && data.X <= 5 && data.X >= 3 &&
                ((data.Y >= 0 && data.Y <= 2) || (data.Y <= 9 && data.Y >= 7)))
            {
                base.MoveTo(data);
            }
            else
            {
                Debug.LogError("general move wrong!");
            }
        }
    }
}