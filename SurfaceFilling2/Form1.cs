using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace SurfaceFilling2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            N = 400;
            M = 600;
            Pn = 200;
            //Form1.ActiveForm.Text = "Aha";
            //MessageBox.Show("The calculations are complete", "My Application",MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            selPen = new Pen(Color.Blue);
            whitePen = new Pen(Color.White);
            graphic = this.pictureBox1.CreateGraphics();
            graphic.Clear(Color.White);
            noktalar = new Point[Pn];
            kenarlar_bas_son = new int[Pn];
            kenarlar_bas_son[0] = 0;
            poligon_noktalar = new Point[10000];
            matrix = new int[N, M];
            poligon_nokta_sayac = 0;
            nokta_sayisi = 0;
            pictureBox1.Enabled = true;
            pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            nokta_girisi_devam = 1;
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                    matrix[i, j] = 0;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("Tek tıklama", "Dnm",MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (nokta_girisi_devam == 1)
                    {
                        Rectangle rect1 = new Rectangle();
                        rect1.Width = 10;
                        rect1.Height = 10;
                        rect1.X = e.X - 5;
                        rect1.Y = e.Y - 5;
                        selPen.Color = Color.Red;
                        graphic.DrawEllipse(selPen, rect1);
                        noktalar[nokta_sayisi].X = e.X;
                        noktalar[nokta_sayisi].Y = e.Y;
                        nokta_sayisi++;

                        selPen.Color = Color.Blue;
                        if (nokta_sayisi > 1)
                        {
                            //graphic.DrawLine(selPen, noktalar[nokta_sayisi - 2], noktalar[nokta_sayisi - 1]);
                            BLine2(noktalar[nokta_sayisi - 2], noktalar[nokta_sayisi - 1]);
                            kenarlar_bas_son[nokta_sayisi - 1] = poligon_nokta_sayac;
                        }
                        this.Text = nokta_sayisi.ToString();
                    }
                    break;
                case MouseButtons.Right:
                    //graphic.DrawLine(selPen, noktalar[nokta_sayisi - 1], noktalar[0]);
                    BLine2(noktalar[nokta_sayisi - 1], noktalar[0]);
                    kenarlar_bas_son[nokta_sayisi - 1] = poligon_nokta_sayac;
                    nokta_girisi_devam = 0;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    btnSeedFill.Enabled = true;

                    /*   Rectangle rect2 = new Rectangle();
                       rect2.Width = 10;
                       rect2.Height = 10;
                       rect2.X = e.X-5;
                       rect2.Y = e.Y-5;
                       selPen.Color = Color.Red;
                       button1.Text = e.X.ToString();
                       graphic.DrawRectangle(selPen, rect2);*/
                    break;
                default:
                    break;
            }
        }
        private void BLine(Point p1, Point p2)
        {
            int x, y;
            int deltaX, deltaY;
            int s1, s2;
            int temp;
            int intCH;
            int error;

            x = p1.X;
            y = p1.Y;

            deltaX = Math.Abs(p2.X - x);
            deltaY = Math.Abs(p2.Y - p1.Y);

            s1 = Math.Sign(p2.X - p1.X);
            s2 = Math.Sign(p2.Y - p1.Y);

            button1.Text = s1.ToString();
            this.Text = s2.ToString();

            if (deltaY > deltaX)
            {
                temp = deltaX;
                deltaX = deltaY;
                deltaY = deltaX;
                intCH = 1;
            }
            else
            {
                intCH = 0;
            }
            error = 2 * deltaY - deltaX;
            for (int i = 0; i < deltaX; i++)
            {
                graphic.DrawEllipse(selPen, x, y, 1, 1);
                while (error > 0)
                {
                    if (intCH == 1) x = x + s1;
                    else y = y + s2;
                    error = error - 2 * deltaX;
                }
                if (intCH == 1) y = y + s2;
                else x = x + s1;
                error = error + 2 * deltaY;
            }
        }
        private void BLine2(Point p1, Point p2)
        {
            // function line(x0, x1, y0, y1)
            int steep, x0, x1, y0, y1, temp;
            x0 = p1.X; x1 = p2.X;
            y0 = p1.Y; y1 = p2.Y;

            if (Math.Abs(y1 - y0) > Math.Abs(x1 - x0))
                steep = 1;
            else
                steep = 0;
            //boolean steep := abs(y1 - y0) > abs(x1 - x0)
            if (steep == 1)  //if steep then
            {

                temp = x0; x0 = y0; y0 = temp; // swap(x0, y0)
                temp = x1; x1 = y1; y1 = temp; //  swap(x1, y1)
            }
            if (x0 > x1) //if x0 > x1 then
            {
                temp = x0; x0 = x1; x1 = temp; // swap(x0, x1)
                temp = y0; y0 = y1; y1 = temp; //  swap(y0, y1)
            }
            int deltaX = x1 - x0;    //  int deltax := x1 - x0
            int deltaY = Math.Abs(y1 - y0);   //  int deltay := abs(y1 - y0)
            double error = 0;   //  real error := 0
            double deltaE = (double)deltaY / (double)deltaX; // real deltaerr := deltay / deltax
            int ystep;   // int ystep
            int y = y0;  // int y := y0
            if (y0 < y1) ystep = 1;  // if y0 < y1 then ystep := 1 else ystep := -1
            else ystep = -1;
            for (int x = x0; x <= x1; x++)
            {
                if (steep == 1)
                {
                    graphic.DrawEllipse(selPen, y, x, 1, 1);
                    poligon_noktalar[poligon_nokta_sayac].X = y;
                    poligon_noktalar[poligon_nokta_sayac].Y = x;
                    poligon_nokta_sayac++;
                    matrix[x, y] = 1;
                }
                else
                {
                    graphic.DrawEllipse(selPen, x, y, 1, 1);
                    poligon_noktalar[poligon_nokta_sayac].X = x;
                    poligon_noktalar[poligon_nokta_sayac].Y = y;
                    poligon_nokta_sayac++;
                    matrix[y, x] = 1;
                }
                error = error + deltaE;
                if (error >= 0.5)
                {
                    y = y + ystep;
                    error = error - 1;
                }
            }
            button1.Text = poligon_nokta_sayac.ToString();
            /* function line(x0, x1, y0, y1)
                 boolean steep := abs(y1 - y0) > abs(x1 - x0)
                 if steep then
                     swap(x0, y0)
                     swap(x1, y1)
                 if x0 > x1 then
                     swap(x0, x1)
                     swap(y0, y1)
                 int deltax := x1 - x0
                 int deltay := abs(y1 - y0)
                 real error := 0
                 real deltaerr := deltay / deltax
                 int ystep
                 int y := y0
                 if y0 < y1 then ystep := 1 else ystep := -1
                 for x from x0 to x1
                     if steep then plot(y,x) else plot(x,y)
                     error := error + deltaerr
                     if error ≥ 0.5 then
                         y := y + ystep
                         error := error - 1.0

                        */
        }
        private void orderedEdgeList()
        {
        }
        private void EdgeFill()
        {
        }

        
        private void SeedFill(){
            List<Point> containedPoints = new List<Point>();
            Pen drawPen = new Pen(Color.Black);

            for (; ; )
            {
                if (yigin.Count != 0)
                {
                    var pt = (Point)yigin.Pop();

                    graphic.DrawEllipse(drawPen, pt.X, pt.Y, 1, 1);
                    containedPoints.Add(pt);

                    Point[] ptNeighborArr = {
                        new Point(pt.X,pt.Y+1),
                        new Point(pt.X-1,pt.Y),
                        new Point(pt.X+1,pt.Y),
                        new Point(pt.X,pt.Y-1),
                    };

                    for (int i = 0; i < ptNeighborArr.Length; i++)
                    {
                        if (!poligon_noktalar.Contains(ptNeighborArr[i])
                            && !containedPoints.Contains(ptNeighborArr[i]))
                        {
                            yigin.Push(ptNeighborArr[i]);
                        }
                    }
                }
                else
                    break;

            }
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            orderedEdgeList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EdgeFill();
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //
            //Çift tıklama ile alınan nokta ilk eleman olarak yığına ekleniyor.
            Point seedPoint;
            seedPoint = new Point();
            seedPoint.X = e.X;
            seedPoint.Y = e.Y;

            yigin.Push(seedPoint);
            SeedFill();
        }

        private void btnSeedFill_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Algoritmayı çalıştırmak için poligon içindeki bir noktaya çift tıklayın.",
                            "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        
        private static Stack yigin = new Stack();
        private Graphics graphic;
        private Pen selPen, whitePen;
        private Point[] noktalar;
        private int[] kenarlar_bas_son;
        private int nokta_sayisi;
        private int nokta_girisi_devam;
        private Point[] poligon_noktalar;
        private int poligon_nokta_sayac;
        private int[,] matrix;
        private int N, M, Pn;
    }

}
