using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using System.Linq;

namespace MediaPipe.EmotionDetection {

//
// Emotion detector class
//
public sealed partial class EmotionDetector : System.IDisposable
{
    
    #region Public accessors

    public int ImageSize
      => _size;

    // public ComputeBuffer DetectionBuffer
    //   => _postBuffer;

    #endregion

    #region Public methods

    public EmotionDetector(ResourceSet resources)
      => AllocateObjects(resources);

    public void Dispose()
      => DeallocateObjects();


    public void ProcessImage(Texture image, float threshold)
      => RunModel(image, threshold);

    public ComputeBuffer DetectionBuffer
      => _buffers.post;

    #endregion

    #region Compile-time constants

    // Maximum number of detections. This value must be matched with
    // MAX_DETECTION in Common.hlsl.
    const int MaxDetection = 8;

    #endregion

    #region Private objects

    ResourceSet _resources;
    IWorker _worker;
    int _size;

    (ComputeBuffer preprocess,
     ComputeBuffer feature,
     ComputeBuffer post,
     ComputeBuffer counter,
     ComputeBuffer countRead) _buffers;   

    readonly static string[] Labels =
      { "Neutral", "Happiness", "Surprise", "Sadness",
        "Anger", "Disgust", "Fear", "Contempt"};

    void AllocateObjects(ResourceSet resources)
    {
        var model = ModelLoader.Load(resources.model);
        _worker = model.CreateWorker();

        _resources = resources;
        
        _size = model.inputs[0].shape[6]; // Input tensor width

        _buffers.preprocess = new ComputeBuffer
          (_size * _size * 1, sizeof(float));

        _buffers.post = new ComputeBuffer
          (MaxDetection, Detection.Size, ComputeBufferType.Append);
          
        _buffers.counter = new ComputeBuffer
          (1, sizeof(uint), ComputeBufferType.Counter);

        _buffers.countRead = new ComputeBuffer
          (1, sizeof(uint), ComputeBufferType.Raw);

        
    }

    void DeallocateObjects()
    {
        _buffers.preprocess?.Dispose();
        _buffers.preprocess = null;

        _buffers.post?.Dispose();
        _buffers.post = null;

        _buffers.counter?.Dispose();
        _buffers.counter = null;

        _buffers.countRead?.Dispose();
        _buffers.countRead = null;

        _worker?.Dispose();
        _worker = null;
    }

    #endregion

    #region Neural network inference function

    void RunModel(Texture source, float threshold)
    {
        
        var pre = _resources.preprocess;
        pre.SetInt("_ImageSize", _size);
        pre.SetTexture(0, "_Texture", source);
        pre.SetBuffer(0, "_Tensor", _buffers.preprocess);
        pre.Dispatch(0, _size / 8, _size / 8, 1);

        using (var tensor = new Tensor(1, _size, _size, 1, _buffers.preprocess))
            _worker.Execute(tensor);

        var probs = _worker.PeekOutput().AsFloats().Select(x => Mathf.Exp(x));
        var sum = probs.Sum();
        var lines = Labels.Zip(probs, (l, p) => $"{l,-12}: {p / sum:0.00}");

        Debug.Log(string.Join("\n", lines));

    }

    #endregion

}

} // namespace MediaPipe.EmotionDetection
