using SimCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    class SimView : SimObject
    {
        private const int FramesPerSecond = 10;
        private TimeSpan _updateDelay = new TimeSpan(0, 0, 0, 0, 1000 / FramesPerSecond);
        private DateTime _lastUpdateTime = DateTime.Now;

        protected override void AdvanceSec(double advanceAmountSec)
        {
        }

        protected override void OnEventOccurred(ScheduledEvent scheduledEvent)
        {
            var referenceTime = DateTime.Now;
            var nextUpdateTime = _lastUpdateTime + _updateDelay;
            var timeToWait = nextUpdateTime - referenceTime;
            if (timeToWait > TimeSpan.Zero)
            {
                Thread.Sleep(timeToWait);
            }
            else
            {
                nextUpdateTime = referenceTime;
            }
            UpdateDisplay();
            _lastUpdateTime = nextUpdateTime;
        }

        private void UpdateDisplay()
        {

        }
    }
}
