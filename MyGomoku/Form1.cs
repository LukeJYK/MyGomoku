using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Media;
namespace MyGomoku
{
    public partial class Form1 : Form
    {
        GomokuOK gomo = new GomokuOK();
        int x0 = 50;
        int y0 = 50;
        int inter = 30;
        int r = 20;
        int timenum1 = 0;
        int timenum2 = 0;
        int yellowbutton = 1;
        int bluebutton = 0;
        int greenbutton = 0;
        public Form1()
        {
            this.DoubleBuffered = true;
            InitializeComponent();

            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
            Graphics g = e.Graphics;
            SolidBrush blueSB = new SolidBrush(Color.LightBlue);
            SolidBrush whiteSB = new SolidBrush(Color.White);
            SolidBrush blackSB = new SolidBrush(Color.Black);
            
            Pen blackPen=new Pen(Color.Black);
            if (bluebutton == 1)
            {
                Image ChessBoard = Image.FromFile("BackGround.png");
              
            }

            if (greenbutton == 1)
            {
                Image ChessBoard = Image.FromFile("BackGround.png");

            }
            Image Backgroud = Image.FromFile("BackGround.png");
            g.DrawImage(Backgroud, 0, 0, 720, 550);
            Bitmap photoBitmap = new Bitmap(".\\ChessBoard.png");
            if (yellowbutton == 1)
            {
                g.FillRectangle(whiteSB,
                new Rectangle(x0, y0, 14 * inter, 14 * inter));
                blueSB.Color = Color.White;
                Rectangle formRect = new Rectangle(x0, y0, 14 * inter, 14 * inter);
                Rectangle imageRect = new Rectangle(x0, y0, 14 * inter, 14 * inter);
                g.DrawImage(photoBitmap, formRect, imageRect, System.Drawing.GraphicsUnit.Pixel);
                
            }
            //其他颜色
            if (yellowbutton == 0)
            {
                g.FillRectangle(blueSB,
                    new Rectangle(x0, y0, 14 * inter, 14 * inter));
            }
         
            SolidBrush s = new SolidBrush(Color.Black);
            g.FillEllipse(s, 255,255, 10, 10);

            for (int i=0;i<15;i++)
            {
                g.DrawLine(blackPen, x0, y0 + i * inter, x0 + 14 * inter, y0 + i * inter);
            }
            for (int i = 0; i < 15; i++)
            {
                g.DrawLine(blackPen, x0+i*inter, y0 , x0 + i* inter, y0 +14 * inter);
            }

            for (int i=0;i<15;i++)
            {
                for(int j=0;j<15;j++)
                {
                    if(gomo.Get(new Point(i,j))==ChessState.BLACK)
                    {
                        Point p2 = LogicToView(new Point(i, j));
                        Image Chess = Image.FromFile("BlackChess.png");
                        g.DrawImage(Chess, p2.X-r/2, p2.Y-r/2, r, r);
                                
                    }
                    if (gomo.Get(new Point(i, j)) == ChessState.WHITE)
                    {
                        Point p2 = LogicToView(new Point(i, j));
                        Image Chess = Image.FromFile("WhiteChess.png");
                        g.DrawImage(Chess, p2.X - r / 2, p2.Y - r / 2, r, r);

                    }
                }
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            gomo.New();
          
            textBox2.Text = "0秒";
            textBox3.Text = "0秒";
            gomo.Start = true;
            gomo.StartMarshine = false;
            if (timenum1 > 0 && timenum2>0)
            {
                timenum1 = -1;
                timenum2 = -1;
                Invalidate();

            }
            if (timenum1 > 0 && timenum2 == 0)
            {
                timenum1 = -1;
                Invalidate();

            }
            if (timenum1 == 0 && timenum2 > 0)
            {
              
                timenum2 = -1;
                Invalidate();

            }
            Invalidate();
            timer2.Start();
            timer3.Stop();
            if (timenum2 < 0)
                timenum2++;
                    }

        private Point ViewToLogic(Point p)
        {
            Point p2 = new Point(0, 0);
            int x = -1;
            int y = -1;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                     int x1 = x0 + i * inter;
                     int y1 = y0 + j * inter;
                    if (Math.Abs(x1-p.X)<inter/2 && Math.Abs(y1-p.Y)<inter/2)
                    {
                        x = i;
                        y = j;
                    }
                }
            }
            if (x > -1 && y > -1)
            {
                p2.X = x;
                p2.Y = y;
                return p2;
            }
            else
                return new Point(-1,-1);
            
        }

