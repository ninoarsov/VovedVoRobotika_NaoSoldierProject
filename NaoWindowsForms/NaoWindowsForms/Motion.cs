using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows;
using Aldebaran.Proxies;
using System.Threading;
namespace NaoWindowsForms
{ 
    class Motion
    {
        public MotionProxy naoMotion = null;
        BehaviorManagerProxy bmp = null;

        /// <summary> 
        /// Connects to the motion system in the NAO robot 
        /// </summary> 
        /// <param name="ip"> ip address of the robot </param> 
        /// 

       
        public void connect(string ip)
        {
            try
            {
                naoMotion = new MotionProxy(ip, 9559);
                bmp = new BehaviorManagerProxy(ip, 9559);

                // give joints stiffness 
                naoMotion.stiffnessInterpolation("Head", 1.0f, 1.0f);
                naoMotion.stiffnessInterpolation("LArm", 1.0f, 1.0f);
                naoMotion.stiffnessInterpolation("RArm", 1.0f, 1.0f);
            }
            catch (Exception e)
            {
                //MessageBox.Show("Exception occurred, error log in C:\\NAOcam\\exception.txt");
                System.IO.File.WriteAllText(@"C:\\NAOcam\\exception.txt", e.ToString()); // write exepctions to text file 
            }
        }

      

        public void walkForward()
        {
            bmp.runBehavior("WalkForward");
        }

        public void walkBackward()
        {
            bmp.runBehavior("WalkBackward");
        }

        public void walkForwardLeft()
        {
            bmp.runBehavior("WalkForwardRight");
        }

        public void walkForwardRight()
        {
            bmp.runBehavior("WalkForwardLeft");
        }

        public void sitDown()
        {
            bmp.runBehavior("SitDown");
        }

        public void standUp()
        {
            bmp.runBehavior("StandUp");
        }

        public void setInitialArmsPosition()
        {
            naoMotion.setAngles("LShoulderPitch", degToRad((float)70.0), 0.3f);
            //naoMotion.setAngles("LWristYaw", degToRad((float)0.0), 0.3f);
            naoMotion.setAngles("LShoulderRoll", degToRad((float)4.0), 0.3f);
            naoMotion.setAngles("LElbowRoll", degToRad((float)-2.8), 0.3f);

            naoMotion.setAngles("RShoulderPitch", degToRad((float)70.0), 0.3f);
            //naoMotion.setAngles("RWristYaw", degToRad((float)0.0), 0.3f);
            naoMotion.setAngles("RShoulderRoll", degToRad((float)-4.0), 0.3f);
            naoMotion.setAngles("RElbowRoll", degToRad((float)2.8), 0.3f);
        }

        public void lockArms()
        {
            naoMotion.setAngles("HeadPitch", degToRad((float)0.5), 0.1f);
            naoMotion.setAngles("HeadYaw", (float)0.0, 0.1f);
            naoMotion.setAngles("RElbowRoll", degToRad((float)23.0), 0.1f);
            naoMotion.setAngles("RElbowYaw", degToRad((float)9.3), 0.1f);
            naoMotion.setAngles("RShoulderRoll", degToRad((float)-10.5), 0.3f);
            naoMotion.setAngles("RShoulderPitch", degToRad((float)-4.0), 0.3f);
        }
    

