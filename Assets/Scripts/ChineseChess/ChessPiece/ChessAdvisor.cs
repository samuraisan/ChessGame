using UnityEngine;

namespace ChineseChess
{
    public class ChessAdvisor:ChessPiece
    {
        public ChessAdvisor(ChessData data) : base(data)
        {
            
        }

        protected override void MoveTo(ChessData data)
        {
            if(Mathf.Abs(Data.Y-data.Y) <= 1)
                base.MoveTo(data);
        }
    }
}