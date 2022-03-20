using UnityEngine;
using Unity.Barracuda;

namespace MediaPipe.EmotionDetection {

//
// ScriptableObject class used to hold references to internal assets
//
[CreateAssetMenu(fileName = "EmotionDetection",
                 menuName = "ScriptableObjects/MediaPipe/EmotionDetection Resource Set")]
public sealed class ResourceSet : ScriptableObject
{
    public NNModel model;
    public ComputeShader preprocess;
    public ComputeShader postprocess;
}

} // namespace MediaPipe.EmotionDetection
