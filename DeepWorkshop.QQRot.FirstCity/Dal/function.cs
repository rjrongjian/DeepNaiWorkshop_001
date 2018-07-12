using DeepWorkshop.QQRot.FirstCity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormsApplication4;

namespace AI.Bll
{
    internal class function
    {
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="text"></param>
        public static void xiewenjian(string path, byte[] text)
        {
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(text);
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 播放wmv
        /// </summary>
        /// <param name="buf"></param>
        public static void Play(byte[] buf)
        {
            MemoryStream ms = new MemoryStream(buf);
            SoundPlayer sp = new SoundPlayer(ms);
            sp.Play();
        }

        /// <summary>
        /// 取本机IP
        /// </summary>
        public static List<string> GetIP()
        {
            List<string> iplist = new List<string>();
            string hostName = Dns.GetHostName();//本机名
            //System.Net.IPAddress[] addressList = Dns.GetHostByName(hostName).AddressList;//会警告GetHostByName()已过期，我运行时且只返回了一个IPv4的地址
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6
            foreach (IPAddress ip in addressList)
            {
                iplist.Add(ip.ToString());
            }
            return iplist;
        }

        /// <summary>
        /// 等比例缩放宽高
        /// </summary>
        /// <param name="original"></param>
        /// <param name="END"></param>
        /// <returns></returns>
        public static void ReadFileStream(ref double X, ref double Y, double cd)
        {
            if (X <= cd && Y <= cd) return;
            if (X == Y)
            {
                X = cd;

                Y = cd;
                return;
            }
            if (X > Y)
            {
                Y = Y * (cd / X);
                X = cd;
            }
            if (X < Y)
            {
                X = X * (cd / Y);

                Y = cd;
            }
        }

        /// <summary>
        /// 读文件流
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadFileStream(string path)
        {
            using (FileStream fsRead = new FileStream(path, FileMode.Open))
            {
                int fsLen = (int)fsRead.Length;
                byte[] heByte = new byte[fsLen];
                fsRead.Read(heByte, 0, heByte.Length);
                fsRead.Close();
                return heByte;
            }
        }

        /// <summary>
        /// 设置Image大小
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public static byte[] setGifSize(Image res, int width, int height)
        {
            if (getImageF(res) != ImageFormat.Gif)
            {
                Image img = new Bitmap(res, width, height);
                img.Tag = res.Tag;
                return ImageToBytes(img);
            }

            //res.Save(Directory.GetCurrentDirectory() + @"\temp2.gif");
            Image gif = new Bitmap(width, height);
            Image frame = new Bitmap(width, height);
            //Image res = Image.FromFile(path);
            Graphics g = Graphics.FromImage(gif);
            Rectangle rg = new Rectangle(0, 0, width, height);

            //Image img = new Bitmap(res, width, height);

            //img.Save(System.IO.Directory.GetCurrentDirectory() + @"\temp1.gif");

            Graphics gFrame = Graphics.FromImage(frame);

            foreach (Guid gd in res.FrameDimensionsList)
            {
                FrameDimension fd = new FrameDimension(gd);

                //因为是缩小GIF文件所以这里要设置为Time，如果是TIFF这里要设置为PAGE，因为GIF以时间分割，TIFF为页分割
                FrameDimension f = FrameDimension.Time;
                int count = res.GetFrameCount(fd);
                ImageCodecInfo codecInfo = GetEncoder(ImageFormat.Gif);
                System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.SaveFlag;
                EncoderParameters eps = null;

                for (int i = 0; i < count; i++)
                {
                    res.SelectActiveFrame(f, i);
                    if (0 == i)
                    {
                        eps = new EncoderParameters(1);
                        //第一帧需要设置为MultiFrame
                        eps.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.MultiFrame);
                        g.DrawImage(res, rg);
                        bindProperty(res, gif);
                        gif.Save(System.IO.Directory.GetCurrentDirectory() + @"\temp.gif", codecInfo, eps);
                    }
                    else
                    {
                        gFrame.DrawImage(res, rg);

                        eps = new EncoderParameters(1);

                        //如果是GIF这里设置为FrameDimensionTime，如果为TIFF则设置为FrameDimensionPage

                        eps.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.FrameDimensionTime);

                        bindProperty(res, frame);
                        gif.SaveAdd(frame, eps);
                    }
                }

                eps = new EncoderParameters(1);
                eps.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.Flush);
                gif.SaveAdd(eps);
            }

