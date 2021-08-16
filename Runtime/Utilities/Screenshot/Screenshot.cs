using UnityEngine;

namespace Drolegames.Utilities.Screenshot
{
    public class Screenshot : MonoBehaviour
    {
        public string fileName = "";
        public int superSize = 1;
        public static int NumberOfScreenshotsTaken = 0;
        public KeyCode trigger;
        public void Capture()
        {
            ScreenCapture.CaptureScreenshot($"{fileName}_{NumberOfScreenshotsTaken}.png", superSize);
            NumberOfScreenshotsTaken++;
        }

        private void Update()
        {
            if (Input.GetKeyDown(trigger))
            {
                Capture();
            }
        }
    }
}
