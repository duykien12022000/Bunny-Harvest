using UnityEngine;

namespace MyPlugins
{
    public class Utils
    {
        public static bool isiPad()
        {
#if (UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
            if (UnityEngine.iOS.Device.generation.ToString().Contains("iPad"))
            {
                return true;
            }
            else
            {
                float w = Screen.width;
                float h = Screen.height;
                if (h < w)
                {
                    float th = h;
                    h = w;
                    w = th;
                }
                if (w > 0)
                {
                    float per = h / w;
                    if (per < 1.65f)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
#else
            float w = Screen.width;
            float h = Screen.height;
            if (h < w)
            {
                float th = h;
                h = w;
                w = th;
            }
            if (w > 0)
            {
                float per = h / w;
                if (per < 1.65f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
#endif
        }
        public static float GetSizeBanner(int dp = 60)
        {
            if (isiPad())
            {
                dp = 90;
            }
#if UNITY_ANDROID && !UNITY_EDITOR
            return dp * Screen.dpi / 160;
#else
            var scaleFactory = dp * Screen.width / getScreenWidth() + Screen.safeArea.yMin;
            return scaleFactory;
#endif
        }
        public static float getScreenWidth()
        {
            //#if (UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
            //            return GameHelperIos.getScreenWidth();
            //#else
            return Screen.width;
            //#endif
        }
    }
}
