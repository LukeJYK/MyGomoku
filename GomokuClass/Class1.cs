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
        BLANK,
        BLACK,
        WHITE
    }
    public enum Result
    {
        BLACKWIN,
        WHITEWIN,
        CONTINUE,
        DRAW
    }

    [Serializable]
    public abstract class Gomoku
    {
        public ChessState[,] chessState = new
            ChessState[15, 15];
        //记录权重的表
        public int[,] weightChessState = new
         int[15, 15];
        //人人对战开关
        public bool Start { get; set; }
        protected List<Point> list = new List<Point>();
        public abstract Result Judge();
        public int Count { get; set; }
        //悔棋次数限制
        public int NumCount { get; set; }
        //最多悔棋步数
        public int MaxRegret {get; set;}
        //悔棋计数器
        public int RegretTimes { get; set; }
        //回看计数
        public int ReviewNumber { get; set; }
        //回放开关
        public bool ReviewTurner { get; set; }
        //机器学习系数
        public int Ratio { get; set; }
        public int Ratio2 { get; set; }
        //人机对战开关
        public bool StartMarshine { get; set; }
        //测试点
        public  Point Test1 { get; set; }
        public Point Test2 { get; set; }
       
        public Point Current{get;set;}
        public ChessState CurrentChess{get;set;}
        public abstract void Down();
        public abstract void New();
        public abstract bool GiveUp();
    }

    [Serializable]
    public class GomokuOK : Gomoku
    {
        public GomokuOK()
        {
            New();
        }
        public override void New()
        {
            for(int i=0;i<15;i++)
            {
                for(int j=0;j<15;j++)
                {
                    chessState[i, j] = ChessState.BLANK;
                }
            }
            Count = 0;
            //机器学习系数*************************************************************************
            Ratio = 2;
            Ratio2 = 1;
            //最多悔棋步数
            if(MaxRegret==0)
            MaxRegret = 2;
            //悔棋计数器
            RegretTimes = 0;
            Start = false;
            ReviewTurner = false;
            //人机对战开关初值
            StartMarshine = false;
            //机器下子开关
           
            CurrentChess = ChessState.BLACK;
            ReviewNumber = 0;
        }
        public ChessState Get(Point p)
        {
            return chessState[p.X, p.Y];
        }
        public override void Down()
        {
            if(chessState[Current.X,Current.Y]==ChessState.BLANK)
            {
                //悔棋列表
                list.Insert(Count, new Point(Current.X, Current.Y));
                if(CurrentChess==ChessState.BLACK)
                {
                    chessState[Current.X, Current.Y] = ChessState.BLACK;
                    CurrentChess = ChessState.WHITE;
                }
                else if (CurrentChess == ChessState.WHITE)
                {
                    chessState[Current.X, Current.Y] = ChessState.WHITE;
                    CurrentChess = ChessState.BLACK;
                }

                Count++;
                RegretTimes = 0;
                
                

            }
        }
       
        public override Result Judge()
        {
            if(diaLeftJudge()==Result.BLACKWIN|diaLeftJudge()==Result.WHITEWIN)
                return diaLeftJudge();
            if (listJudge() == Result.BLACKWIN | listJudge() == Result.WHITEWIN)
                return listJudge();
            if (rowJudge() == Result.BLACKWIN | rowJudge() == Result.WHITEWIN)
                return rowJudge();
            if (diaRightJudge() == Result.BLACKWIN | diaRightJudge() == Result.WHITEWIN)
                return diaRightJudge();
            return Result.CONTINUE;
        }

        public Result rowJudge()
        {
            int total = 1;
            int i = 0;

            for (i = Current.X - 1; i >= 0; i--)
            {
                if (Current.X < 0)
                    break;
                if (chessState[Current.X, Current.Y] == chessState[i, Current.Y])
                    total++;
                else
                    break;
            }
            for (i = Current.X + 1; i < 15; i++)
            {
                if (Current.X > 15)
                    break;
                if (chessState[Current.X, Current.Y] == chessState[i, Current.Y])
                    total++;
                else
                    break;
            }
            if (total >= 5)
            {
                if (CurrentChess == ChessState.WHITE)
                    return Result.BLACKWIN;
                if (CurrentChess == ChessState.BLACK)
                    return Result.WHITEWIN;
            }
          
                return Result.CONTINUE;
        }

        public Result listJudge()
        {
            int total = 1;
            int i = 0;

            for (i = Current.Y - 1; i >= 0; i--)
            {
                if (Current.Y < 0)
                    break;
                if (chessState[Current.X, Current.Y] == chessState[Current.X, i])
                    total++;
                else
                    break;
            }
            for (i = Current.Y + 1; i < 15; i++)
            {
                if (Current.Y > 15)
                    break;
                if (chessState[Current.X, Current.Y] == chessState[Current.X, i])
                    total++;
                else
                    break;
            }
            if (total >= 5)
            {
                if (CurrentChess == ChessState.WHITE)
                    return Result.BLACKWIN;
                if (CurrentChess == ChessState.BLACK)
                    return Result.WHITEWIN;
            }

            return Result.CONTINUE;
        }

        public Result diaLeftJudge()
        {
            int total = 1;
            int i = 0;
            int j = 0;
            j = Current.Y - 1;
            for(i=Current.X+1;i<15 && j>0;i++)
            {
                if (Current.X > 15| Current.Y<0)
                    break;
                if (chessState[Current.X, Current.Y] == chessState[i,j])
                    total++;
                else
                    break;
                j--;
            }
            j = Current.Y + 1;
            for (i = Current.X -1; j < 15 && i > 0; i--)
            {
                if (Current.X < 0|Current.Y>15)
                    break;
                if (chessState[Current.X, Current.Y] == chessState[i, j])
                    total++;
                else
                    break;
                j++;
            }
            if (total >= 5)
            {
                if (CurrentChess == ChessState.WHITE)
                    return Result.BLACKWIN;
                if (CurrentChess == ChessState.BLACK)
                    return Result.WHITEWIN;
            }

            return Result.CONTINUE;
        }

        public Result diaRightJudge()
        {
            int total = 1;
            int i = 0;
            int j = 0;
            j = Current.Y + 1;
            for (i = Current.X + 1; i < 15 && j<15; i++)
            {
                if (Current.X > 15|Current.Y>15)
                    break;
                if (chessState[Current.X, Current.Y] == chessState[i, j])
                    total++;
                else
                    break;
                j++;
            }
            j = Current.Y -1;
            for (i = Current.X - 1; j >0 && i > 0; i--)
            {
                if (Current.X < 0| Current.Y<0)
                    break;
                if (chessState[Current.X, Current.Y] == chessState[i, j])
                    total++;
                else
                    break;
                j--;
            }
            if (total >= 5)
            {
                if (CurrentChess == ChessState.WHITE)
                    return Result.BLACKWIN;
                if (CurrentChess == ChessState.BLACK)
                    return Result.WHITEWIN;
            }

            return Result.CONTINUE;
        }

        
        //悔棋函数
        public void Regret()
        {
            if (Count > 1 && RegretTimes<MaxRegret )
            {
                chessState[list[Count - 1].X, list[Count - 1].Y] = ChessState.BLANK;
                Count--;
                Current = list[Count - 1];
                if (CurrentChess == ChessState.BLACK)
                    CurrentChess = ChessState.WHITE;
                else if (CurrentChess == ChessState.WHITE)
                    CurrentChess = ChessState.BLACK;
                RegretTimes++;
                
            }
        }

        public void RightReview()
        {
            if(ReviewTurner==true && ReviewNumber<Count)
            {
                chessState[list[ReviewNumber].X, list[ReviewNumber].Y] = CurrentChess;
                ReviewNumber++;
            }
            if (CurrentChess == ChessState.BLACK)
                CurrentChess = ChessState.WHITE;
            else if (CurrentChess == ChessState.WHITE)
                CurrentChess = ChessState.BLACK;
        }

        public void LeftReview()
        {
            if(ReviewTurner==true && ReviewNumber>0)
            {
                chessState[list[ReviewNumber-1].X, list[ReviewNumber-1].Y] = ChessState.BLANK;
                ReviewNumber--;
                if (CurrentChess == ChessState.BLACK)
                    CurrentChess = ChessState.WHITE;
                else if (CurrentChess == ChessState.WHITE)
                    CurrentChess = ChessState.BLACK;
            }
        }
        public override bool GiveUp()
        {
           
            if (Count % 2 == 0)
                return true;
            else
                return false;

        }//认输


        public void New2()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    chessState[i, j] = ChessState.BLANK;
                }
            }
          
            Start = false;
            ReviewTurner = true;
            CurrentChess = ChessState.BLACK;
        }

        //黑棋权重判断函数
        public int AveBlackWinWeight(Point p)
        {
            int weight = 0;
            int compare1 = Math.Max(AveBlackWinWeightForDiaLeft(p), AveBlackWinWeightForDiaRight(p));
            int compare2 = Math.Max(AveBlackWinWeightForList(p), AveBlackWinWeightForRow(p));
            weight = Math.Max(compare1, compare2);
            return weight;
        }

        public int AveBlackWinWeightForRow(Point p)
        {

            int WinWeight = 1;
            int WinCount = 0;
            for (int i = 1; i < 5 && p.Y + i < 15; i++)
            {
                if (chessState[p.X, p.Y + i] == ChessState.WHITE)
                {
                    break;
                }
                if (chessState[p.X, p.Y + i] == ChessState.BLACK)
                    WinWeight++;
                WinCount++;
            }
            for (int i = 1; i < 5 && p.Y - i >= 0; i++)
            {
                if (chessState[p.X, p.Y - i] == ChessState.WHITE)
                {
                    break;
                }
                if (chessState[p.X, p.Y - i] == ChessState.BLACK)
                    WinWeight++;
                WinCount++;
            }
            if (WinCount < 4)
                return 0;
            return WinWeight;

        }

        public int AveBlackWinWeightForList(Point p)
        {

            int WinWeight = 1;
            int WinCount = 0;
            for (int i = 1; i < 5 && p.X + i < 15; i++)
            {
                if (chessState[p.X+i, p.Y ] == ChessState.WHITE)
                {
                    break;
                }
                if (chessState[p.X+i, p.Y ] == ChessState.BLACK)
                    WinWeight++;
                WinCount++;
            }
            for (int i = 1; i < 5 && p.X - i >= 0; i++)
            {
                if (chessState[p.X-i, p.Y] == ChessState.WHITE)
                {
                    break;
                }
                if (chessState[p.X-i, p.Y] == ChessState.BLACK)
                    WinWeight++;
                WinCount++;
            }
            if (WinCount < 4)
                return 0;
            return WinWeight;

        }

        public int AveBlackWinWeightForDiaLeft(Point p)
        {

            int WinWeight = 1;
            int WinCount = 0;
            for (int i = 1; i < 5 && p.X + i < 15&& p.Y +i<15; i++)
            {
                if (chessState[p.X + i, p.Y+i] == ChessState.WHITE)
                {
                    break;
                }
                if (chessState[p.X + i, p.Y+i] == ChessState.BLACK)
                    WinWeight++;
                WinCount++;
            }
            for (int i = 1; i < 5 && p.X - i >=0 &&p.Y-i >= 0; i++)
            {
                if (chessState[p.X - i, p.Y-i] == ChessState.WHITE)
                {
                    break;
                }
                if (chessState[p.X - i, p.Y-i] == ChessState.BLACK)
                    WinWeight++;
                WinCount++;
            }
            if (WinCount < 4)
                return 0;
            return WinWeight;

        }

        public int AveBlackWinWeightForDiaRight(Point p)
        {

            int WinWeight = 1;
            int WinCount = 0;
            for (int i = 1; i < 5 && p.X + i < 15 && p.Y -i>0; i++)
            {
                if (chessState[p.X + i, p.Y-i] == ChessState.WHITE)
                {
                    break;
                }
                if (chessState[p.X + i, p.Y-i] == ChessState.BLACK)
                    WinWeight++;
                WinCount++;
            }
            for (int i = 1; i < 5 && p.X - i >= 0 && p.Y +i <15; i++)
            {
                if (chessState[p.X - i, p.Y +i] == ChessState.WHITE)
                {
                    break;
                }
                if (chessState[p.X - i, p.Y +i] == ChessState.BLACK)
                    WinWeight++;
                WinCount++;
            }
            if (WinCount < 4)
                return 0;
            return WinWeight;

        }

        //白棋权重判断函数
        public int AveWhiteWinWeight(Point p)
        {
            int weight = 0;
            int compare1 = Math.Max(AveWhiteWinWeightForDiaLeft(p), AveWhiteWinWeightForDiaRight(p));
            int compare2 = Math.Max(AveWhiteWinWeightForList(p), AveWhiteWinWeightForRow(p));
            weight = Math.Max(compare1, compare2);
            return weight;
        }

        public int AveWhiteWinWeightForRow(Point p)
        {

            int WinWeight = 1;
            int WinCount = 0;
            for (int i = 1; i < 5 && p.Y + i < 15; i++)
            {
                if (chessState[p.X, p.Y + i] == ChessState.BLACK)
                {
                    break;
                }
                if (chessState[p.X, p.Y + i] == ChessState.WHITE)
                    WinWeight++;
                WinCount++;
            }
            for (int i = 1; i < 5 && p.Y - i >= 0; i++)
            {
                if (chessState[p.X, p.Y - i] == ChessState.BLACK)
                {
                    break;
                }
                if (chessState[p.X, p.Y - i] == ChessState.WHITE)
                    WinWeight++;
                WinCount++;
            }
            if (WinCount < 4)
                return 0;
            return WinWeight;

        }

        public int AveWhiteWinWeightForList(Point p)
        {

            int WinWeight = 1;
            int WinCount = 0;
            for (int i = 1; i < 5 && p.X + i < 15; i++)
            {
                if (chessState[p.X + i, p.Y] == ChessState.BLACK)
                {
                    break;
                }
                if (chessState[p.X + i, p.Y] == ChessState.WHITE)
                    WinWeight++;
                WinCount++;
            }
            for (int i = 1; i < 5 && p.X - i >= 0; i++)
            {
                if (chessState[p.X - i, p.Y] == ChessState.BLACK)
                {
                    break;
                }
                if (chessState[p.X - i, p.Y] == ChessState.WHITE)
                    WinWeight++;
                WinCount++;
            }
            if (WinCount < 4)
                return 0;
            return WinWeight;

        }

        public int AveWhiteWinWeightForDiaLeft(Point p)
        {

            int WinWeight = 1;
            int WinCount = 0;
            for (int i = 1; i < 5 && p.X + i < 15 && p.Y + i < 15; i++)
            {
                if (chessState[p.X + i, p.Y + i] == ChessState.BLACK)
                {
                    break;
                }
                if (chessState[p.X + i, p.Y + i] == ChessState.WHITE)
                    WinWeight++;
                WinCount++;
            }
            for (int i = 1; i < 5 && p.X - i >= 0 && p.Y - i >= 0; i++)
            {
                if (chessState[p.X - i, p.Y - i] == ChessState.BLACK)
                {
                    break;
                }
                if (chessState[p.X - i, p.Y - i] == ChessState.WHITE)
                    WinWeight++;
                WinCount++;
            }
            if (WinCount < 4)
                return 0;
            return WinWeight;

        }

        public int AveWhiteWinWeightForDiaRight(Point p)
        {

            int WinWeight = 1;
            int WinCount = 0;
            for (int i = 1; i < 5 && p.X + i < 15 && p.Y - i > 0; i++)
            {
                if (chessState[p.X + i, p.Y - i] == ChessState.BLACK)
                {
                    break;
                }
                if (chessState[p.X + i, p.Y - i] == ChessState.WHITE)
                    WinWeight++;
                WinCount++;
            }
            for (int i = 1; i < 5 && p.X - i >= 0 && p.Y + i < 15; i++)
            {
                if (chessState[p.X - i, p.Y + i] == ChessState.BLACK)
                {
                    break;
                }
                if (chessState[p.X - i, p.Y + i] == ChessState.WHITE)
                    WinWeight++;
                WinCount++;
            }
            if (WinCount < 4)
                return 0;
            return WinWeight;

        }

        public int WinWeight(Point p)
        {
            int weightsum =Ratio*AveBlackWinWeight(p) + Ratio2*AveWhiteWinWeight(p);
            return weightsum;
        }

        public void WeightValueAll()
        {
           for(int i=0;i<15;i++)
            {
                for(int j=0;j<15;j++)
                {
                    if (chessState[i, j] == ChessState.BLANK)
                        weightChessState[i, j] = WinWeight(new Point(i, j));
                    else
                        weightChessState[i, j] = -1;
                }
            }
        }

 

        public Point Selection(Point p)
        {
            WeightValueAll();
            Point bestChoice = new Point(0, 0);
            bestChoice = p;
            int numcount = 0;

            for (int i = p.X - 1; p.X >= 1 && i < p.X + 1 && p.X < 14; i++)
            {
                for (int j = p.Y - 1; j < p.Y + 1 && p.Y >= 1 && p.Y < 14; j++)
                {
                    if (chessState[i, j] == ChessState.BLANK)
                    {
                        if (weightChessState[i, j] > numcount)
                        {
                            bestChoice = new Point(i, j);
                            numcount = weightChessState[i, j];
                        }
                    }
                }
            }
            //
            //Test1 = new Point(bestChoice.X ,bestChoice.Y);


            for (int i = p.X-3;p.X >=3 && i < p.X+3 && p.X <12; i++)
            {
                for (int j = p.Y -3; j < p.Y +3 && p.Y>=3 && p.Y <12 ; j++)
                {
                    if (chessState[i, j] == ChessState.BLANK)
                    {
                        if (weightChessState[i, j] > weightChessState[bestChoice.X, bestChoice.Y])
                            bestChoice = new Point(i, j);
                    }
                }
            }
            //
            //Test2 = new Point(bestChoice.X, bestChoice.Y);

            for (int i=0;i<15;i++)
            {
                for(int j=0;j<15;j++)
                {
                    if (chessState[i, j] == ChessState.BLANK)
                    {
                        if (weightChessState[i, j] > weightChessState[bestChoice.X, bestChoice.Y])
                            bestChoice = new Point(i, j);
                    }
                }
            }
            return bestChoice;
        }


    }

    
}
