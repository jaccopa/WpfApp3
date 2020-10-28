using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfControls.Helper
{
    public class ImageHelper
    {
        public static readonly ImageHelper Instance = new ImageHelper();

        private ImageHelper() { }

        public System.Drawing.Bitmap ImageSourceToBitmap(ImageSource imageSource)
        {
            BitmapSource m = (BitmapSource)imageSource;

            if (m == null)
                return null;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m.PixelWidth, m.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb); // 坑点：选Format32bppRgb将不带透明度

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
            new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            m.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);

            return bmp;
        }


        public ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
              hBitmap,
              IntPtr.Zero,
              Int32Rect.Empty,
              BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(hBitmap);
            return wpfBitmap;
        }


        #region    图片转数组

        public byte[] ImageToArrayFast(Bitmap image)
        {
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            BitmapData dstBmData = image.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.IntPtr dstPtr = dstBmData.Scan0;
            int dst_bytes = dstBmData.Stride * image.Height;
            byte[] dstValues = new byte[dst_bytes];
            System.Runtime.InteropServices.Marshal.Copy(dstPtr, dstValues, 0, dst_bytes);
            image.UnlockBits(dstBmData);
            return dstValues;
        }

        public byte[] ImageToArray(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            byte[] bs = new byte[width * height * 4];
            int index = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    System.Drawing.Color pixelColor = image.GetPixel(i, j);//获得某个x y 的像素值
                    bs[index++] = pixelColor.A;
                    bs[index++] = pixelColor.R;
                    bs[index++] = pixelColor.G;
                    bs[index++] = pixelColor.B;
                    //bs[index++] = pixelColor.B;
                    //bs[index++] = pixelColor.G;
                    //bs[index++] = pixelColor.R;
                    //bs[index++] = pixelColor.A;
                }
            }

            return bs;
        }

        public byte[] ImageSourceToArray(ImageSource imageSource)
        {
            int width = (int)imageSource.Width;
            int height = (int)imageSource.Height;

            int size = width * height * 4;
            byte[] argb = new byte[size];
            Bitmap result = new Bitmap(width, height);

            BitmapData bits = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //imageSource.CopyPixels(argb, bits.Stride, 0);
            //imageSource.
            result.Dispose();

            return argb;
        }

        public byte[] DualMode(int width, int height, byte[] datas)
        {
            Bitmap bpTemp = ArrayToIamge(width, height, datas);

            int startDot = (int)CLanmage.StartLevelBaseDots;

            Bitmap bpTemp2 = bpTemp.Clone(new Rectangle(new System.Drawing.Point(width / 4, 0), new System.Drawing.Size(width / 2, height)), bpTemp.PixelFormat);

            Bitmap bpRes = new Bitmap(width, height);

            if (bpRes == null)
            {
                return null;
            }

            Graphics g1 = Graphics.FromImage(bpRes);

            g1.DrawImage(bpTemp2, new PointF(0 / 2, 0));
            g1.DrawImage(bpTemp2, new PointF(width / 2, 0));

            Bitmap bp = bpRes.Clone(new Rectangle(0, 0, width, height), bpTemp.PixelFormat);

            bpTemp.Dispose();
            bpTemp2.Dispose();
            bpRes.Dispose();

            if (bp == null)
            {
                return null;
            }

            Graphics g = Graphics.FromImage(bp);

            System.Drawing.Pen curPen = new System.Drawing.Pen(System.Drawing.Brushes.White, 1);

            for (int i = 0; i < CLanmage.CurrentDepth; i++)
            {
                int y = startDot + i * (int)(10 / CLanmage.LengthPerDotY);

                int x2 = 10 + 600;

                if ((i + 1) % 5 == 0)
                    x2 = 20 + 600;

                g.DrawLine(curPen, new System.Drawing.Point(600, y), new System.Drawing.Point(x2, y));
            }

            byte[] outData = ImageToArrayFast(bp);
            curPen.Dispose();
            g.Dispose();
            bp.Dispose();
            return outData;
        }

        public byte[] AddRulerMark(int width, int height, byte[] datas)
        {
            Bitmap bp = ArrayToIamge(width, height, datas);
            int startDot = (int)CLanmage.StartLevelBaseDots;

            if (bp == null)
                return null;

            Graphics g = Graphics.FromImage(bp);
            System.Drawing.Pen curPen = new System.Drawing.Pen(System.Drawing.Brushes.White, 1);

            for (int i = 0; i < CLanmage.CurrentDepth; i++)
            {
                int y = startDot + i * (int)(10 / CLanmage.LengthPerDotY);

                int x2 = 10 + 600;

                if ((i + 1) % 5 == 0)
                    x2 = 20 + 600;

                g.DrawLine(curPen, new System.Drawing.Point(600, y), new System.Drawing.Point(x2, y));
            }

            byte[] outData = ImageToArrayFast(bp);
            curPen.Dispose();
            g.Dispose();
            bp.Dispose();
            return outData;
        }

        public byte[] AddRulerMarkAndFocus(int width, int height, byte[] datas)
        {
            Bitmap bp = ArrayToIamge(width, height, datas);
            int startDot = (int)CLanmage.StartLevelBaseDots;

            if (bp == null)
                return null;

            Graphics g = Graphics.FromImage(bp);
            System.Drawing.Pen curPen = new System.Drawing.Pen(System.Drawing.Brushes.White, 1);

            for (int i = 0; i <= CLanmage.CurrentDepth; i++)
            {
                int y = startDot + (int)((10 * i) / CLanmage.LengthPerDotY);

                int x2 = 10 + 600;

                if ((i + 0) % 5 == 0)
                    x2 = 20 + 600;

                g.DrawLine(curPen, new System.Drawing.Point(600, y), new System.Drawing.Point(x2, y));
            }

            //draw focus lines
            using (SolidBrush b = new SolidBrush(System.Drawing.Color.White))
            {
                int y = startDot + (int)(CLanmage.FocusPos / CLanmage.LengthPerDotY);

                System.Drawing.Point point1 = new System.Drawing.Point(600, y);

                System.Drawing.Point point2 = new System.Drawing.Point(600 + 10, y - 5);

                System.Drawing.Point point3 = new System.Drawing.Point(600 + 10, y + 5);

                GraphicsPath gp = new GraphicsPath(FillMode.Winding);

                gp.AddCurve(new System.Drawing.Point[] { point1, point2, point3 });

                g.FillPath(b, gp);

                gp.Dispose();
            }

            byte[] outData = ImageToArrayFast(bp);
            curPen.Dispose();
            g.Dispose();
            bp.Dispose();
            return outData;
        }

        Bitmap bpHalf = null;
        public byte[] AddRulerMarkAndFocusAndDual(int width, int height, byte[] datas)
        {
            Bitmap bp;
            if (CLanmage.DB)
            {
                Bitmap bpTemp = ArrayToIamge(width, height, datas);

                if (bpTemp == null)
                {
                    return null;
                }

                Bitmap bpTemp2 = bpTemp.Clone(new Rectangle(new System.Drawing.Point(width / 4, 0), new System.Drawing.Size(width / 2, height)), bpTemp.PixelFormat);

                if (bpTemp2 == null)
                {
                    return null;
                }

                Bitmap bpRes = new Bitmap(width, height);

                if (bpRes == null)
                {
                    return null;
                }

                Graphics g1 = Graphics.FromImage(bpRes);

                if (CLanmage.DBCount == 1)
                {
                    bpHalf?.Dispose();
                    bpHalf = bpTemp2.Clone(new Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(bpTemp2.Width, bpTemp2.Height)), bpTemp2.PixelFormat);
                    CLanmage.DBCount = 0;
                }

                if (bpHalf != null)
                {
                    g1.DrawImage(bpHalf, new PointF(0 / 2, 0));
                }

                g1.DrawImage(bpTemp2, new PointF(width / 2, 0));

                bp = bpRes.Clone(new Rectangle(0, 0, width, height), bpTemp.PixelFormat);

                bpTemp?.Dispose();
                bpTemp2?.Dispose();
                bpRes?.Dispose();
            }
            else
            {
                bp = ArrayToIamge(width, height, datas);
            }

            int startDot = (int)CLanmage.StartLevelBaseDots;

            if (bp == null)
            {
                return null;
            }

            Graphics g = Graphics.FromImage(bp);
            using (System.Drawing.Pen curPen = new System.Drawing.Pen(System.Drawing.Brushes.White, 1))
            {
                for (int i = 0; i <= CLanmage.CurrentDepth; i++)
                {
                    int y = startDot + (int)((10 * i) / CLanmage.LengthPerDotY);

                    int x2 = 10 + 600;

                    if ((i + 0) % 5 == 0)
                        x2 = 20 + 600;

                    g.DrawLine(curPen, new System.Drawing.Point(600, y), new System.Drawing.Point(x2, y));
                }
            }

            //add cross red
            //if
            //(
            //        CStatusQToView.machineStatus.mainState == ControlMainState.MainState_Remedy &&
            //        (CStatusQToView.machineStatus.remedyState < ControlRemedyState.RemedyState_PredictPower ||
            //        CStatusQToView.machineStatus.remedyState > ControlRemedyState.RemedyState_Remedy)
            //)
            //{
                using (System.Drawing.Pen crossRedPen = new System.Drawing.Pen(System.Drawing.Color.White, 1))
                {
                    System.Drawing.Point crossPt;
                    if (CLanmage.DB)
                        crossPt = new System.Drawing.Point(width / 2 + width / 4, (int)((CTargetPara.BaseTarget + CLanmage.StartLevelBaseLen) / CLanmage.LengthPerDotY));
                    else
                        crossPt = new System.Drawing.Point(width / 2, (int)((CTargetPara.BaseTarget + CLanmage.StartLevelBaseLen) / CLanmage.LengthPerDotY));

                    g.DrawLine(crossRedPen, new System.Drawing.Point(crossPt.X - 5, crossPt.Y), new System.Drawing.Point(crossPt.X + 5, crossPt.Y));
                    g.DrawLine(crossRedPen, new System.Drawing.Point(crossPt.X, crossPt.Y - 5), new System.Drawing.Point(crossPt.X, crossPt.Y + 5));
                }
            //}
            //else if
            //(
            //        CStatusQToView.machineStatus.mainState == ControlMainState.MainState_Remedy &&
            //        (CStatusQToView.machineStatus.remedyState == ControlRemedyState.RemedyState_PredictPower ||
            //        CStatusQToView.machineStatus.remedyState == ControlRemedyState.RemedyState_Remedy)
            //)
            //{
            //    using (System.Drawing.Pen crossRedPen = new System.Drawing.Pen(System.Drawing.Color.White, 1))
            //    {
            //        System.Drawing.Point crossPt;
            //        if (CLanmage.DB)
            //            crossPt = new System.Drawing.Point(width / 2 + width / 4, (int)((CControlCenter.cappos + CLanmage.StartLevelBaseLen) / CLanmage.LengthPerDotY));
            //        else
            //            crossPt = new System.Drawing.Point(width / 2, (int)((CControlCenter.cappos + CLanmage.StartLevelBaseLen) / CLanmage.LengthPerDotY));

            //        g.DrawLine(crossRedPen, new System.Drawing.Point(crossPt.X - 5, crossPt.Y), new System.Drawing.Point(crossPt.X + 5, crossPt.Y));
            //        g.DrawLine(crossRedPen, new System.Drawing.Point(crossPt.X, crossPt.Y - 5), new System.Drawing.Point(crossPt.X, crossPt.Y + 5));
            //    }
            //}


            //add paras
            using (System.Drawing.Pen curPen = new System.Drawing.Pen(System.Drawing.Brushes.White, 1))
            {
                string _FontName = "Arial";
                int _FontSize = 10;
                Font stringFont = new Font(_FontName, _FontSize, System.Drawing.FontStyle.Bold);
                g.DrawString("Probe", stringFont, System.Drawing.Brushes.White, 500, 0);
                g.DrawString(CLanmage.ProbeIDName, stringFont, System.Drawing.Brushes.White, 545, 0);

                g.DrawString("Mode", stringFont, System.Drawing.Brushes.White, 500, 15);
                g.DrawString(CLanmage.sonicMode.ToString(), stringFont, System.Drawing.Brushes.White, 545, 15);

                g.DrawString("Power", stringFont, System.Drawing.Brushes.White, 500, 30);
                g.DrawString(CLanmage.Power.ToString() + "%", stringFont, System.Drawing.Brushes.White, 545, 30);

                g.DrawString("Gain", stringFont, System.Drawing.Brushes.White, 500, 45);
                g.DrawString(CLanmage.Gn.ToString(), stringFont, System.Drawing.Brushes.White, 545, 45);

                g.DrawString("Freq", stringFont, System.Drawing.Brushes.White, 500, 60);
                string Fre;
                if (CLanmage.IsThi)
                {
                    Fre = "H" + CLanmage.Freq.ToString() + "MHz";
                }
                else
                {
                    Fre = CLanmage.Freq.ToString() + "MHz";
                }
                g.DrawString(Fre, stringFont, System.Drawing.Brushes.White, 545, 60);


                g.DrawString("MI", stringFont, System.Drawing.Brushes.White, 500, 75);
                g.DrawString(CLanmage.Mi.ToString("#0.0"), stringFont, System.Drawing.Brushes.White, 545, 75);

                g.DrawString("TIS", stringFont, System.Drawing.Brushes.White, 500, 90);
                g.DrawString(CLanmage.Tis.ToString("#0.0"), stringFont, System.Drawing.Brushes.White, 545, 90);

                g.DrawString("Depth", stringFont, System.Drawing.Brushes.White, 500, 105);
                g.DrawString(CLanmage.CurrentDepth.ToString() + "cm", stringFont, System.Drawing.Brushes.White, 545, 105);

                //if (CLanmage.SonicStatusOfFreeze == SonicStatusOfFreeze.UnFreeze)
                {
                    g.DrawString("FPS", stringFont, System.Drawing.Brushes.White, 500, 450);

                    if (CLanmage.SonicStatusOfFreeze == SonicStatusOfFreeze.UnFreeze)
                    {
                        g.DrawString(CLanmage.FPS.ToString(), stringFont, System.Drawing.Brushes.White, 545, 450);
                        g.DrawString(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), stringFont, System.Drawing.Brushes.White, 500, 465);
                    }
                    else
                    {
                        g.DrawString(CLanmage.FPSFreeze.ToString(), stringFont, System.Drawing.Brushes.White, 545, 450);
                        g.DrawString(CLanmage.DateTimeFreeze.ToString("yyyy/MM/dd HH:mm:ss"), stringFont, System.Drawing.Brushes.White, 500, 465);
                    }
                }
            }

            //draw focus lines
            using (SolidBrush b = new SolidBrush(System.Drawing.Color.White))
            {
                int y = startDot + (int)(CLanmage.FocusPos / CLanmage.LengthPerDotY);

                System.Drawing.Point point1 = new System.Drawing.Point(600, y);

                System.Drawing.Point point2 = new System.Drawing.Point(600 + 10, y - 5);

                System.Drawing.Point point3 = new System.Drawing.Point(600 + 10, y + 5);

                GraphicsPath gp = new GraphicsPath(FillMode.Winding);

                gp.AddCurve(new System.Drawing.Point[] { point1, point2, point3 });

                g.FillPath(b, gp);

                gp.Dispose();
            }

            //add white line in zero
            {
                //if (MasterWindow.curUC == UCType.Type_PreRemedyPlan)
                //{
                //    System.Drawing.Color color = System.Drawing.Color.FromArgb(255, 255, 255, 128);
                //    using (System.Drawing.Pen crossRedPen = new System.Drawing.Pen(color, 1))
                //    {
                //        g.DrawLine(crossRedPen, new System.Drawing.Point(320, 0), new System.Drawing.Point(320, (int)((CTargetPara.BaseTarget + CLanmage.StartLevelBaseLen + (CStatusQToView.PeriodDataInfo.ZStartPosSetting + 20)) / CLanmage.LengthPerDotY)));
                //    }
                //}
            }

            if (bp == null)
                return null;

            byte[] outData = ImageToArrayFast(bp);
            //curPen.Dispose();
            g?.Dispose();
            bp?.Dispose();
            return outData;
        }

        public Bitmap ArrayToIamge(int width, int height, byte[] datas)
        {
            try
            {
                var bitmap = new Bitmap(width, height);
                Rectangle rect = new Rectangle(0, 0, width, height);
                BitmapData dstBmData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                System.IntPtr dstPtr = dstBmData.Scan0;
                int dst_bytes = dstBmData.Stride * height;
                byte[] dstValues = new byte[dst_bytes];
                int bIndex = 0;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        byte b = datas[bIndex++];
                        byte g = datas[bIndex++];
                        byte r = datas[bIndex++];
                        byte a = datas[bIndex++];

                        dstValues[i * dstBmData.Stride + j * 4 + 0] = a;
                        dstValues[i * dstBmData.Stride + j * 4 + 1] = r;
                        dstValues[i * dstBmData.Stride + j * 4 + 2] = g;
                        dstValues[i * dstBmData.Stride + j * 4 + 3] = b;
                    }
                }

                System.Runtime.InteropServices.Marshal.Copy(dstValues, 0, dstPtr, dst_bytes);
                bitmap.UnlockBits(dstBmData);
                return bitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                //log4net.LogManager.GetLogger("loginfo").Info("ImageHelper :: public Bitmap ArrayToIamge(int width, int height, byte[] datas)::" + ex.ToString());

                return null;
            }
        }

        //public Bitmap ArrayToIamge(int width, int height, byte[] datas)
        //{
        //    var bitmap = new Bitmap(width, height);

        //    int pCount = datas.Length / 4;
        //    int bIndex = 0;

        //    for (int y = 0; y < height; y++)
        //    {
        //        for (int x = 0; x < width; x++)
        //        {
        //            if (y * x > pCount)
        //            {
        //                break;
        //            }
        //            //int a = datas[bIndex++];
        //            //int r = datas[bIndex++];
        //            //int g = datas[bIndex++];
        //            //int b = datas[bIndex++];
        //            int b = datas[bIndex++];
        //            int g = datas[bIndex++];
        //            int r = datas[bIndex++];
        //            int a = datas[bIndex++];
        //            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(a, r, g, b));
        //        }
        //    }
        //    return bitmap;
        //}

        public Bitmap ArrayToImageLock(int width, int height, byte[] datas)
        {
            var bitmap = new Bitmap(width, height);
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData dstBmData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.IntPtr dstPtr = dstBmData.Scan0;
            int dst_bytes = dstBmData.Stride * height;
            byte[] dstValues = new byte[dst_bytes];
            int bIndex = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    byte b = datas[bIndex++];
                    byte g = datas[bIndex++];
                    byte r = datas[bIndex++];
                    byte a = datas[bIndex++];
                    dstValues[i * dstBmData.Stride + j * 4 + 0] = a;
                    dstValues[i * dstBmData.Stride + j * 4 + 1] = r;
                    dstValues[i * dstBmData.Stride + j * 4 + 2] = g;
                    dstValues[i * dstBmData.Stride + j * 4 + 3] = b;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(dstValues, 0, dstPtr, dst_bytes);
            bitmap.UnlockBits(dstBmData);
            return bitmap;
        }


        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public BitmapSource BitmapToBitmapSource(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                IntPtr ip = bitmap.GetHbitmap();//从GDI+ Bitmap创建GDI位图对象

                BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ip);

                return bitmapSource;
            }
            else
            {
                return null;
            }
        }

        public Bitmap BitmapSourceToBitmap(BitmapSource bitmapSource)
        {
            BitmapSource m = bitmapSource;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m.PixelWidth, m.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
            new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            m.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        public BitmapSource ArrayToBitmapSource(int width, int height, byte[] datas)
        {
            return BitmapToBitmapSource(ArrayToIamge(width, height, datas));
        }

        #endregion

        #region 控件转图片

        /// <summary>
        /// 将控件保存为BitmapSource
        /// </summary>
        /// <returns></returns>
        public BitmapSource SaveAsBitmapSource(FrameworkElement el)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)el.ActualWidth, (int)el.ActualHeight, 96, 96, PixelFormats.Default);
            bmp.Render(el);
            return bmp;
        }

        /// <summary>
        /// 将控件保存为byte
        /// </summary>
        /// <returns></returns>
        public byte[] SaveAsImageArray(FrameworkElement el)
        {
            var bitmapSource = SaveAsBitmapSource(el);
            return ImageHelper.Instance.ImageToArray(ImageHelper.Instance.BitmapSourceToBitmap(bitmapSource));
        }

        public BitmapSource GetScreenSnapshot()
        {
            try
            {
                System.Drawing.Rectangle rc = System.Windows.Forms.SystemInformation.VirtualScreen;

                var bitmap = new Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                using (Graphics memoryGrahics = Graphics.FromImage(bitmap))
                {
                    memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
                }

                return BitmapToBitmapSource(bitmap);
            }

            catch (Exception)
            {
                return null;
            }
            return null;
        }

        public BitmapSource GetFromScreenSnapshot(System.Drawing.Rectangle rect)
        {
            try
            {
                System.Drawing.Rectangle rc = System.Windows.Forms.SystemInformation.VirtualScreen;

                var bitmap = new Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                if (bitmap == null)
                    return null;

                using (Graphics memoryGrahics = Graphics.FromImage(bitmap))
                {
                    memoryGrahics.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
                }

                return BitmapToBitmapSource(bitmap);
            }

            catch (Exception)
            {
                return null;
            }
            return null;
        }

        public BitmapSource GetFromScreenSnapshot(FrameworkElement el, FrameworkElement window)
        {
            try
            {
                System.Windows.Point point = el.TransformToAncestor(window).Transform(new System.Windows.Point(0, 0));

                System.Drawing.Size size = new System.Drawing.Size((int)el.Width, (int)el.Height);

                System.Drawing.Rectangle rc = System.Windows.Forms.SystemInformation.VirtualScreen;

                var bitmap = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                if (bitmap == null)
                    return null;

                using (Graphics memoryGrahics = Graphics.FromImage(bitmap))
                {
                    memoryGrahics.CopyFromScreen((int)point.X, (int)point.Y, 0, 0, size, CopyPixelOperation.SourceCopy);
                }

                return BitmapToBitmapSource(bitmap);
            }

            catch (Exception)
            {
                return null;
            }
            return null;
        }

        public BitmapSource GetFromScreenSnapshot(FrameworkElement el, FrameworkElement window, out Bitmap _bitmap)
        {
            try
            {

                System.Windows.Point point = el.TransformToAncestor(window).Transform(new System.Windows.Point(0, 0));

                System.Drawing.Size size = new System.Drawing.Size((int)el.Width, (int)el.Height);

                System.Drawing.Rectangle rc = System.Windows.Forms.SystemInformation.VirtualScreen;

                var bitmap = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                if (bitmap == null)
                {
                    _bitmap = null;

                    return null;
                }


                using (Graphics memoryGrahics = Graphics.FromImage(bitmap))
                {
                    memoryGrahics.CopyFromScreen((int)point.X, (int)point.Y, 0, 0, size, CopyPixelOperation.SourceCopy);

                    _bitmap = bitmap;
                }

                return BitmapToBitmapSource(bitmap);
            }
            catch (Exception)
            {
                _bitmap = null;
            }

            return null;
        }

        #endregion
    }
}
