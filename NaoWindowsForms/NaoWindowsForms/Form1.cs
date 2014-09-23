using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NaoWindowsForms
{
    public partial class Form1 : Form
    {

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton mouseButton;
        private System.Windows.Forms.ToolStripButton connectButton;
        private System.Windows.Forms.ToolStripTextBox textBox1;

        private string IP = null;
        private Bitmap bmp = null;
        private bool bitmapFlag = false;

        private System.Windows.Forms.Timer timer1 = null;

        private bool mouseEnabled = false;
        private bool connectEnabled = false;
        private bool isLocking = false;
        private Camera naoCam = null;
        private Motion naoMotion = null;
       
        private FaceDetection naoFaceDetection = null;

        private DataStorage storage = new DataStorage();
        private GetFrame newFrames;
        private Thread frameThread;
        private Thread faceThread;
        private System.Drawing.Point initialPosition;

        private System.Drawing.Point faceP1;
        private System.Drawing.Point faceP2;
        private System.Drawing.Pen pen;
        private Rectangle r = new Rectangle();
        private List<Rectangle> listRectangles = new List<Rectangle>();


        //variables
        private bool SPEECH_CONTROL = false;
        private bool isAiming = false;
        private const int COLOR_SPACE = 13;
        private const int FPS = 40;
        private bool isCamInitialized;
        private bool isPictureUpdating = false;
        private bool isFaceDetectionActive = false;
        private NaoCamImageFormat currentFormat;
        private NaoCamImageFormat HDFormat;
        private int time = 0;
        private bool areJointsSet = false;

        private float rollPosition = 0;
        private float shoulderPitchPosition = 0;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void initializeNaoComponents()
        {
            naoCam = new Camera();
            currentFormat = naoCam.NaoCamImageFormats[2];
            HDFormat = naoCam.NaoCamImageFormats[3];


            naoMotion = new Motion();
            naoFaceDetection = new FaceDetection(IP);

            initialPosition = new System.Drawing.Point(this.Width / 2, this.Height / 2);
            pen = new System.Drawing.Pen(System.Drawing.Brushes.Green);
            faceP1 = new System.Drawing.Point(-1, -1);
            faceP2 = new System.Drawing.Point(-1, -1);

            try
            {
                naoMotion.connect(IP);

                naoFaceDetection.initializeProxies();

                newFrames = new GetFrame(IP, currentFormat, COLOR_SPACE, FPS, naoCam, storage);
                frameThread = new Thread(new ThreadStart(newFrames.grabFrame));
                frameThread.Start();

                faceThread = new Thread(new ThreadStart(naoFaceDetection.startDetecting));
                faceThread.Start();

                isCamInitialized = true;
                textBox1.Enabled = false;
            }
            catch (Exception ex)
            {
                isCamInitialized = false;
                System.IO.File.WriteAllText(@"C:\\NAOcam\\exception.txt", ex.ToString());
            }
            timer1.Start();


        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            getDetectedFaces();
            //getListDetectedFaces();
            Invalidate(true);
        }

        private void mouseButton_Click(object sender, EventArgs e)
        {
            mouseEnabled = !mouseEnabled;
            if (mouseEnabled)
                mouseButton.Text = "Disable mouse";
            else
                mouseButton.Text = "Enable mouse";
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            connectEnabled = !connectEnabled;
            if (connectEnabled)
            {
                bitmapFlag = false;
                connectButton.Text = "Disconnect from NAO";
                mouseEnabled = false;
                //textBox1.Text = "";
                initializeNaoComponents();
            }
            else
            {
                bitmapFlag = true;
                this.Invalidate(true);
                try
                {
                    while (frameThread.IsAlive)
                    {
                        // stop the frame acquisition thread 
                        frameThread.Abort();
                    }
                    while (faceThread.IsAlive)
                    {
                        faceThread.Abort();
                    }
                }
                catch (Exception)
                { }
                
                
                
                // stop timer and disconnect camera 
                timer1.Stop();
                naoCam.Disconnect();
       
                naoFaceDetection.Disconnect();
                
                connectButton.Text = "Connect to NAO";
           
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength >= 1)
            {
                IP = textBox1.Text;
                connectButton.Enabled = true;
            }
            else
            {
                connectButton.Enabled = false;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (mouseEnabled)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (isAiming) naoMotion.shoot();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    isAiming = !isAiming;
                    if (isAiming)
                        naoMotion.aimDownTheSight();
                    else
                        naoMotion.setInitialArmsPosition();
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseEnabled)
            {
                System.Drawing.Point newPosition = e.Location;

                if (newPosition.X - initialPosition.X <= 0)
                {
                    if (rollPosition + (float)0.01 < Motion.degToRad((float)6.5))
                    {
                        rollPosition += (float)Math.Abs(newPosition.X - initialPosition.X) / ((float)this.Width) * Math.Abs(Motion.degToRad((float)70.0) - Motion.degToRad((float)3.5));
                    }
                    naoMotion.moveJoint(rollPosition, "RShoulderRoll");
                }
                else
                {
                    if (rollPosition - (float)0.01 > Motion.degToRad((float)-29.0))
                        rollPosition -= (float)Math.Abs(newPosition.X - initialPosition.X) / ((float)this.Width) * Math.Abs(Motion.degToRad((float)70.0) - Motion.degToRad((float)3.5));
                    naoMotion.moveJoint(rollPosition, "RShoulderRoll");
                }

                if (newPosition.Y - initialPosition.Y > 0)
                {
                    if (shoulderPitchPosition + (float)0.01 < 2.08)
                        shoulderPitchPosition += (float)Math.Abs(newPosition.Y - initialPosition.Y) / ((float)this.Height * 2) * Math.Abs((float)-2.08 - (float)2.08);
                    naoMotion.moveJoint(shoulderPitchPosition, "RShoulderPitch");
                }
                else if (newPosition.Y - initialPosition.Y < 0)
                {
                    if (shoulderPitchPosition - (float)0.01 > -2.08)
                        shoulderPitchPosition -= (float)Math.Abs(newPosition.Y - initialPosition.Y) / ((float)this.Height * 2) * Math.Abs((float)-2.08 - (float)2.08);
                    naoMotion.moveJoint(shoulderPitchPosition, "RShoulderPitch");
                }
      
                initialPosition = newPosition;
            }
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                naoMotion.walkForward();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.A)
            {
                naoMotion.walkForwardLeft();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.S)
            {
                naoMotion.walkBackward();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.D)
            {
                naoMotion.walkForwardRight();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.X)
            {
                if (!SPEECH_CONTROL)
                    naoMotion.standUp();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Z)
            {
                if (!SPEECH_CONTROL)
                    naoMotion.sitDown();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Q)
            {
                if (!SPEECH_CONTROL)
                    naoMotion.punchLeft();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.E)
            {
                if (!SPEECH_CONTROL)
                    naoMotion.punchRight();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F)
            {
                isFaceDetectionActive = !isFaceDetectionActive;
            }
            else if (e.KeyCode == Keys.G)
            {
                if (faceP1.X != -1 && faceP1.Y != -1 && faceP2.X != -1 && faceP2.Y != -1)
                {
                    mouseEnabled = false;
                    aimAtSight();
                }
            }
        }

        private void aimAtSight()
        {
           // if (!isLocking)
           // {
                
                isLocking = true;
                naoMotion.lockArms();
                Thread.Sleep(500);
                Point initPos = new Point(this.Width / 2 + 15, this.Height / 2 +15);
                Point rCentar = new Point(faceP1.X + r.Width / 2, faceP1.Y + r.Height / 2);

                double dx = Math.Abs(initPos.X - rCentar.X);
                double dy = Math.Abs(initPos.Y - rCentar.Y);
                double qr = (dx / (this.Width)) * 57;
                double qp = (dy / (this.Height)) * 40;
                float roll, pitch;
     
                naoMotion.naoMotion.stiffnessInterpolation("RShoulderPitch", (float)1.0f, 0.1f);
                if (rCentar.X - initPos.X <= 0)
                {
                    Console.WriteLine("levo na ekran");
                    float dJoint = (float)Motion.degToRad((float)qr);
                    float joint = naoMotion.getAngle("RShoulderRoll");
                    dJoint += joint;
                    roll = dJoint;
                    naoMotion.moveJoint(dJoint, "RShoulderRoll");
                    naoMotion.naoMotion.setAngles("RShoulderPitch", Motion.degToRad((float)-4.0), 0.3f); 
                    //Thread.Sleep(100);
                }
                else
                {
                    Console.WriteLine("desno na ekran");
                    float dJoint = (float)Motion.degToRad((float)qr);
                    float joint = naoMotion.getAngle("RShoulderRoll");
                    joint -= dJoint;
                    roll = joint;
                    naoMotion.moveJoint(joint, "RShoulderRoll");
                    //Thread.Sleep(100);
                }
                naoMotion.naoMotion.stiffnessInterpolation("RShoulderPitch", (float)1.0f,0.1f);
                naoMotion.naoMotion.stiffnessInterpolation("RShoulderRoll", (float)1.0f, 0.1f);
                if (rCentar.Y - initPos.Y > 0)
                {
                    float dJoint = Motion.degToRad((float)qp);
                    float joint = naoMotion.getAngle("RShoulderPitch");
                    joint += dJoint;
                    pitch = joint;
                    naoMotion.moveJoint(joint, "RShoulderPitch");
                    //Thread.Sleep(100);
                }
                else
                {
                    float dJoint = Motion.degToRad((float)qp);
                    float joint = naoMotion.getAngle("RShoulderPitch");
                    joint -= dJoint;
                    pitch = joint;
                    naoMotion.moveJoint(joint, "RShoulderPitch");
                    //Thread.Sleep(100);
                }

                naoMotion.moveJoint(roll, "RShoulderRoll");
                naoMotion.moveJoint(pitch, "RShoulderPitch");
                naoMotion.naoMotion.stiffnessInterpolation("RShoulderPitch", (float)1.0f, 0.1f);
                naoMotion.naoMotion.stiffnessInterpolation("RShoulderRoll", (float)1.0f, 0.1f);
                //Thread.Sleep(1000);
                //isLocking = false;
           // }
        }

        private void getDetectedFaces()
        {

                naoFaceDetection.startDetecting();
                Thread.Sleep(30);

                List<float> faceInfo = naoFaceDetection.getFacesInfo();
                if (faceInfo != null)
                {
          
                    float alpha = faceInfo[0];
                    float beta = faceInfo[1];
                    float sx = faceInfo[2];
                    float sy = faceInfo[3];

                    float sizeX = (float)this.Height * sx;
                    float sizeY = (float)this.Width * sy;

                    float x = (float)this.Width / 2 - this.Width * alpha;
                    float y = (float)this.Height / 2 + this.Height * beta;
                    System.Drawing.Point p1 = new System.Drawing.Point((int)(x - (sizeX / 2)), (int)(y - (sizeY / 2)));
                    System.Drawing.Point p2 = new System.Drawing.Point((int)(x + (sizeX / 2)), (int)(y + (sizeY / 2)));
                    if (p1.X < p2.X && p1.Y < p2.Y)
                    {
                        faceP1 = p1;
                        faceP2 = p2;
                    }
                    else
                    {
                        faceP1 = p2;
                        faceP2 = p1;
                    }
                }
                else
                {
                    faceP1.X = -1;
                    faceP1.Y = -1;
                    faceP2.Y = -1;
                    faceP2.X = -1;
                }
  
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (isCamInitialized && !isPictureUpdating)
            {
                isPictureUpdating = true;

                try
                {
            
                    byte[] imageBytes = storage.getBytes();

                    if (imageBytes != null)
                    {
      
                        BitmapSource bmps = BitmapSource.Create(currentFormat.width, currentFormat.height, 96, 96, PixelFormats.Bgr24, BitmapPalettes.WebPalette, imageBytes, currentFormat.width * 3);

                        using (var outStream = new MemoryStream())
                        {
                            BitmapEncoder enc = new BmpBitmapEncoder();
                            enc.Frames.Add(BitmapFrame.Create(bmps));
                            enc.Save(outStream);
                            bmp = new Bitmap(outStream);
                            Image outputImg = resizeImage(bmp, new Size(this.Width - 30, this.Height - 30));
                            bmp = new Bitmap(outputImg);
                            e.Graphics.DrawImage(bmp, 30, 30);
                        }
                        if (bmp != null)
                        {

                            if (faceP1.X != -1 && faceP1.Y != -1 && faceP2.X != -1 && faceP2.Y != -1)
                            {

                                r.X = faceP1.X;
                                r.Y = faceP1.Y;
                                r.Height = faceP2.Y - faceP1.Y;
                                r.Width = faceP2.X - faceP1.X;
                                
                                pen.Width = 4;
                                pen.Color = System.Drawing.Color.Yellow;
                                e.Graphics.DrawRectangle(pen, r);
                            }
                        }
                    

                    }
 
                }
                catch (Exception e1)
                {
                    // display error message and write exceptions to a file 
                    MessageBox.Show("Exception occurred, error log in C:\\NAOcam\\exception.txt");
                    System.IO.File.WriteAllText(@"C:\\NAOcam\\exception.txt",
                    e1.ToString());
                }
            }
            if(bitmapFlag)
            {
                e.Graphics.Clear(this.BackColor);
            }
            isPictureUpdating = false; // picture is updated 
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

    }
}