        private Point LogicToView(Point p)
        {
            Point p2 = new Point(0, 0);
            p2.X = x0 + p.X * inter;
            p2.Y = y0 + p.Y * inter;
             return p2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //人人对战***********************************************************************************
            if (e.Button==MouseButtons.Left && gomo.Start==true)
            {
               
                if (e.X > x0 && e.X < x0 + 14 * inter && e.Y > y0
                    && e.Y < y0 + 14 * inter)
                {
                    Point p2 = ViewToLogic(new Point(e.X, e.Y));
                    //时间计时器
                    if (gomo.CurrentChess == ChessState.BLACK)
                    {
                        timer3.Start();
                        timer2.Stop();
                    }
                    else if (gomo.CurrentChess == ChessState.WHITE)
                    {
                        timer2.Start();
                        timer3.Stop();
                    }
                    if (p2.X > -1)
                    {
                        gomo.Current = p2;
                        gomo.Down();
                          var result = gomo.Judge();
                        if (result==Result.BLACKWIN)
                        {
                            textBox1.Text = "恭喜黑方获胜";
                            Invalidate();
                            MessageBox.Show("恭喜黑方获胜");
                            timer3.Stop();
                            timer2.Stop();
                        }
                        
                        if (result == Result.WHITEWIN)
                        {
                            textBox1.Text = "恭喜白方获胜";
                            Invalidate();
                            MessageBox.Show("恭喜白方获胜");
                            timer3.Stop();
                            timer2.Stop();
                        }
                        if (result == Result.DRAW)
                        {
                            textBox1.Text = "额，平局";
                            Invalidate();
                            MessageBox.Show("额，平局");
                            timer3.Stop();
                            timer2.Stop();
                        }
                    }
                }
                  
                if (gomo.Judge() == Result.CONTINUE)
                    textBox1.Text = "继续游戏";

                if (gomo.Judge() == Result.BLACKWIN)
                    textBox1.Text = "恭喜黑方获胜";

                if (gomo.Judge() == Result.WHITEWIN)
                    textBox1.Text = "恭喜白方获胜";

                if (gomo.Judge() == Result.DRAW)
                    textBox1.Text = "额，平局";
            }
            //人机对战*****************************************************************************************
            if (e.Button == MouseButtons.Left && gomo.StartMarshine == true)
            {

                if (e.X > x0 && e.X < x0 + 14 * inter && e.Y > y0
                    && e.Y < y0 + 14 * inter)
                {
                    Point p2 = ViewToLogic(new Point(e.X, e.Y));
                    //时间计时器
             
                    if (p2.X > -1)
                    {
                        gomo.Current = p2;
                        
                        gomo.Down();
                        
                        Invalidate();
                        Thread.Sleep(500);
                        var result = gomo.Judge();
                        if (result == Result.BLACKWIN)
                        {
                            textBox1.Text = "恭喜黑方获胜";
                            Invalidate();
                            MessageBox.Show("恭喜黑方获胜");
                            timer3.Stop();
                            timer2.Stop();
                        }

                        if (result == Result.WHITEWIN)
                        {
                            textBox1.Text = "恭喜白方获胜";
                            Invalidate();
                            MessageBox.Show("恭喜白方获胜");
                            timer3.Stop();
                            timer2.Stop();
                        }
                        if (result == Result.DRAW)
                        {
                            textBox1.Text = "额，平局";
                            Invalidate();
                            MessageBox.Show("额，平局");
                            timer3.Stop();
                            timer2.Stop();
                        }
                        if (gomo.Judge() == Result.CONTINUE)
                            textBox1.Text = "继续游戏";

                        if (gomo.Judge() == Result.BLACKWIN)
                            textBox1.Text = "恭喜黑方获胜";

                        if (gomo.Judge() == Result.WHITEWIN)
                            textBox1.Text = "恭喜白方获胜";

                        if (gomo.Judge() == Result.DRAW)
                            textBox1.Text = "额，平局";


                        
                        gomo.Current = gomo.Selection(p2);
                        //权重测试
                        /*textBox4.Text = "";
                        for(int i=0;i<15;i++)
                        {
                            for(int j=0;j<15;j++)
                            {
                                textBox4.Text = textBox4.Text + "[" + i.ToString() +","+ j.ToString() + "]" + "=" + gomo.weightChessState[j, i].ToString()+"\t";
                            }
                           textBox4.Text = textBox4.Text + "\n";
                        }
                        textBox4.Text = textBox4.Text + gomo.Test1.ToString();
                        textBox4.Text = textBox4.Text + gomo.Test2.ToString();
                        textBox4.Text = textBox4.Text + gomo.Selection(p2).ToString();*/
                        if (result == Result.CONTINUE)
                        {
                           
                            gomo.Down();
                            result = gomo.Judge();
                            if (result == Result.BLACKWIN)
                            {
                                textBox1.Text = "恭喜黑方获胜";
                                Invalidate();
                                MessageBox.Show("恭喜黑方获胜");

                                timer2.Stop();
                            }

                            if (result == Result.WHITEWIN)
                            {
                                textBox1.Text = "恭喜白方获胜";
                                Invalidate();
                                MessageBox.Show("恭喜白方获胜");

                                timer2.Stop();
                            }
                            if (result == Result.DRAW)
                            {
                                textBox1.Text = "额，平局";
                                Invalidate();
                                MessageBox.Show("额，平局");

                                timer2.Stop();
                            }

                            if (gomo.Judge() == Result.CONTINUE)
                                textBox1.Text = "继续游戏";

                            if (gomo.Judge() == Result.BLACKWIN)
                                textBox1.Text = "恭喜黑方获胜";

                            if (gomo.Judge() == Result.WHITEWIN)
                                textBox1.Text = "恭喜白方获胜";

                            if (gomo.Judge() == Result.DRAW)
                                textBox1.Text = "额，平局";
                        }
                    }
                }
               
            }

           //*********************************************************************************************************
                Invalidate();

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        //悔棋 
        private void button1_Click(object sender, EventArgs e)
        {
           
            gomo.Regret();
            Invalidate();
            if (gomo.CurrentChess == ChessState.BLACK)
            {
                timer2.Start();
                timer3.Stop();
            }
            else if (gomo.CurrentChess == ChessState.WHITE)
            {
                timer3.Start();
                timer2.Stop();
            }

            if(gomo.StartMarshine==true)
            {
                gomo.Regret();
                Invalidate();
                if (gomo.CurrentChess == ChessState.BLACK)
                {
                    timer2.Start();
                    timer3.Stop();
                }
                else if (gomo.CurrentChess == ChessState.WHITE)
                {
                    timer3.Start();
                    timer2.Stop();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        //SAVE
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sa = new SaveFileDialog();
            sa.Filter = "*.gomo(五子棋文件)|*.gomo";
            if (sa.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(sa.FileName,
                    FileMode.Create, FileAccess.Write);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, gomo);
                fs.Close();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "*.gomo(五子棋文件)|*.gomo";
            if (op.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(op.FileName,
                   FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                gomo = (GomokuOK)bf.Deserialize(fs);
                Invalidate();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            gomo.MaxRegret = 1;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            gomo.MaxRegret = 2;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            gomo.MaxRegret = 3;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            gomo.MaxRegret = 4;
        }

        private void countlessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gomo.MaxRegret = 1000;
        }

        private void stepByStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer3.Stop();
            timer2.Stop();
            gomo.New2();
            Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gomo.RightReview();
            Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gomo.LeftReview();
            Invalidate();
        }

        private void byTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer3.Stop();
            timer2.Stop();
            gomo.New2();
            timer1.Start();
            Invalidate();
            
        }//自动复盘

        private void timer1_Tick(object sender, EventArgs e)
        {
            gomo.RightReview();
            Invalidate();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timenum1++;
            textBox2.Text = timenum1.ToString() + "秒";
                

        }

        private void timer3_Tick(object sender, EventArgs e)
        {

            timenum2++;
            textBox3.Text = timenum2.ToString() + "秒";
                
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void functionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            yellowbutton = 1;
            bluebutton = 0;
            greenbutton = 0;
            Invalidate();
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yellowbutton = 0;
            bluebutton = 1;
            greenbutton = 0;
            Invalidate();
            
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yellowbutton = 0;
            bluebutton = 0;
            greenbutton = 1;
            Invalidate();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //测试权重表程序
           /* textBox4.Text = "";
            for(int i=0;i<15;i++)
            {
                for(int j=0;j<15;j++)
                {
                    textBox4.Text = textBox4.Text +"[" + i.ToString()+","+j.ToString()+"]" +gomo.weightChessState[i, j].ToString()+"  ";

                }
                textBox4.Text = textBox4.Text + "\n";
            }*/
        }

        private void aIToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

        }

        private void timer4_Tick(object sender, EventArgs e)
        {
          
        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void easyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            gomo.New();
            gomo.Ratio = 1;
            gomo.Ratio2 = 2;
            timer3.Stop();
            textBox2.Text = "0秒";
            textBox3.Text = "0秒";
            gomo.Start = false;
            gomo.StartMarshine = true;
            if (timenum1 > 0)
            {
                timenum1 = -1;

                Thread.Sleep(1000);
                Invalidate();
            }
            Invalidate();
            timer2.Start();
        }

        private void difficultToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            gomo.New();
            gomo.Ratio = 2;
            timer3.Stop();
            textBox2.Text = "0秒";
            textBox3.Text = "0秒";
            gomo.Start = false;
            gomo.StartMarshine = true;
            if (timenum1 > 0)
            {
                timenum1 = -1;

                Thread.Sleep(1000);
                Invalidate();
            }
            Invalidate();
            timer2.Start();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cOLORToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 打赏MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 a = new Form2();
            a.Show();
        }

        private void 音乐开启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.play();
        }

      
        

        private void 规则介绍ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 b = new Form3();
            b.Show();
        }

        private void 认输GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gomo.GiveUp())
            {
                textBox1.Text = "恭喜白方获胜";
                MessageBox.Show("恭喜白方获胜！");
              
            }
            else
            {
                textBox1.Text = "恭喜黑方获胜";
                MessageBox.Show("恭喜黑方获胜！");
                
            }
            Invalidate();
    }


        private void 音乐结束DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private void axWindowsMediaPlayer1_Enter_1(object sender, EventArgs e)
        {
            System.Media.SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = @"music.wav";
            sp.PlayLooping();
            
        }

        private void 五子棋规则MToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