        public void punchLeft()
        {
            //naoMotion.setAngles("LWristYaw", degToRad((float)0.0), 1.0f);
            naoMotion.setAngles("LElbowYaw", degToRad((float)-90.0), 1.0f);
            Thread.Sleep(500);

            //naoMotion.setAngles("LWristYaw", degToRad((float)-90.0), 1.0f);
            naoMotion.setAngles("LShoulderPitch", degToRad((float)38.7), 1.0f);
            naoMotion.setAngles("LShoulderRoll", degToRad((float)40.0), 1.0f);
            naoMotion.setAngles("LElbowRoll", degToRad((float)-89.0), 1.0f);
            Thread.Sleep(500);

            //naoMotion.setAngles("LWristYaw", degToRad((float)-90.0), 1.0f);
            naoMotion.setAngles("LShoulderPitch", degToRad((float)2.5), 1.0f);
            naoMotion.setAngles("LShoulderRoll", degToRad((float)90.0), 1.0f);
            naoMotion.setAngles("LElbowRoll", degToRad((float)-89.0), 1.0f);
            naoMotion.setAngles("LElbowYaw", degToRad((float)-20.0), 1.0f);
            Thread.Sleep(500);

            //naoMotion.setAngles("LWristYaw", degToRad((float)0.0), 1.0f);
            naoMotion.setAngles("LShoulderRoll", degToRad((float)5.6), 1.0f);
            naoMotion.setAngles("LElbowRoll", degToRad((float)-1.0), 1.0f);
            Thread.Sleep(1000);

            naoMotion.setAngles("LShoulderPitch", degToRad((float)70.0), 1.0f);
            //naoMotion.setAngles("LWristYaw", degToRad((float)0.0), 1.0f);
            naoMotion.setAngles("LShoulderRoll", degToRad((float)4.0), 1.0f);
            naoMotion.setAngles("LElbowRoll", degToRad((float)-2.8), 1.0f);
        }

        public void punchRight()
        {
            //naoMotion.setAngles("RWristYaw", degToRad((float)0.0), 1.0f);
            naoMotion.setAngles("RElbowYaw", degToRad((float)90.0), 1.0f);
            Thread.Sleep(500);

            //naoMotion.setAngles("RWristYaw", degToRad((float)90.0), 1.0f);
            naoMotion.setAngles("RShoulderPitch", degToRad((float)38.7), 1.0f); 
            naoMotion.setAngles("RShoulderRoll", degToRad((float)-40.0), 1.0f);
            naoMotion.setAngles("RElbowRoll", degToRad((float)89.0), 1.0f);
            Thread.Sleep(500);

            //naoMotion.setAngles("RWristYaw", degToRad((float)90.0), 1.0f);
            naoMotion.setAngles("RShoulderPitch", degToRad((float)2.5), 1.0f);
            naoMotion.setAngles("RShoulderRoll", degToRad((float)-90.0), 1.0f);
            naoMotion.setAngles("RElbowRoll", degToRad((float)89.0), 1.0f);
            naoMotion.setAngles("RElbowYaw", degToRad((float)20.0), 1.0f);
            Thread.Sleep(500);

            //naoMotion.setAngles("RWristYaw", degToRad((float)0.0), 1.0f);
            naoMotion.setAngles("RElbowRoll", degToRad((float)5.6), 1.0f);
            naoMotion.setAngles("RShoulderRoll", degToRad((float)-0.5), 1.0f);
            Thread.Sleep(1000);

            naoMotion.setAngles("RShoulderPitch", degToRad((float)70.0), 1.0f);
            //naoMotion.setAngles("RWristYaw", degToRad((float)0.0), 1.0f);
            naoMotion.setAngles("RShoulderRoll", degToRad((float)-4.0), 1.0f);
            naoMotion.setAngles("RElbowRoll", degToRad((float)2.8), 1.0f);
        }

        public void aimDownTheSight()
        {
            // Nisanenje so dvete race gore
            naoMotion.setAngles("RShoulderRoll", degToRad((float)-0.5), 0.2f);
            naoMotion.setAngles("RElbowRoll", degToRad((float)22.2), 0.2f);
            naoMotion.setAngles("RShoulderPitch", degToRad((float)2.5), 0.2f);
            
            naoMotion.setAngles("LShoulderRoll", degToRad((float)0.5), 0.2f);
            naoMotion.setAngles("LElbowRoll", degToRad((float)-22.2), 0.2f);
            naoMotion.setAngles("LShoulderPitch", degToRad((float)2.5), 0.2f);

            //naoMotion.setAngles("RWristYaw", degToRad((float)60.0), 0.5f);
            naoMotion.setAngles("RElbowYaw", degToRad((float)0.0), 0.5f);
      
            //naoMotion.setAngles("LWristYaw", degToRad((float)-60.0), 0.5f);
            naoMotion.setAngles("LElbowYaw", degToRad((float)0.0), 0.5f);
            Thread.Sleep(1000);
        }

