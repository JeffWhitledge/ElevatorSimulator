using System.Collections.Generic;

namespace SimCore
{
    /// <summary>
    /// Base class for all objects that participate in the simulation. Maintains a schedule of future events.
    /// </summary>
    public abstract class SimObject
    {
        // The current schedule of events in which the object will participate.
        private readonly SortedSet<ScheduledEvent> _futureEvents =
            new SortedSet<ScheduledEvent>(ScheduledEvent.ScheduledEventTimeComparer);

        /// <summary>
        /// Adds an event to the schedule.
        /// </summary>
        /// <param name="timeOfOccurenceSec">The time at which the event is to occur in simulation seconds from the 
        /// start of the simulation.</param>
        /// <param name="eventTypeName">The type of the event identified by a string constant.</param>
        /// <returns>Returns the new event that was scheduled.</returns>
        protected ScheduledEvent ScheduleEvent(double timeOfOccurenceSec, string eventTypeName)
        {
            var result = new ScheduledEvent(timeOfOccurenceSec, eventTypeName, this);
            _futureEvents.Add(result);
            return result;
        }

        /// <summary>
        /// Removes an event of the schedule.
        /// </summary>
        /// <param name="scheduledEvent">The event that is to be removed.</param>
        protected void EventUnscheduled(ScheduledEvent scheduledEvent)
        {
            _futureEvents.Remove(scheduledEvent);
        }

        /// <summary>
        /// Removes all events from the schedule.
        /// </summary>
        protected void UnscheduleAllEvents()
        {
            _futureEvents.Clear();
        }

        /// <summary>
        /// Removes all events of the specified type from the schedule.
        /// </summary>
        /// <param name="eventType">The type of the events to remove identified by a string constant.</param>
        protected void UnscheduleEventsOfType(string eventType)
        {
            _futureEvents.RemoveWhere(x => x.IsType(eventType));
        }

        /// <summary>
        /// Removes all events that are scheduled to occur after the specified time.
        /// </summary>
        /// <param name="latestTimeToKeepSec">Specifies the time of the soonest events to keep in simulation seconds 
        /// from the start of the simulation.</param>
        protected void UnscheduleEventsAfter(double latestTimeToKeepSec)
        {
            _futureEvents.RemoveWhere(x => latestTimeToKeepSec.CompareTo(x.TimeOfOccurenceSec) < 0);
        }

        /// <summary>
        /// Removes all events of the specified type that are scheduled to occur after the specified time.
        /// </summary>
        /// <param name="eventType">The type of the events to remove identified by a string constant.</param>
        /// <param name="latestTimeToKeepSec">Specifies the time of the soonest events to keep in simulation seconds 
        /// from the start of the simulation.</param>
        protected void UnscheduleEventsOfTypeAfter(string eventType, double latestTimeToKeepSec)
        {
            _futureEvents.RemoveWhere(x => x.IsType(eventType) && 
                latestTimeToKeepSec.CompareTo(x.TimeOfOccurenceSec) < 0);
        }


        /// <summary>
        /// Gets the next event that is scheduled to occur, if any.
        /// </summary>
        protected internal ScheduledEvent NextEvent
        {
            get
            {
                if (_futureEvents.Count == 0)
                {
                    return ScheduledEvent.Empty;
                }
                return _futureEvents.Min;
            }
        }

        /// <summary>
        /// Advances the simulation of the current object the specified number of seconds.
        /// </summary>
        /// <param name="advanceAmountSec">The amount of time in seconds that the object is to advance.</param>
        protected internal abstract void AdvanceSec(double advanceAmountSec);

        /// <summary>
        /// Signals to the current object that it is to perform the specified event.
        /// </summary>
        /// <param name="scheduledEvent">The event to perform.</param>
        protected internal abstract void OnEventOccurred(ScheduledEvent scheduledEvent);

        internal void EventOccurred(ScheduledEvent scheduledEvent)
        {
            EventUnscheduled(scheduledEvent);
            OnEventOccurred(scheduledEvent);
        }
    }
}
