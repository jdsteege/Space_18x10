using UnityEngine;
using System.Collections;

public class FrameLimiter : MonoBehaviour
{
	
	static int targetFramesPerSecond;
	static int targetFrameMillis;
	static System.Diagnostics.Stopwatch _watch;
	static long nextFrameEnd = 0;
	static long nextFrameHalf = 0;
	static bool frameDone = false;
	static bool frameHalfDone = false;

	static System.Diagnostics.Stopwatch watch {
		get {
			if (_watch == null) {
				_watch = new System.Diagnostics.Stopwatch ();
				_watch.Start ();
			}
			return _watch;
		}
	}

	void Awake ()
	{
		targetFramesPerSecond = 60;
		
		targetFrameMillis = 1000 / targetFramesPerSecond;
	}

	void Update ()
	{
		long thisFrameStart = watch.ElapsedMilliseconds;
		nextFrameEnd = thisFrameStart + targetFrameMillis;
		nextFrameHalf = thisFrameStart + (targetFrameMillis / 2);
		frameDone = false;
		frameHalfDone = false;
	}

	public static bool IsFrameDone ()
	{
		frameDone = frameDone || (watch.ElapsedMilliseconds >= nextFrameEnd);
		return frameDone;
	}

	public static bool IsFrameHalfDone ()
	{
		frameHalfDone = frameHalfDone || (watch.ElapsedMilliseconds >= nextFrameHalf);
		return frameHalfDone;
	}
	
}
