using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
namespace JourneymapMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Insert Folder Path: ");
            string path = Console.ReadLine();
            string[] files = GetFiles(path);
            Console.Write("Insert First Region Corner X (\"none\" to disable): ");
            string corner1X = Console.ReadLine();
            int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
            if (corner1X.ToLower() != "none")
            {
                x1 = int.Parse(corner1X);
                Console.Write("Insert First Region Corner Y (\"none\" to disable): ");
                string corner1Y = Console.ReadLine();
                y1 = int.Parse(corner1Y);
                Console.Write("Insert Second Region Corner X (\"none\" to disable): ");
                string corner2X = Console.ReadLine();
                x2 = int.Parse(corner2X);
                Console.Write("Insert Second Region Corner Y (\"none\" to disable): ");
                string corner2Y = Console.ReadLine();
                y2 = int.Parse(corner2Y);
            }
            Console.Write("Insert Destination: ");
            string savePath = Console.ReadLine();
            Console.Write("Insert File Name: ");
            string name = Console.ReadLine();
            CoordsManager[] arrTest = new CoordsManager[files.Length];
            CoordsManager[] arrTest2 = new CoordsManager[files.Length];
            CoordsManager[] NoModifyArrTest = new CoordsManager[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                string fName = files[i].Remove(0, (path + @"\").Length);
                char[] test = { '.', 'p', 'n', 'g' };
                fName = fName.TrimEnd(test);
                //Console.WriteLine(fName);
                string[] tempStr = fName.Split(',');
                arrTest[i] = new CoordsManager(int.Parse(tempStr[0]), int.Parse(tempStr[1]));
                arrTest2[i] = new CoordsManager(int.Parse(tempStr[0]), int.Parse(tempStr[1]));
                NoModifyArrTest[i] = new CoordsManager(arrTest[i]);
            }
            string[] finalArr = new string[files.Length];
            CoordsManager min;
            CoordsManager min2;
            int index = 0;
            string temp = string.Empty;
            for (int t = 0; t < files.Length; t++)
            {
                for (int i = 1; i < files.Length; i++)
                {
                    min = new CoordsManager(arrTest[i - 1]);
                    if ((int)arrTest[i - 1].GetX() > (int)arrTest[i].GetX())
                    {
                        min = new CoordsManager(arrTest[i]);
                        arrTest[i] = new CoordsManager(arrTest[i - 1]);
                        arrTest[i - 1] = new CoordsManager(min);
                        NoModifyArrTest[i] = new CoordsManager(NoModifyArrTest[i - 1]);
                        NoModifyArrTest[i - 1] = new CoordsManager(min);
                        temp = files[i];
                    }
                    else if ((int)arrTest[i - 1].GetX() == (int)arrTest[i].GetX())
                    {
                        if ((int)arrTest[i - 1].GetY() > (int)arrTest[i].GetY())
                        {
                            min = new CoordsManager(arrTest[i]);
                            arrTest[i] = new CoordsManager(arrTest[i - 1]);
                            arrTest[i - 1] = new CoordsManager(min);
                            NoModifyArrTest[i] = new CoordsManager(NoModifyArrTest[i - 1]);
                            NoModifyArrTest[i - 1] = new CoordsManager(min);
                        }
                    }
                }
            }

            for (int t = 0; t < files.Length; t++)
            {
                for (int i = 1; i < files.Length; i++)
                {
                    min2 = new CoordsManager(arrTest2[i - 1]);
                    if ((int)arrTest2[i - 1].GetY() > (int)arrTest2[i].GetY())
                    {
                        min2 = new CoordsManager(arrTest2[i]);
                        arrTest2[i] = new CoordsManager(arrTest2[i - 1]);
                        arrTest2[i - 1] = new CoordsManager(min2);
                    }
                    else if ((int)arrTest2[i - 1].GetY() == (int)arrTest2[i].GetY())
                    {
                        if ((int)arrTest2[i - 1].GetX() > (int)arrTest2[i].GetX())
                        {
                            min2 = new CoordsManager(arrTest2[i]);
                            arrTest2[i] = new CoordsManager(arrTest2[i - 1]);
                            arrTest2[i - 1] = new CoordsManager(min2);
                        }
                    }
                }
            }
            if (corner1X.ToLower() != "none")
            {
                CoordsManager[] limArrTes;
                CoordsManager[] limArrTes2;
                int withinLimitCount = 0;
                foreach (CoordsManager pos in arrTest)
                {
                    if (pos.GetX() >= x1 && pos.GetY() >= y1 && pos.GetX() <= x2 && pos.GetY() <= y2)
                    {
                        withinLimitCount++;
                    }
                    else if (pos.GetX() <= x1 && pos.GetY() <= y1 && pos.GetX() >= x2 && pos.GetY() >= y2)
                    {
                        withinLimitCount++;
                    }
                }
                limArrTes = new CoordsManager[withinLimitCount];
                limArrTes2 = new CoordsManager[withinLimitCount];
                CoordsManager[] noModifyLimArr = new CoordsManager[withinLimitCount];
                int allowance = 0;
                for (int i = 0; i < arrTest.Length && allowance < withinLimitCount; i++)
                {
                    if (arrTest[i].GetX() >= x1 && arrTest[i].GetY() >= y1 && arrTest[i].GetX() <= x2 && arrTest[i].GetY() <= y2)
                    {
                        limArrTes[allowance] = arrTest[i];
                        noModifyLimArr[allowance] = arrTest[i];
                        allowance++;
                    }
                    else if (arrTest[i].GetX() <= x1 && arrTest[i].GetY() <= y1 && arrTest[i].GetX() >= x2 && arrTest[i].GetY() >= y2)
                    {
                        limArrTes[allowance] = arrTest[i];
                        noModifyLimArr[allowance] = arrTest[i];
                        allowance++;
                    }
                }
                allowance = 0;
                for (int i = 0; i < arrTest.Length && allowance < withinLimitCount; i++)
                {
                    if (arrTest[i].GetX() >= x1 && arrTest[i].GetY() >= y1 && arrTest[i].GetX() <= x2 && arrTest[i].GetY() <= y2)
                    {
                        limArrTes2[allowance] = arrTest[i];
                        allowance++;
                    }
                    else if (arrTest[i].GetX() <= x1 && arrTest[i].GetY() <= y1 && arrTest[i].GetX() >= x2 && arrTest[i].GetY() >= y2)
                    {
                        limArrTes2[allowance] = arrTest[i];
                        allowance++;
                    }
                }
                string str = string.Empty;
                Image[] images = new Image[limArrTes.Length];
                for (int i = 0; i < limArrTes.Length; i++)
                {
                    str = string.Format("{0},{1}", noModifyLimArr[i].GetX(), noModifyLimArr[i].GetY());
                    Bitmap test = new Bitmap((path + @"\") + str + ".png");
                    images[i] = new Bitmap(test, 512, 512);
                }
                for (int i = 1; i < limArrTes.Length; i++)
                {
                    limArrTes[i].SetX(limArrTes[i].GetX() - limArrTes[0].GetX());
                    limArrTes[i].SetY(limArrTes[i].GetY() - limArrTes2[0].GetY());
                }
                limArrTes[0].SetX(limArrTes[0].GetX() - limArrTes[0].GetX());
                limArrTes[0].SetY(limArrTes[0].GetY() - limArrTes2[0].GetY());
                Point[] pts = new Point[limArrTes.Length];
                for (int i = 0; i < limArrTes.Length; i++)
                {
                    pts[i] = new Point(limArrTes[i].GetX() * 512, limArrTes[i].GetY() * 512);
                }
                System.Drawing.Bitmap finalImage = null;
                try
                {
                    int width = (limArrTes[limArrTes.Length - 1].GetX() * 512) + 512;
                    int max = 0;
                    foreach (CoordsManager pos in limArrTes2)
                    {
                        if (pos.GetY() > max)
                        {
                            max = pos.GetY();
                        }
                    }
                    int height = (max * 512) + 512;
                    finalImage = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                    {
                        g.Clear(System.Drawing.Color.Gray); // Change this to whatever you want the background color to be, you may set this to Color.Transparent as well
                        for (int i = 0; i < images.Length; i++)
                        {
                            g.DrawImage(images[i], pts[i]);
                            string imgPos = string.Format("{0},{1}", noModifyLimArr[i].GetX(), noModifyLimArr[i].GetY());
                            g.DrawLine(new Pen(Color.AliceBlue, 1), pts[i], new Point((pts[i].X) + 512, pts[i].Y));
                            g.DrawLine(new Pen(Color.AliceBlue), pts[i], new Point(pts[i].X, (pts[i].Y) + 512));
                            g.DrawString(imgPos, new Font("Times New Roman", 12.0f), new SolidBrush(Color.FromArgb(125, Color.Red)), pts[i]);
                        }
                    }
                    finalImage.Save(savePath + @"\" + name + ".png", ImageFormat.Png);
                }
                catch (Exception ex)
                {
                    if (finalImage != null)
                        finalImage.Dispose();

                    throw ex;
                }
                /*
                            for (int i = 0; i < files.Length; i++)
                            {
                                str = string.Format("{0},{1}", arrTest[i].GetX(), arrTest[i].GetY());
                                files[i] = str;
                            }
                            foreach (string file in files)
                            {

                            }*/
            }
            else
            {
                string str = string.Empty;
                Image[] images = new Image[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    str = string.Format("{0},{1}", arrTest[i].GetX(), arrTest[i].GetY());
                    Bitmap test = new Bitmap((path + @"\") + str + ".png");
                    images[i] = new Bitmap(test, 128, 128);
                }
                for (int i = 1; i < arrTest.Length; i++)
                {
                    arrTest[i].SetX(arrTest[i].GetX() - arrTest[0].GetX());
                    arrTest[i].SetY(arrTest[i].GetY() - arrTest2[0].GetY());
                }
                int temp1 = arrTest[0].GetX();
                arrTest[0].SetX(arrTest[0].GetX() - arrTest[0].GetX());
                arrTest[0].SetY(arrTest[0].GetY() - arrTest2[0].GetY());
                Point[] pts = new Point[arrTest.Length];
                for (int i = 0; i < arrTest.Length; i++)
                {
                    pts[i] = new Point(arrTest[i].GetX() * 128, arrTest[i].GetY() * 128);
                }
                System.Drawing.Bitmap finalImage = null;
                try
                {
                    int width = (arrTest[arrTest.Length - 1].GetX() * 128) + 128;
                    int max = 0;
                    foreach (CoordsManager pos in arrTest)
                    {
                        if (pos.GetY() > max)
                        {
                            max = pos.GetY();
                        }
                    }
                    int height = (max * 128) + 256;
                    //int height = (arrTest2[arrTest2.Length - 1].GetY() * 512) + 512;
                    finalImage = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                    {
                        g.Clear(System.Drawing.Color.Gray); // Change this to whatever you want the background color to be, you may set this to Color.Transparent as well
                        for (int i = 0; i < images.Length; i++)
                        {
                            g.DrawImage(images[i], pts[i]);
                            /**/
                            string imgPos = string.Format("{0},{1}", NoModifyArrTest[i].GetX(), NoModifyArrTest[i].GetY());
                            g.DrawLine(new Pen(Color.AliceBlue, 1), pts[i], new Point((pts[i].X) + 128, pts[i].Y));
                            g.DrawLine(new Pen(Color.AliceBlue, 1), pts[i], new Point(pts[i].X, (pts[i].Y) + 128));
                            g.DrawString(imgPos, new Font("Times New Roman", 16.0f), new SolidBrush(Color.FromArgb(125, Color.Red)), pts[i]);
                        }
                        g.DrawString("Created by MaxedCarp", new Font("Times New Roman", 56.0f), new SolidBrush(Color.FromArgb(125, Color.Red)), new Point(0, 0));
                    }
                    /**/
                    using (MemoryStream memory = new MemoryStream())
                    {
                        using (FileStream fs = new FileStream(savePath + @"\" + name + ".png", FileMode.Create, FileAccess.ReadWrite))
                        {
                            finalImage.Save(memory, ImageFormat.Png);
                            byte[] bytes = memory.ToArray();
                            fs.Write(bytes, 0, bytes.Length);
                        }
                    }
                    /*Bitmap finalImage2 = new Bitmap(finalImage);
                    finalImage.Dispose();
                    finalImage2.Save(savePath + @"\test.png", ImageFormat.Png);*/
                }
                catch (Exception ex)
                {
                    if (finalImage != null)
                        finalImage.Dispose();

                    throw ex;
                }
                /*
                            for (int i = 0; i < files.Length; i++)
                            {
                                str = string.Format("{ 0},{1}", arrTest[i].GetX(), arrTest[i].GetY());
                                files[i] = str;
                            }
                            foreach (string file in files)
                            {

                            }*/
            }
        }
        public static string[] GetFiles(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            return files;
        }
    }
}
