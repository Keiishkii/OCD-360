using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GazeData
{
    public List<FrameData> frameData = new List<FrameData>();
}

[Serializable]
public class FrameData
{
    public float time;
    public Vector3 lookDirection;
    public Vector3 gazeDirection;
}
