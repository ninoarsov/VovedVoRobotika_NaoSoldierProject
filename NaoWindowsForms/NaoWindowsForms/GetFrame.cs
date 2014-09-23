using System; 
 
namespace NaoWindowsForms 
{ 
    /// <summary> 
    /// Gets frames from the NAO camera 
    /// </summary> 
    class GetFrame 
    { 
        // Classes  
        private Camera naoCam; 
        private DataStorage storage; 
 
        /// <summary> 
        /// Class constructor 
        /// </summary> 
        /// <param name="ip"> ip address of the robot </param> 
        /// <param name="format"> image format to use </param> 
        /// <param name="colorSpace"> color space to use </param> 
        /// <param name="FPS"> FPS to use </param> 
        /// <param name="currentCamera"> instance of the camera class to use 
        ///</param> 
        /// <param name="currentStorage"> instance of the DataStorage class to use 
        ///</param> 
        public GetFrame(string ip, NaoCamImageFormat format, int colorSpace, int 
FPS, Camera currentCamera, DataStorage currentStorage) 
        { 
            naoCam = currentCamera; 
            storage = currentStorage; 
 
            naoCam.connect(ip, format, colorSpace, FPS); 
        } 
 
        /// <summary> 
        /// Gets new frames from the camera and stores them 
        /// in the data storage class 
        /// </summary> 
        public void grabFrame() 
        { 
            while (true) 
            { 
                try 
                { 
                    byte[] bytes = naoCam.getImage();  
                    storage.setBytes(bytes); 
 
                    System.Threading.Thread.Sleep(1000 / 30); 
                } 
                catch (Exception) 
                { } 
            } 
        } 
    } 
 
    /// <summary> 
    /// Class to enable sharing of data between threads 
    /// </summary> 
    public class DataStorage 
    { 
        private byte[] bytes; 
 
        public void setBytes(byte[] bytes1) 
        { 
            bytes = bytes1; 
        } 
 
        public byte[] getBytes() 
        { 
            return bytes; 
        } 
    } 
}
