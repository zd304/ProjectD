using System;
using System.Diagnostics;

namespace GameServer
{
    public class Time
    {
        public void Initialize()
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
            lastTime = stopWatch.ElapsedMilliseconds;
        }

        public void Uninitialize()
        {
            stopWatch.Stop();
        }

        public void Update()
        {
            long cur = stopWatch.ElapsedMilliseconds;
            deltaTime = (float)(cur - lastTime) * 0.001f;
            lastTime = cur;
        }

        public float deltaTime = 0.0f;

        public float ElaspsedSeconds
        {
            get
            {
                return (float)stopWatch.ElapsedMilliseconds * 0.001f;
            }
        }

        private Stopwatch stopWatch = null;
        private long lastTime;
    }
}
