using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class BenchmarkZernike : MonoBehaviour
{
    public ZernikeManager manager;
    public List<List<Vector2>> testStrokes;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Stopwatch sw = new Stopwatch();

        sw.Start();
        manager.processor.DrawStrokes(testStrokes);
        sw.Stop();
        UnityEngine.Debug.Log("DrawStrokes: " + sw.ElapsedMilliseconds + " ms");

        sw.Reset();
        sw.Start();
        var moments = manager.processor.ComputeZernikeMoments(manager.maxMomentOrder);
        sw.Stop();
        UnityEngine.Debug.Log("ComputeZernikeMoments: " + sw.ElapsedMilliseconds + " ms");

        sw.Reset();
        sw.Start();
        var hist = manager.processor.GetSymbolDistribution();
        sw.Stop();
        UnityEngine.Debug.Log("GetSymbolDistribution: " + sw.ElapsedMilliseconds + " ms");
    }
}