        public void shoot()
        {
            //List<float> angles = naoMotion.getAngles("RWristYaw", false);
            
            // Gesture za pogodok
            naoMotion.setAngles("LShoulderPitch", degToRad((float)-20.0), 1.0f);
            naoMotion.setAngles("RShoulderPitch", degToRad((float)-20.0), 1.0f);
            Thread.Sleep(1000);
            naoMotion.setAngles("LShoulderPitch", degToRad((float)2.5), 0.3f);
            naoMotion.setAngles("RShoulderPitch", degToRad((float)2.5), 0.3f);
            
        }

        public static float degToRad(float deg)
        {
            return (float)Math.PI / 180 * deg;
        }


        /// <summary> 
        /// Class deconstructor 
        /// Cuts motor stiffness 
        /// </summary> 
        ~Motion()
        {
            if (naoMotion == null)
            {
                return;
            }
            try
            {
                naoMotion.setAngles("LShoulderPitch", 2f, 0.25f);
                naoMotion.setAngles("RShoulderPitch", 2f, 0.25f);
                System.Threading.Thread.Sleep(1000);

                // reduce stiffness 
                naoMotion.stiffnessInterpolation("Head", 0.0f, 0.1f);
                naoMotion.stiffnessInterpolation("LArm", 0.1f, 0.1f);
                naoMotion.stiffnessInterpolation("RArm", 0.1f, 0.1f);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                // display error message and write exceptions to a file 
                //MessageBox.Show("Exception occurred, error log in C:\\NAOcam\\exception.txt"); System.IO.File.WriteAllText(@"C:\\NAOcam\\exception.txt",
//e.ToString());
            }
        }

        /// <summary> 
        /// Opens the desired hand 
        /// </summary> 
        /// <param name="hand"> the desired hand, either LHand or RHand </param> 
        public void openHand(string hand)
        {
            try
            {
                naoMotion.openHand(hand);
            }
            catch (Exception e)
            {
                // display error message and write exceptions to a file 
                //MessageBox.Show("Exception occurred, error log in C:\\NAOcam\\exception.txt");
                System.IO.File.WriteAllText(@"C:\\NAOcam\\exception.txt",
e.ToString());
            }
        }

        /// <summary> 
        /// Closes the desired hand 
        /// </summary> 
        /// <param name="hand"> the desired hand, either LHand or RHand </param> 
        public void closeHand(string hand)
        {
            try
            {
                naoMotion.closeHand(hand);
            }
            catch (Exception e)
            {
                // display error message and write exceptions to a file 
               // MessageBox.Show("Exception occurred, error log in C:\\NAOcam\\exception.txt");
                System.IO.File.WriteAllText(@"C:\\NAOcam\\exception.txt",
e.ToString());
            }
        }

        /// <summary> 
        /// Gets the current angle of a joint 
        /// </summary> 
        /// <param name="joint"> the joint to retrieve the angle from </param> 
        /// <returns> the angle in radians </returns> 
        public float getAngle(string joint)
        {
            try
            {
                List<float> angles = naoMotion.getAngles(joint, false);

                return angles[0];
            }
            catch (Exception e)
            {
                // display error message and write exceptions to a file 
               // MessageBox.Show("Exception occurred, error log in C:\\NAOcam\\exception.txt");
                System.IO.File.WriteAllText(@"C:\\NAOcam\\exception.txt",
e.ToString());
            }

            return -1;
        }

        /// <summary> 
        /// Moves the joint to the desired angle 
        /// </summary> 
        /// <param name="value"> the angle in radians </param> 
        /// <param name="joint"> the joint to be moved </param> 
        public void moveJoint(float value, string joint)
        {
            try
            {
                naoMotion.setAngles(joint, value, 0.1f);
            }
            catch (Exception e)
            {
                // display error message and write exceptions to a file 
                //MessageBox.Show("Exception occurred, error log in C:\\NAOcam\\exception.txt");
                System.IO.File.WriteAllText(@"C:\\NAOcam\\exception.txt",
e.ToString());
            }
        }
    }
}