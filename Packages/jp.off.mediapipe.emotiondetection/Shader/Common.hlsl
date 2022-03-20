#ifndef _EMOTIONDETECTIONBARRACUDA_COMMON_H_
#define _EMOTIONDETECTIONBARRACUDA_COMMON_H_

#include "Struct.hlsl"

// Maximum number of detections. This value must be matched with MaxDetection
// in EmotionDetector.cs.
#define MAX_DETECTION 8

// We can encode the geometric features of Detection into a float4x4 matrix.
// This is handy for calculating weighted means of detections.

struct Detection
{
    uint classIndex;
    float score;
};

// Common math functions

float Sigmoid(float x)
{
    return 1 / (1 + exp(-x));
}


#endif
