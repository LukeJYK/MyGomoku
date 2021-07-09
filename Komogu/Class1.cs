using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGomoku
{
    public enum ChessState
    {
        BLACK,
        WHITE,
        BLANK
    }
    public enum ChessResult
    {
        BLACK,
        WHITE,
        DRAW,
        CONTINUE
    }

    public abstract class BaseGomo
    {
        public ChessState[,] chessSate 
            = new ChessState[15, 15];
        protected int total;
        protected Point currentPoint;
        protected bool start;
        protected bool bOrW;
        protected List<Point> listPoint = new List<Point>();

        public BaseGomo()
        {
            for(int i=0;i<15;i++)
            {
                for(int j=0;j<15;j++)
                {
                    chessSate[i, j] = ChessState.BLANK;
                }
            }
            start = false;
            total = 0;
            bOrW = true;
        }
        public abstract void NewChess();
        public abstract ChessResult Judge();
        public abstract void StartCheck();
        public abstract void EndChess();

    }

    public class Gomo : BaseGomo
    {
        public override void NewChess()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    chessSate[i, j] = ChessState.BLANK;
                }
            }
            start =true;
            total = 0;
            bOrW = true;
        }
        private bool PutDown()
        {
            if(chessSate[currentPoint.X,currentPoint.Y]
                !=ChessState.BLANK)
                return false;
            
            if (total < 15 * 15)
                total++;
            else
                return false;

            if(bOrW)
            {
                chessSate[currentPoint.X, currentPoint.Y] = ChessState.BLACK;
                bOrW = false;
            }
            else
            {
                chessSate[currentPoint.X, currentPoint.Y] 
                    = ChessState.WHITE;
                bOrW = true;
            }
            return true;
        }
        public override ChessResult Judge()
        {
            if (total == 15 * 15)
                return ChessResult.DRAW;

            if(PutDown())
            {
                //for(int i=currentPoint.X; )
            }
            return ChessResult.CONTINUE;
        }
        public Point CurrentPoint
        {
            get
            {
                return currentPoint;
            }
            set
            {
                currentPoint = value;
            }
        }
        public override void StartCheck()
        {
            throw new NotImplementedException();
        }
        public override void EndChess()
        {
            throw new NotImplementedException();
        }
    }
}
