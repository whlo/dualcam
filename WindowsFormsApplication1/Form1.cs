using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Collections;

namespace DualCamCap
{
    public partial class Form1 : Form
    {

        IplImage frame1, frame2, frame3;
        bool finishFlag = false;

        public Form1()
        {
            InitializeComponent();
        }


        private void captureCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Bitmap cam1 = null;
            Bitmap cam2 = null;
            Bitmap cam3 = null;
            captureBmp(ref cam1, ref cam2, ref cam3);
        }

        private void savePictureBtn_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            if(captureCheckBox.Checked == true)
            {
                frame1.SaveImage("test.png");
                frame2.SaveImage("test2.png");
                frame3.SaveImage("test3.png");
            }
            else
            {
                MessageBox.Show("カメラがキャプチャモードになっていません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        //Webカメラ画像取得用メソッド
        private void captureBmp(ref Bitmap bitmap1, ref Bitmap bitmap2, ref Bitmap bitmap3)
        {
            //カメラのインデックス番号の取得
            //通常0から始まる連番のはず
            try
            {
                using (CvCapture capture1 = Cv.CreateCameraCapture(int.Parse(textBox1.Text)))
                using (CvCapture capture2 = Cv.CreateCameraCapture(int.Parse(textBox2.Text)))
                using (CvCapture capture3 = Cv.CreateCameraCapture(int.Parse(textBox3.Text)))
                {
                    //カメラ初期化で時間が掛かると例外エラーが出るので1秒停止する
                    System.Threading.Thread.Sleep(1000);

                    //リサイズ画像格納用変数
                    IplImage frameResized1 = Cv.CreateImage(Cv.Size(pictureBox1.Width, pictureBox1.Height), BitDepth.U8, 3);
                    IplImage frameResized2 = Cv.CreateImage(Cv.Size(pictureBox2.Width, pictureBox2.Height), BitDepth.U8, 3);
                    IplImage frameResized3 = Cv.CreateImage(Cv.Size(pictureBox3.Width, pictureBox3.Height), BitDepth.U8, 3);
                    //チェックボックスがONの時キャプチャし続ける
                    while (captureCheckBox.Checked == true && finishFlag == false)
                    {
                        //カメラ1のキャプチャからリサイズ、表示
                        frame1 = Cv.QueryFrame(capture1);
                        Cv.Resize(frame1, frameResized1, Interpolation.Lanczos4);
                        bitmap1 = BitmapConverter.ToBitmap(frameResized1);
                        pictureBox1.Image = bitmap1;
                        //カメラ2のキャプチャからリサイズ、表示
                        frame2 = Cv.QueryFrame(capture2);
                        Cv.Resize(frame2, frameResized2, Interpolation.Lanczos4);
                        bitmap2 = BitmapConverter.ToBitmap(frameResized2);
                        pictureBox2.Image = bitmap2;
                        //カメラ3のキャプチャからリサイズ、表示
                        if (capture3 == null)
                        {
                            label5.Text = "down";
                        }
                        else
                        {
                            frame3 = Cv.QueryFrame(capture3);
                            Cv.Resize(frame3, frameResized3, Interpolation.Lanczos4);
                            bitmap3 = BitmapConverter.ToBitmap(frameResized3);
                            pictureBox3.Image = bitmap3;
                        }
                        //画面更新
                        Application.DoEvents();
                    }
                    Cv.ReleaseImage(frame1);
                    Cv.ReleaseImage(frame2);
                    Cv.ReleaseImage(frame3);
                    Cv.ReleaseImage(frameResized1);
                    Cv.ReleaseImage(frameResized2);
                    Cv.ReleaseImage(frameResized3);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("何らかのエラーです。\n\nカメラが正しく接続され、\nIndex番号が正しく入力されているか確認して下さい。",
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            finishFlag = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CvCapture test = Cv.CreateCameraCapture(3);
            if (test == null)
            {
                label5.Text = "取得失敗";
            }
            else
            {
                IplImage test1 = Cv.QueryFrame(test);

            }
        }
    }
}
