using System;

namespace Pacman_Remaster.Objects
{
    class TimerObject
    {
        private DateTime m_LastTime;
        public TimerObject()
        {

        }
        public void StartTimer()
        {
            m_LastTime = DateTime.Now;
        }
        public float GetElapsedSeconds()
        {
            DateTime now = DateTime.Now;
            TimeSpan elasped = now - m_LastTime;
            m_LastTime = now;
            return (float)elasped.Ticks / TimeSpan.TicksPerSecond;
        }
    }
}
