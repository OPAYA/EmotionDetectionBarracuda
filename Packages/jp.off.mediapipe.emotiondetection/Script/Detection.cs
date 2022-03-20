using System.Runtime.InteropServices;
using UnityEngine;

namespace MediaPipe.EmotionDetection {

partial class EmotionDetector
{
    //
    // Detection structure. The layout of this structure must be matched with
    // the one defined in Common.hlsl.
    //
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Detection
    {
        public readonly uint classIndex;
        public readonly float score;

        // sizeof(Detection)
        public static int Size = 6 * sizeof(int);

        // String formatting
        public override string ToString()
        => $"({classIndex}({score})";
        };
}

} // namespace MediaPipe.BlazePalm
