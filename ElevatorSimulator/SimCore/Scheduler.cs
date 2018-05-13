using System.Collections.Generic;

namespace SimCore
{
    /// <summary>
    /// Coordinates all of the actions in the simulation.
    /// </summary>
    public class Scheduler
    {
        private double _currentTimeSec = 0d;
        private readonly List<SimObject> _simObjects = new List<SimObject>();

        /// <summary>
        /// The current time in the simulation in seconds since simulation start.
        /// </summary>
        public double CurrentTimeSec { get { return _currentTimeSec; } }

        /// <summary>
        /// Constructs a new Scheduler object.
        /// </summary>
        public Scheduler()
        {
        }

        /// <summary>
        /// Advances the simulation forward to the next scheduled event, and performs that event.
        /// </summary>
        /// <returns>Returns true if the simulation performed an event; false, if there are no more events, signaling 
        /// that the simulation is over.</returns>
        public bool AdvanceToNextEvent()
        {
            return AdvanceToEvent(GetNextEvent());
        }

        private bool AdvanceToEvent(ScheduledEvent scheduledEvent)
        {
            if (scheduledEvent.IsEmpty()) return false;
            var advanceAmountSec = scheduledEvent.TimeOfOccurenceSec - _currentTimeSec;
            if (advanceAmountSec > 0)
            {
                foreach (var simObj in _simObjects)
                {
                    simObj.AdvanceSec(advanceAmountSec);
                }
                _currentTimeSec = scheduledEvent.TimeOfOccurenceSec;
            }
            scheduledEvent.InvolvedObject.EventOccurred(scheduledEvent);
            return true;
        }

        /// <summary>
        /// Adds an object to the simulation.
        /// </summary>
        /// <param name="simObj">The object to add.</param>
        public void AddSimObject(SimObject simObj)
        {
            _simObjects.Add(simObj);
        }

        /// <summary>
        /// Removes an object from the simulation.
        /// </summary>
        /// <param name="simObj">The object to remove.</param>
        public void RemoveSimObject(SimObject simObj)
        {
            _simObjects.Remove(simObj);
        }

        private ScheduledEvent GetNextEvent()
        {
            var comp = ScheduledEvent.ScheduledEventTimeComparer;
            var nextEvent = ScheduledEvent.Empty;
            foreach (var simObj in _simObjects)
            {
                var objNextEvent = simObj.NextEvent;
                if (comp.Compare(objNextEvent, nextEvent) < 0 )
                {
                    nextEvent = objNextEvent;
                }
            }
            return nextEvent;
        }

    }
}
