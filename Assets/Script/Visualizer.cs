using UnityEngine;
using UnityEngine.UI;
using MediaPipe.EmotionDetection;

namespace MediaPipe {

public sealed class Visualizer : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] WebcamInput _webcam = null;
    
    [SerializeField] RawImage _previewUI = null;
    [Space]
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] Shader _shader = null;
    [SerializeField, Range(0, 1)] float _threshold = 0.5f;

    #endregion

    #region Private members

    EmotionDetector _detector;
    Material _material;

    ComputeBuffer _boxDrawArgs;
    ComputeBuffer _keyDrawArgs;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _detector = new EmotionDetector(_resources);
        _material = new Material(_shader);
        
        var cbType = ComputeBufferType.IndirectArguments;
       
    }

    void OnDestroy()
    {
        _detector.Dispose();
        Destroy(_material);

        _boxDrawArgs.Dispose();
        _keyDrawArgs.Dispose();
    }

    void LateUpdate()
    {
        _detector.ProcessImage(_webcam.Texture, _threshold);
        _previewUI.texture = _webcam.Texture;
    }

    #endregion
}

} // namespace MediaPipe
