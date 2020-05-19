using UnityEngine;

namespace Jichaels.Core
{

    public static class Yielders
    {
        public static WaitForEndOfFrame EndOfFrame { get; } = new WaitForEndOfFrame();
        public static WaitForSeconds OneSecond { get; } = new WaitForSeconds(1);
        public static WaitForSeconds PointOneSecond { get; } = new WaitForSeconds(0.1f);
    }
}