            return ReadFileStream(System.IO.Directory.GetCurrentDirectory() + @"\temp.gif");
        }

        /// <summary>
        /// 将源图片文件里每一帧的属性设置到新的图片对象里
        /// </summary>
        /// <param name="a">源图片帧</param>
        /// <param name="b">新的图片帧</param>
        private static void bindProperty(Image a, Image b)
        {
            //这个东西就是每一帧所拥有的属性，可以用GetPropertyItem方法取得这里用为完全复制原有属性所以直接赋值了

            //顺便说一下这个属性里包含每帧间隔的秒数和透明背景调色板等设置，这里具体那个值对应那个属性大家自己在msdn搜索GetPropertyItem方法说明就有了

            for (int i = 0; i < a.PropertyItems.Length; i++)
            {
                b.SetPropertyItem(a.PropertyItems[i]);
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// 无阻塞延迟
        /// </summary>
        /// <param name="milliSecond"></param>
        public static void yanci(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                Application.DoEvents();
            }
        }

        public static Image wzztp(string str)
        {
            Graphics g = Graphics.FromImage(new Bitmap(1, 1));
            Font font = new Font("宋体", 9);
            SizeF sizeF = g.MeasureString(str, font); //测量出字体的高度和宽度
            Brush brush; //笔刷，颜色
            brush = Brushes.Lime;
            PointF pf = new PointF(0, 0);
            Bitmap img = new Bitmap(Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height));
            g = Graphics.FromImage(img);
            g.FillRectangle(brush, 0, 0, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height));
            g.DrawString(str, font, brush, pf);
            return img;
        }

        /// <summary>
        /// 把文字转换才Bitmap
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="rect">用于输出的矩形，文字在这个矩形内显示，为空时自动计算</param>
        /// <param name="fontcolor">字体颜色</param>
        /// <param name="backColor">背景颜色</param>
        /// <returns></returns>
        public static Image TextToBitmap(string text, Color fontcolor, Color backColor)
        {
            Font font = new Font("宋体", 9);
            Graphics g;
            Bitmap bmp;
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);

            bmp = new Bitmap(1, 1);
            g = Graphics.FromImage(bmp);
            //计算绘制文字所需的区域大小（根据宽度计算长度），重新创建矩形区域绘图
            SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);

            int width = (int)(sizef.Width + 1);
            int height = (int)(sizef.Height + 1);
            Rectangle rect = new Rectangle(0, 0, width, height);
            bmp.Dispose();

            bmp = new Bitmap(width, height);

            g = Graphics.FromImage(bmp);

            //使用ClearType字体功能
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.FillRectangle(new SolidBrush(backColor), rect);
            g.DrawString(text, font, new SolidBrush(fontcolor), rect, format);
            return bmp;
        }

        /// <summary>
        /// 取两文本中间字符串
        /// </summary>
        /// <param name="String"></param>
        /// <param name="UPString"></param>
        /// <param name="lowString"></param>
        /// <returns></returns>
        public static string middlestring(string String, string UPString, string lowString)
        {
            int s = String.IndexOf(UPString);
            if (s == -1) { return ""; }
            s = s + UPString.Length;
            int f = String.IndexOf(lowString, s);
            if (f == -1) { return ""; }
            return String.Substring(s, f - s);
        }

        /// <summary>
        /// 删除之间字符串
        /// </summary>
        /// <param name="String"></param>
        /// <param name="UPString"></param>
        /// <param name="lowString"></param>
        /// <returns></returns>
        public static string Deletetext(string String, string UPString, string lowString)
        {
            string text = String.Replace(UPString + middlestring(String, UPString, lowString) + lowString, null);
            if (middlestring(text, UPString, lowString) != "")
            {
                text = Deletetext(text, UPString, lowString);
            }
            return text;
        }

        /// <summary>
        /// 取13位时间戳
        /// </summary>
        /// <returns></returns>
        public static string MilliTime()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            ret = Convert.ToInt64(ts.TotalMilliseconds).ToString(); ;
            return ret;
        }

        /// <summary>
        /// Convert Image to Byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.MemoryBmp))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// Convert Byte[] to Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }

        /// <summary>
        /// 绘制消息提示  红圆圈VS数字
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        public static void prompt(Graphics g, int x, int y, string text)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Brush bush = new SolidBrush(Color.FromArgb(255, 59, 48));//填充的颜色

            g.FillEllipse(bush, x, y, 16, 16);//填充椭圆，x坐标、y坐标、宽、高，如果是100，则半径为50

            Font myFont = new Font("宋体", text.Length == 1 ? 10 : 8, FontStyle.Regular);
            bush = new SolidBrush(Color.White);//填充的颜色
            x = text.Length == 1 ? x + 3 : x + 1;

            g.DrawString(text, myFont, bush, x, text.Length == 1 ? y + 2 : y + 3);
        }

        /* /// <summary>
   /// 柔化
   /// <param name="b">原始图</param>
   /// <returns>输出图</returns>
        public Bitmap KiBlur(Bitmap b)
        {
            if (b == null)
            {
                return null;
            }

            int w = b.Width;
            int h = b.Height;

            try
            {
                Bitmap bmpRtn = new Bitmap(w, h, PixelFormat.Format24bppRgb);

                BitmapData srcData = b.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                BitmapData dstData = bmpRtn.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* pIn = (byte*)srcData.Scan0.ToPointer();
                    byte* pOut = (byte*)dstData.Scan0.ToPointer();
                    int stride = srcData.Stride;
                    byte* p;

                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            //取周围9点的值
                            if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
                            {
                                //不做
                                pOut[0] = pIn[0];
                                pOut[1] = pIn[1];
                                pOut[2] = pIn[2];
                            }
                            else
                            {
                                int r1, r2, r3, r4, r5, r6, r7, r8, r9;
                                int g1, g2, g3, g4, g5, g6, g7, g8, g9;
                                int b1, b2, b3, b4, b5, b6, b7, b8, b9;

                                float vR, vG, vB;

                                //左上
                                p = pIn - stride - 3;
                                r1 = p[2];
                                g1 = p[1];
                                b1 = p[0];

                                //正上
                                p = pIn - stride;
                                r2 = p[2];
                                g2 = p[1];
                                b2 = p[0];

                                //右上
                                p = pIn - stride + 3;
                                r3 = p[2];
                                g3 = p[1];
                                b3 = p[0];

                                //左侧
                                p = pIn - 3;
                                r4 = p[2];
                                g4 = p[1];
                                b4 = p[0];

                                //右侧
                                p = pIn + 3;
                                r5 = p[2];
                                g5 = p[1];
                                b5 = p[0];

                                //右下
                                p = pIn + stride - 3;
                                r6 = p[2];
                                g6 = p[1];
                                b6 = p[0];

                                //正下
                                p = pIn + stride;
                                r7 = p[2];
                                g7 = p[1];
                                b7 = p[0];

                                //右下
                                p = pIn + stride + 3;
                                r8 = p[2];
                                g8 = p[1];
                                b8 = p[0];

                                //自己
                                p = pIn;
                                r9 = p[2];
                                g9 = p[1];
                                b9 = p[0];

                                vR = (float)(r1 + r2 + r3 + r4 + r5 + r6 + r7 + r8 + r9);
                                vG = (float)(g1 + g2 + g3 + g4 + g5 + g6 + g7 + g8 + g9);
                                vB = (float)(b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8 + b9);

                                vR /= 9;
                                vG /= 9;
                                vB /= 9;

                                pOut[0] = (byte)vB;
                                pOut[1] = (byte)vG;
                                pOut[2] = (byte)vR;
                            }

                            pIn += 3;
                            pOut += 3;
                        }// end of x

                        pIn += srcData.Stride - w * 3;
                        pOut += srcData.Stride - w * 3;
                    } // end of y
                }

                b.UnlockBits(srcData);
                bmpRtn.UnlockBits(dstData);

                return bmpRtn;
            }
            catch
            {
                return null;
            }
        } // end of KiBlur
            */

        public class LockBitmap
        {
            private Bitmap source = null;
            private IntPtr Iptr = IntPtr.Zero;
            private BitmapData bitmapData = null;

            public byte[] Pixels { get; set; }
            public int Depth { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }

            public LockBitmap(Bitmap source)
            {
                this.source = source;
            }

            /// <summary>
            /// Lock bitmap data
            /// </summary>
            public void LockBits()
            {
                try
                {
                    // Get width and height of bitmap
                    Width = source.Width;
                    Height = source.Height;

                    // get total locked pixels count
                    int PixelCount = Width * Height;

                    // Create rectangle to lock
                    Rectangle rect = new Rectangle(0, 0, Width, Height);

                    // get source bitmap pixel format size
                    Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

                    // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                    if (Depth != 8 && Depth != 24 && Depth != 32)
                    {
                        throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                    }

                    // Lock bitmap and return bitmap data
                    bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                                 source.PixelFormat);

                    // create byte array to copy pixel values
                    int step = Depth / 8;
                    Pixels = new byte[PixelCount * step];
                    Iptr = bitmapData.Scan0;

                    // Copy data from pointer to array
                    Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Unlock bitmap data
            /// </summary>
            public void UnlockBits()
            {
                try
                {
                    // Copy data from byte array to pointer
                    Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

                    // Unlock bitmap data
                    source.UnlockBits(bitmapData);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get the color of the specified pixel
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public Color GetPixel(int x, int y)
            {
                Color clr = Color.Empty;

                // Get color components count
                int cCount = Depth / 8;

                // Get start index of the specified pixel
                int i = ((y * Width) + x) * cCount;

                if (i > Pixels.Length - cCount)
                    throw new IndexOutOfRangeException();

                if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
                {
                    byte b = Pixels[i];
                    byte g = Pixels[i + 1];
                    byte r = Pixels[i + 2];
                    byte a = Pixels[i + 3]; // a
                    clr = Color.FromArgb(a, r, g, b);
                }
                if (Depth == 24) // For 24 bpp get Red, Green and Blue
                {
                    byte b = Pixels[i];
                    byte g = Pixels[i + 1];
                    byte r = Pixels[i + 2];
                    clr = Color.FromArgb(r, g, b);
                }
                if (Depth == 8)
                // For 8 bpp get color value (Red, Green and Blue values are the same)
                {
                    byte c = Pixels[i];
                    clr = Color.FromArgb(c, c, c);
                }
                return clr;
            }

            /// <summary>
            /// Set the color of the specified pixel
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="color"></param>
            public void SetPixel(int x, int y, Color color)
            {
                // Get color components count
                int cCount = Depth / 8;

                // Get start index of the specified pixel
                int i = ((y * Width) + x) * cCount;

                if (Depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
                {
                    Pixels[i] = color.B;
                    Pixels[i + 1] = color.G;
                    Pixels[i + 2] = color.R;
                    Pixels[i + 3] = color.A;
                }
                if (Depth == 24) // For 24 bpp set Red, Green and Blue
                {
                    Pixels[i] = color.B;
                    Pixels[i + 1] = color.G;
                    Pixels[i + 2] = color.R;
                }
                if (Depth == 8)
                // For 8 bpp set color value (Red, Green and Blue values are the same)
                {
                    Pixels[i] = color.B;
                }
            }
        }

        /// <summary>
        /// 高斯模糊
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap SoftenImage(Bitmap bmp)
        {
            int height = bmp.Height;
            int width = bmp.Width;
            Bitmap newbmp = new Bitmap(width, height);

            LockBitmap lbmp = new LockBitmap(bmp);
            LockBitmap newlbmp = new LockBitmap(newbmp);
            lbmp.LockBits();
            newlbmp.LockBits();

            Color pixel;
            //高斯模板
            int[] Gauss = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int Index = 0;
                    for (int col = -1; col <= 1; col++)
                    {
                        for (int row = -1; row <= 1; row++)
                        {
                            pixel = lbmp.GetPixel(x + row, y + col);
                            r += pixel.R * Gauss[Index];
                            g += pixel.G * Gauss[Index];
                            b += pixel.B * Gauss[Index];
                            Index++;
                        }
                    }
                    r /= 16;
                    g /= 16;
                    b /= 16;
                    //处理颜色值溢出
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    newlbmp.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            }
            lbmp.UnlockBits();
            newlbmp.UnlockBits();
            return newbmp;
        }

        /// <summary>
        /// 画圆角
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="cornerRadius"></param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath Rect = new GraphicsPath();
            Rect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            Rect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            Rect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            Rect.AddArc(rect.X, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            Rect.CloseFigure();
            return Rect;
        }

        /// <summary>
        /// 图片圆角化
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap CreateRoundRectImage(Bitmap bmp)
        {
            Bitmap bp = new Bitmap(bmp.Width, bmp.Height);
            bp.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
            using (Graphics grfx = Graphics.FromImage(bp))
            {
                grfx.SmoothingMode = SmoothingMode.HighQuality;
                Region rg1 = new Region(CreateRoundedRectanglePath(new Rectangle(0, 0, bmp.Width, bmp.Height), 5));
                grfx.Clip = rg1;
                grfx.DrawImageUnscaled(bmp, 0, 0);
            }
            return bp;
        }

        #region --base64

        /// <summary>
        /// base64到字符串
        /// </summary>
        /// <param name="code_type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }

        /// <summary>
        /// 字符串到base64
        /// </summary>
        /// <param name="code_type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string EncodeBase64(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }

        /// <summary>
        /// base64到图片
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static Image Base64ToImage(string base64)
        {
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64));
            return new Bitmap(stream);
        }

        /// <summary>
        /// 图片到base64
        /// </summary>
        /// <param name="Img"></param>
        /// <returns></returns>
        public static string ImageToBase64(Image Img)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                Img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return Convert.ToBase64String(stream.GetBuffer());
            }
            catch (Exception ex) { return ""; }
        }

        #endregion --base64

        #region --获取字符串的首字母

        /// <summary>
        /// 获取联系人姓名的首字母
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns>姓名的首字母</returns>
        public static string Get_Name_Frist(string name)
        {
            String _Temp = null;
            for (int i = 0; i < name.Length; i++)
                _Temp = _Temp + GetOneIndex(name.Substring(i, 1));
            try
            {
                return _Temp.Substring(0, 1).ToUpper();
            }
            catch (Exception ex)
            {
                return "#";
            }
        }

        /// <summary>
        /// 取MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5String(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] data2 = md5.ComputeHash(data);

            return GetbyteToString(data2);
            //return BitConverter.ToString(data2).Replace("-", "").ToLower();
        }

        private static string GetbyteToString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到单个字符的首字母
        /// </summary>
        /// <param name="OneIndexTxt"></param>
        /// <returns></returns>
        public static String GetOneIndex(String OneIndexTxt)
        {
            if (Convert.ToChar(OneIndexTxt) >= 0 && Convert.ToChar(OneIndexTxt) < 256)
                return OneIndexTxt;
            else
            {
                Encoding gb2312 = Encoding.GetEncoding("gb2312");
                byte[] unicodeBytes = Encoding.Unicode.GetBytes(OneIndexTxt);
                byte[] gb2312Bytes = Encoding.Convert(Encoding.Unicode, gb2312, unicodeBytes);
                if (gb2312Bytes.Length == 1) return "";
                try
                {
                    return GetX(Convert.ToInt32(
                 String.Format("{0:D2}", Convert.ToInt16(gb2312Bytes[0]) - 160)
                 + String.Format("{0:D2}", Convert.ToInt16(gb2312Bytes[1]) - 160)
                 ));
                }
                catch (Exception ex)
                {
                    return "z";
                }
            }
        }

        /// <summary>
        /// 根据区位得到首字母
        /// </summary>
        /// <param name="GBCode"></param>
        /// <returns></returns>
        public static String GetX(int GBCode)
        {
            if (GBCode >= 1601 && GBCode < 1637) return "A";
            if (GBCode >= 1637 && GBCode < 1833) return "B";
            if (GBCode >= 1833 && GBCode < 2078) return "C";
            if (GBCode >= 2078 && GBCode < 2274) return "D";
            if (GBCode >= 2274 && GBCode < 2302) return "E";
            if (GBCode >= 2302 && GBCode < 2433) return "F";
            if (GBCode >= 2433 && GBCode < 2594) return "G";
            if (GBCode >= 2594 && GBCode < 2787) return "H";
            if (GBCode >= 2787 && GBCode < 3106) return "J";
            if (GBCode >= 3106 && GBCode < 3212) return "K";
            if (GBCode >= 3212 && GBCode < 3472) return "L";
            if (GBCode >= 3472 && GBCode < 3635) return "M";
            if (GBCode >= 3635 && GBCode < 3722) return "N";
            if (GBCode >= 3722 && GBCode < 3730) return "O";
            if (GBCode >= 3730 && GBCode < 3858) return "P";
            if (GBCode >= 3858 && GBCode < 4027) return "Q";
            if (GBCode >= 4027 && GBCode < 4086) return "R";
            if (GBCode >= 4086 && GBCode < 4390) return "S";
            if (GBCode >= 4390 && GBCode < 4558) return "T";
            if (GBCode >= 4558 && GBCode < 4684) return "W";
            if (GBCode >= 4684 && GBCode < 4925) return "X";
            if (GBCode >= 4925 && GBCode < 5249) return "Y";
            if (GBCode >= 5249 && GBCode <= 5589) return "Z";
            if (GBCode >= 5601 && GBCode <= 8794)
            {
                String CodeData = "cjwgnspgcenegypbtwxzdxykygtpjnmjqmbsgzscyjsyyfpggbzgydywjkgaljswkbjqhyjwpdzlsgmr"
                 + "ybywwccgznkydgttngjeyekzydcjnmcylqlypyqbqrpzslwbdgkjfyxjwcltbncxjjjjcxdtqsqzycdxxhgckbphffss"
                 + "pybgmxjbbyglbhlssmzmpjhsojnghdzcdklgjhsgqzhxqgkezzwymcscjnyetxadzpmdssmzjjqjyzcjjfwqjbdzbjgd"
                 + "nzcbwhgxhqkmwfbpbqdtjjzkqhylcgxfptyjyyzpsjlfchmqshgmmxsxjpkdcmbbqbefsjwhwwgckpylqbgldlcctnma"
                 + "eddksjngkcsgxlhzaybdbtsdkdylhgymylcxpycjndqjwxqxfyyfjlejbzrwccqhqcsbzkymgplbmcrqcflnymyqmsqt"
                 + "rbcjthztqfrxchxmcjcjlxqgjmshzkbswxemdlckfsydsglycjjssjnqbjctyhbftdcyjdgwyghqfrxwckqkxebpdjpx"
                 + "jqsrmebwgjlbjslyysmdxlclqkxlhtjrjjmbjhxhwywcbhtrxxglhjhfbmgykldyxzpplggpmtcbbajjzyljtyanjgbj"
                 + "flqgdzyqcaxbkclecjsznslyzhlxlzcghbxzhznytdsbcjkdlzayffydlabbgqszkggldndnyskjshdlxxbcghxyggdj"
                 + "mmzngmmccgwzszxsjbznmlzdthcqydbdllscddnlkjyhjsycjlkohqasdhnhcsgaehdaashtcplcpqybsdmpjlpcjaql"
                 + "cdhjjasprchngjnlhlyyqyhwzpnccgwwmzffjqqqqxxaclbhkdjxdgmmydjxzllsygxgkjrywzwyclzmcsjzldbndcfc"
                 + "xyhlschycjqppqagmnyxpfrkssbjlyxyjjglnscmhcwwmnzjjlhmhchsyppttxrycsxbyhcsmxjsxnbwgpxxtaybgajc"
                 + "xlypdccwqocwkccsbnhcpdyznbcyytyckskybsqkkytqqxfcwchcwkelcqbsqyjqcclmthsywhmktlkjlychwheqjhtj"
                 + "hppqpqscfymmcmgbmhglgsllysdllljpchmjhwljcyhzjxhdxjlhxrswlwzjcbxmhzqxsdzpmgfcsglsdymjshxpjxom"
                 + "yqknmyblrthbcftpmgyxlchlhlzylxgsssscclsldclepbhshxyyfhbmgdfycnjqwlqhjjcywjztejjdhfblqxtqkwhd"
                 + "chqxagtlxljxmsljhdzkzjecxjcjnmbbjcsfywkbjzghysdcpqyrsljpclpwxsdwejbjcbcnaytmgmbapclyqbclzxcb"
                 + "nmsggfnzjjbzsfqyndxhpcqkzczwalsbccjxpozgwkybsgxfcfcdkhjbstlqfsgdslqwzkxtmhsbgzhjcrglyjbpmljs"
                 + "xlcjqqhzmjczydjwbmjklddpmjegxyhylxhlqyqhkycwcjmyhxnatjhyccxzpcqlbzwwwtwbqcmlbmynjcccxbbsnzzl"
                 + "jpljxyztzlgcldcklyrzzgqtgjhhgjljaxfgfjzslcfdqzlclgjdjcsnclljpjqdcclcjxmyzftsxgcgsbrzxjqqcczh"
                 + "gyjdjqqlzxjyldlbcyamcstylbdjbyregklzdzhldszchznwczcllwjqjjjkdgjcolbbzppglghtgzcygezmycnqcycy"
                 + "hbhgxkamtxyxnbskyzzgjzlqjdfcjxdygjqjjpmgwgjjjpkjsbgbmmcjssclpqpdxcdyykypcjddyygywchjrtgcnyql"
                 + "dkljczzgzccjgdyksgpzmdlcphnjafyzdjcnmwescsglbtzcgmsdllyxqsxsbljsbbsgghfjlwpmzjnlyywdqshzxtyy"
                 + "whmcyhywdbxbtlmswyyfsbjcbdxxlhjhfpsxzqhfzmqcztqcxzxrdkdjhnnyzqqfnqdmmgnydxmjgdhcdycbffallztd"
                 + "ltfkmxqzdngeqdbdczjdxbzgsqqddjcmbkxffxmkdmcsychzcmljdjynhprsjmkmpcklgdbqtfzswtfgglyplljzhgjj"
                 + "gypzltcsmcnbtjbhfkdhbyzgkpbbymtdlsxsbnpdkleycjnycdykzddhqgsdzsctarlltkzlgecllkjljjaqnbdggghf"
                 + "jtzqjsecshalqfmmgjnlyjbbtmlycxdcjpldlpcqdhsycbzsckbzmsljflhrbjsnbrgjhxpdgdjybzgdlgcsezgxlblg"
                 + "yxtwmabchecmwyjyzlljjshlgndjlslygkdzpzxjyyzlpcxszfgwyydlyhcljscmbjhblyjlycblydpdqysxktbytdkd"
                 + "xjypcnrjmfdjgklccjbctbjddbblblcdqrppxjcglzcshltoljnmdddlngkaqakgjgyhheznmshrphqqjchgmfprxcjg"
                 + "dychghlyrzqlcngjnzsqdkqjymszswlcfqjqxgbggxmdjwlmcrnfkkfsyyljbmqammmycctbshcptxxzzsmphfshmclm"
                 + "ldjfyqxsdyjdjjzzhqpdszglssjbckbxyqzjsgpsxjzqznqtbdkwxjkhhgflbcsmdldgdzdblzkycqnncsybzbfglzzx"
                 + "swmsccmqnjqsbdqsjtxxmbldxcclzshzcxrqjgjylxzfjphymzqqydfqjjlcznzjcdgzygcdxmzysctlkphtxhtlbjxj"
                 + "lxscdqccbbqjfqzfsltjbtkqbsxjjljchczdbzjdczjccprnlqcgpfczlclcxzdmxmphgsgzgszzqjxlwtjpfsyaslcj"
                 + "btckwcwmytcsjjljcqlwzmalbxyfbpnlschtgjwejjxxglljstgshjqlzfkcgnndszfdeqfhbsaqdgylbxmmygszldyd"
                 + "jmjjrgbjgkgdhgkblgkbdmbylxwcxyttybkmrjjzxqjbhlmhmjjzmqasldcyxyqdlqcafywyxqhz";
                String _gbcode = GBCode.ToString();
                int pos = (Convert.ToInt16(_gbcode.Substring(0, 2)) - 56) * 94 + Convert.ToInt16(_gbcode.Substring(_gbcode.Length - 2, 2));
                return CodeData.Substring(pos - 1, 1);
            }
            return " ";
        }

        #endregion --获取字符串的首字母

        public static int StringWidth(string str, int one, int both, int offset)
        {
            string temp;
            int x = 0;
            for (int i = 0; i < str.Length; i++)
            {
                temp = str.Substring(i, 1);
                if (Encoding.Default.GetByteCount(temp) == 1)
                {
                    x += one;
                }
                else
                {
                    x += both;
                }
            }
            return x + offset;
        }

        /// <summary>
        /// 十六进制文本转十六进制字节
        /// </summary>
        /// <param name="shex"></param>
        /// <returns></returns>
        public static byte[] GetByteArray(string shex)
        {
            string[] ssArray = shex.Split(' ');
            List<byte> bytList = new List<byte>();
            foreach (var s in ssArray)
            {
                //将十六进制的字符串转换成数值
                bytList.Add(Convert.ToByte(s, 16));
            }
            //返回字节数组
            return bytList.ToArray();
        }

        /// <summary>
        /// 取Image封装了什么格式图片
        /// </summary>
        /// <returns></returns>
        public static string getImageType(Image image)
        {
            string FormetType = string.Empty;
            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Tiff.Guid)
                FormetType = "TIFF";
            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid)
                FormetType = "GIF";
            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                FormetType = "JPG";
            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Bmp.Guid)
                FormetType = "BMP";
            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
                FormetType = "PNG";
            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Icon.Guid)
                FormetType = "ICO";
            else
                FormetType = "";
            return FormetType;
        }

        /// <summary>
        /// 取Image封装了什么格式图片
        /// </summary>
        /// <returns></returns>
        public static ImageFormat getImageF(Image image)
        {
            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Tiff.Guid)
                return ImageFormat.Tiff;
            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid)
                return ImageFormat.Gif;
            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                return ImageFormat.Jpeg;
            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Bmp.Guid)
                return ImageFormat.Bmp;
            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
                return ImageFormat.Png;
            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Icon.Guid)
                return ImageFormat.Icon;
            return ImageFormat.Png;
        }

        public static void log(string str)
        {
            FileStream fs = new FileStream(DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ":" + str);
            sw.Close();
            fs.Close();
        }

        public static void logWx(string str)
        {
            FileStream fs = new FileStream("wx" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ":" + str);
            sw.Close();
            fs.Close();
        }

        //===================2018-02===================
        public static void logFp(string str)
        {
            FileStream fs = new FileStream("fp" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ":" + str);
            sw.Close();
            fs.Close();
        }

        public static void FpLog(string qiHao, xztj xiaZhu, feiPanJieGuo fpjgData)
        {
            string sChengYuanXiaZhu = "";
            string sQiudao = "";

            #region 合计积分
            for (int i = 0; i < 5; i++)
            {
                sQiudao = "";
                //球道
                for (int x = 0; x < 10; x++)
                {
                    if (xiaZhu.QD[i, x] > 0 && fpjgData.QD[i, x] == true)
                    {
                        sQiudao += (i + 1) + "/" + x + "/" + xiaZhu.QD[i, x] + "、";
                    }
                }
                //大小单双
                for (int x = 0; x < 4; x++)
                {
                    if (xiaZhu.DXDS[i, x] > 0 && fpjgData.DXDS[i, x] == true)
                    {
                        string nam = "";

                        if (x == 0) nam = "大";
                        if (x == 1) nam = "小";
                        if (x == 2) nam = "单";
                        if (x == 3) nam = "双";

                        sQiudao += (i + 1) + "/" + nam + "/" + xiaZhu.DXDS[i, x] + "、";
                    }
                }

                if (sQiudao.Length > 0)
                {
                    sChengYuanXiaZhu += "\r\n" + sQiudao;
                }
            }


            sQiudao = "";
            for (int i = 0; i < 3; i++)//龙虎和
            {
                if (xiaZhu.LHH[i] > 0 && fpjgData.LHH[i] == true)
                {
                    string nam = "";
                    if (i == 0) nam = "龙";
                    if (i == 1) nam = "虎";
                    if (i == 2) nam = "和";

                    sChengYuanXiaZhu += nam + xiaZhu.LHH[i] + "、";
                }
            }
            if (sQiudao.Length > 0)
            {
                sChengYuanXiaZhu += "\r\n" + sQiudao;
            }


            #endregion
            //
            if (MainPlugin.frmMain != null && MainPlugin.frmMain._group != null)
            {
                foreach (GROUP jp in MainPlugin.frmMain._group.MemberList)
                {
                    if (jp.conter == "")
                    {
                        continue;
                    }

                    #region 合计成员积分
                    sChengYuanXiaZhu +="\r\n"+ jp.NickName +"  ";
                    for (int i = 0; i < 5; i++)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            if (jp.xiazhutongji.QD[i, x] > 0 && fpjgData.QD[i, x] == true)
                            {
                                sChengYuanXiaZhu += (i + 1) + "/" + x + "/" + xiaZhu.QD[i, x] + "、";
                            }
                        }
                    }
                    for (int i = 0; i < 5; i++)//大小单双
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            if (jp.xiazhutongji.DXDS[i, x] > 0 && fpjgData.DXDS[i, x] == true)
                            {
                                string nam = "";

                                if (x == 0) nam = "大";
                                if (x == 1) nam = "小";
                                if (x == 2) nam = "单";
                                if (x == 3) nam = "双";

                                sChengYuanXiaZhu += (i + 1) + "/" + nam + "/" + xiaZhu.DXDS[i, x] + "、";
                            }
                        }
                    }
                    for (int i = 0; i < 3; i++)//龙虎和
                    {
                        if (jp.xiazhutongji.LHH[i] > 0 && fpjgData.LHH[i] == true)
                        {
                            string nam = "";
                            if (i == 0) nam = "龙";
                            if (i == 1) nam = "虎";
                            if (i == 2) nam = "和";
                            sChengYuanXiaZhu += nam + xiaZhu.LHH[i] + "、";
                        }


                    }
                    sChengYuanXiaZhu += "\r\n";
                    #endregion
                }
            }
            //
            if (sChengYuanXiaZhu.Length > 0)
            {
                sChengYuanXiaZhu = "==============" + qiHao + "===============\r\n总提交成功\r\n" + sChengYuanXiaZhu;
                logFp(sChengYuanXiaZhu);
            }
        }

        public static string filtetStingSpecial(string str)
        {
            Regex regx = new Regex("[^0-9a-zA-Z\u4e00-\u9fa5]");
            return regx.Replace(str, ""); //汉字
        }

        //===================2018-02===================



    }
}