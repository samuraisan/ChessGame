using System;
using Unity.VisualScripting;
using UnityEngine;
using Utility.Event;

namespace ChineseChess
{
    /// <summary>
    /// 0是空棋子,1是统帅，2是仕，3是相，4是马，5是车，6是炮，7是兵
    /// </summary>
    public enum ChessPieceType
    {
        None = 0,
        General = 1,
        Advisor = 2,
        Elephant = 3,
        Horse = 4,
        Chariot = 5,
        Cannon = 6,
        Soldier = 7,
    }
    /// <summary>
    /// 棋子信息
    /// </summary>
    public class ChessPiece:MonoBehaviour
    {
        protected ChessData Data;
        
        protected ChessPiece(ChessData data)
        {
            Data = data;
            EventManager.instance.RegisterEvent(EventTable.Move,Move);
            EventManager.instance.RegisterEvent(EventTable.MoveOver,MoveOver);
            EventManager.instance.RegisterEvent(EventTable.ChangeChessState,ChooseThis);
        }

        /// <summary>
        /// 移动棋子
        /// </summary>
        /// <param name="data"></param>
        protected virtual void MoveTo(ChessData data)
        {
            if (data.Side != Data.Side || Data.X != data.X || Data.Y != data.Y)
            {
                Data = data;
                EventManager.instance.SendApplication(EventTable.Move,new object[]{data.Side});
            }
            else
            {
                Debug.LogError("have chess now!");
            }
        }

        private void Move(object[] param)
        {
            EventManager.instance.SendApplication(EventTable.MoveOver,new object[]{Data.Side});
        }

        private void MoveOver(object[] param)
        {
            int side = (int)param[0];
            ChangeState(Data.Side == side ? ChessState.CanNotMove : ChessState.CanMove);
        }

        private void ChooseThis(object[] param)
        {
            ChessData data = param[0] as ChessData;
            Data = data;
            EventManager.instance.SendApplication(EventTable.ChangeChessState,new object[]{ChessState.SelfChoose});
        }

        private void ChangeState(ChessState state)
        {
            Data.State = state;
        }
    }
}