using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aldebaran.Proxies;
using System.Collections;
using System.Threading;
namespace NaoWindowsForms
{
    public class FaceDetection
    {
        private string ip;
        public FaceDetectionProxy fdp = null;
        public VideoDeviceProxy vdp = null;
        public MemoryProxy mp = null;
        public int period = 500;
        
        public FaceDetection(string ip)
        {
            this.ip = ip;
        }

        public void initializeProxies() 
        {
            fdp = new FaceDetectionProxy(this.ip, 9559);
            fdp.enableTracking(true);
            vdp = new VideoDeviceProxy(this.ip, 9559);
            
            mp = new MemoryProxy(this.ip, 9559);
        }

        public void Disconnect()
        {
            fdp.unsubscribe("Test_Face");
        }

        public void startDetecting()
        {
            fdp.subscribe("Test_Face", period, (float)0.0);
            fdp.enableTracking(true);
        }

        public void stopDetecting()
        {
            if (fdp != null)
            {
                try
                {
                    fdp.enableTracking(false);
                }
                catch(Exception e)
                {

                }
            }
        }

        public  List<float> getFacesInfo()
        {
            ArrayList faces = (ArrayList)mp.getData("FaceDetected");
            if (faces.Count >= 2)
            {
                string timeStamp = faces[0].ToString();
                ArrayList faceInfo1 = faces[1] as ArrayList;
                ArrayList faceInfo2 = faceInfo1[0] as ArrayList;//ova e toa sto ni treba
                ArrayList shapeInfo = faceInfo2[0] as ArrayList;
                List<float> faceOutput = new List<float>();

                float alpha = (float)shapeInfo[1];
                float beta = (float)shapeInfo[2];
                float sizeX = (float)shapeInfo[3];
                float sizeY = (float)shapeInfo[4];
                faceOutput.Add(alpha);
                faceOutput.Add(beta);
                faceOutput.Add(sizeX);
                faceOutput.Add(sizeY);
                /*
                Console.WriteLine("Info = {");
                Console.WriteLine(alpha);
                Console.WriteLine(beta);
                Console.WriteLine(sizeX);
                Console.WriteLine(sizeY);
                Console.WriteLine("}");
                 */

                return faceOutput;
            }
            else
            {
                //Console.WriteLine("No faces have been detected!");
                return null;
            }
        }


       /* public List<FaceInformation> getFacesInfo(int a)
        {
            //Thread.Sleep(2000);
            ArrayList val = (ArrayList)mp.getData("FaceDetected");
            //Thread.Sleep(500);
            if (val.Count >= 2)
            {
                List<FaceInformation> faceOutput = new List<FaceInformation>();
                string timeStamp = val[0].ToString();
                ArrayList faceInfoArray = val[1] as ArrayList;
                //ArrayList faceInfo2 = faceInfo1[0] as ArrayList;//ova e toa sto ni treba
                if (faceInfoArray.Count >= 2)
                {
                    Console.WriteLine("pose od 2");
                }
                foreach (ArrayList faceInfo in faceInfoArray)
                {
                    if (faceInfo.Count > 0)
                    {
                        ArrayList shapeInfo = faceInfo[0] as ArrayList;
                        //List<float> faceOutput = new List<float>();

                        if (shapeInfo != null)
                        {
                            float alpha = (float)shapeInfo[1];
                            float beta = (float)shapeInfo[2];
                            float sizeX = (float)shapeInfo[3];
                            float sizeY = (float)shapeInfo[4];
                            faceOutput.Add(new FaceInformation(alpha, beta, sizeX, sizeY));
                        }
                    }
                }
                /*
                Console.WriteLine("Info = {");
                Console.WriteLine(alpha);
                Console.WriteLine(beta);
                Console.WriteLine(sizeX);
                Console.WriteLine(sizeY);
                Console.WriteLine("}");
                 */

              /*  return faceOutput;
            }
            else
            {
                //Console.WriteLine("No faces have been detected!");
                return null;
            }
        }*/

    }
}
