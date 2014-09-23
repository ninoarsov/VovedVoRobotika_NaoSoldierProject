/** 
 * This software was developed by Austin Hughes 
 * Last Modified: 2013‐06‐11 
 */ 
 
using System; 
using System.Collections; 
using System.Collections.Generic; 
using System.Windows; 
using Aldebaran.Proxies; 
 
namespace NaoWindowsForms
{ 
    /// <summary> 
    /// Data structure to hold image format information 
    /// </summary> 
    public struct NaoCamImageFormat 
    { 
        public string name; 
        public int id; 
        public int width; 
        public int height; 
    } 
 
    public class Camera 
    { 
        private static VideoDeviceProxy naoCamera = null; 
 
        public List<NaoCamImageFormat> NaoCamImageFormats = new 
List<NaoCamImageFormat>(); 
 
        // class constructor 
        public Camera() 
        { 
            // set up image formats 
            NaoCamImageFormat format120 = new NaoCamImageFormat(); 
            NaoCamImageFormat format240 = new NaoCamImageFormat(); 
            NaoCamImageFormat format480 = new NaoCamImageFormat(); 
            NaoCamImageFormat format960 = new NaoCamImageFormat(); 
 
            format120.name = "160 * 120"; 
            format120.id = 0; 
            format120.width = 160; 
            format120.height = 120; 
 
            format240.name = "320 * 240"; 
            format240.id = 1; 
            format240.width = 320; 
            format240.height = 240; 
             format480.name = "640 * 480";
            format480.id = 2;
            format480.width = 640; 
            format480.height = 480; 
 
            format960.name = "1280 * 960"; 
            format960.id = 3; 
            format960.width = 1280; 
            format960.height = 960; 
 
            // add them to the formats list 
            NaoCamImageFormats.Add(format120); 
            NaoCamImageFormats.Add(format240); 
            NaoCamImageFormats.Add(format480); 
            NaoCamImageFormats.Add(format960); 
        } 
 
        /// <summary> 
        /// Connects to the camera on the NAO robot 
        /// </summary> 
        /// <param name="ip"> the ip address of the robot </param> 
        /// <param name="format"> the video format desired </param> 
        /// <param name="ColorSpace"> the video color space </param> 
        /// <param name="FPS"> the FPS of the video </param> 
        public void connect(string ip, NaoCamImageFormat format, int ColorSpace, 
int FPS) 
        { 
            try 
            { 
                if (naoCamera != null) 
                { 
                    Disconnect(); 
                } 
 
                naoCamera = new VideoDeviceProxy(ip, 9559);
 
                // Attempt to unsubscribe incase program was not shut down properly 
                try 
                { 
                    naoCamera.unsubscribe("NAO Camera"); 
                } 
                catch (Exception) 
                { 
                } 
 
                // subscribe to NAO Camera for easier access to camera memory 
                naoCamera.subscribe("NAO Camera", format.id, ColorSpace, FPS);
               // naoCamera.subscribe("NAO Camera", format.id, ColorSpace, FPS);
            } 
            catch (Exception e) 
            { 
                // display error message and write exceptions to a file 
               // MessageBox.Show("Exception occurred, error log in C:\\NAOcam\\exception.txt"); 
                naoCamera = null; 
                System.IO.File.WriteAllText(@"C:\\NAOcam\\exception.txt", 
e.ToString()); 
            }         } 
 
        /// <summary> 
        /// Disconnects from the NAO camera 
        /// </summary> 
        public void Disconnect() 
        { 
            try 
            { 
                if (naoCamera != null) 
                { 
                    // unsubscribe so the NAO knows we do not need data from the camera anymore 
                    naoCamera.unsubscribe("NAO Camera"); 
                } 
            } 
            catch 
            {  } 
 
            naoCamera = null; 
        } 
 
        /// <summary> 
        /// Gets an image from the camera 
        /// </summary> 
        /// <returns> single frame from the camera </returns>  
        public byte[] getImage() 
        { 
            byte[] image = new byte[0]; 
 
            try 
            { 
                if (naoCamera != null) 
                { 
                    Object imageObject = naoCamera.getImageRemote("NAO Camera"); 
                    image = (byte[])((ArrayList)imageObject)[6]; 
                } 
            } 
            catch (Exception) 
            { } 
            return image; 
        }

        public static List<float> getImgPosFromAngPos(List<float> angPos)
        {
            List<float> output = new List<float>();
            try
            {
                output = naoCamera.getImgPosFromAngPos(angPos);
            }
            catch
            {
                Console.WriteLine("not working");
               
            }
            return output;
        }

        public static List<float> getImgInfoFromAngInfo(List<float> angInfo)
        {
            List<float> output = new List<float>();
            try
            {
                output = naoCamera.getImgInfoFromAngInfo(angInfo);
            }
            catch
            {
                Console.WriteLine("not working");
            }
            return output;
        }
    } 
}