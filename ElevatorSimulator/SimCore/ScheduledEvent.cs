using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SimCore
{
    /// <summary>
    /// An event that is to be performed by an object at a particular time in the simulation.
    /// </summary>
    public struct ScheduledEvent : IEquatable<ScheduledEvent>
    {
        // A counter that generates unique event ID numbers.
        private static int s_nextScheduledEventId = 0;

        // The empty event.
        public static ScheduledEvent Empty = new ScheduledEvent();

        /// <summary>
        /// A comparer that sorts events by the event time.
        /// </summary>
        internal static IComparer<ScheduledEvent> ScheduledEventTimeComparer
        { get { return s_ScheduledEventTimeComp; } }

        private static readonly ScheduledEventTimeComparerImpl s_ScheduledEventTimeComp = 
            new ScheduledEventTimeComparerImpl();
        private static readonly StringComparer s_eventTypeComparer = StringComparer.Ordinal;

        /// <summary>
        /// A unique ID to identify this event. An object may use this to associate additional data with an event. 
        /// Zero indicates that this is the empty event.
        /// </summary>
        public readonly int ScheduledEventId;

        /// <summary>
        /// If not the empty event, the time that the event is scheduled to occur in simulation seconds from the start 
        /// of the simulation.
        /// </summary>
        public readonly double TimeOfOccurenceSec;

        /// <summary>
        /// If not the empty event, the type of event as defined by a constant string.
        /// </summary>
        public readonly string EventTypeName;

        /// <summary>
        /// If not the empty event, the object that is to perform the event.
        /// </summary>
        public readonly SimObject InvolvedObject;

        /// <summary>
        /// Constructs a new (non-empty) event structure.
        /// </summary>
        /// <param name="timeOfOccurenceSec">The time that the event is scheduled to occur in simulation seconds from 
        /// the start of the simulation.</param>
        /// <param name="eventTypeName">The type of event as defined by a constant string.</param>
        /// <param name="involvedObject">The object that is to perform the event.</param>
        /// <remarks>If the time of occurrence is less than or equal to the current simulation time, the event will 
        /// occur immediately without changing the simulation time. </remarks>
        internal ScheduledEvent(double timeOfOccurenceSec, string eventTypeName, SimObject involvedObject)
        {
            unchecked
            {
                ScheduledEventId = Interlocked.Increment(ref s_nextScheduledEventId);
            }
            TimeOfOccurenceSec = timeOfOccurenceSec;
            EventTypeName = eventTypeName ?? throw new ArgumentNullException(nameof(eventTypeName));
            InvolvedObject = involvedObject ?? throw new ArgumentNullException(nameof(involvedObject));
        }

        /// <summary>
        /// Indicates whether the current instance is the empty event.
        /// </summary>
        /// <returns>Returns true if the current instance is the empty event; false, otherwise.</returns>
        public bool IsEmpty()
        {
            return ScheduledEventId == 0;
        }

        /// <summary>
        /// Indicates whether the current instance is an event of the specified type.
        /// </summary>
        /// <param name="eventTypeName">The event type to test, as defined by a string constant.</param>
        /// <returns>Returns true if the current instance is an event of the specified type; false, otherwise.</returns>
        public bool IsType(string eventTypeName)
        {
            if (eventTypeName == null) throw new ArgumentNullException(nameof(eventTypeName));
            if (IsEmpty()) return false;
            return s_eventTypeComparer.Equals(EventTypeName, eventTypeName);
        }

        /// <summary>
        /// Tests whether two events are equal.
        /// </summary>
        /// <param name="other">The event to compare with the current instance.</param>
        /// <returns>Returns true if the event structures represent the same event; false, otherwise.</returns>
        public bool Equals(ScheduledEvent other)
        {
            return ScheduledEventId == other.ScheduledEventId;
        }

        private class ScheduledEventTimeComparerImpl : IComparer<ScheduledEvent>
        {
            public int Compare(ScheduledEvent x, ScheduledEvent y)
            {
                if (x.ScheduledEventId == y.ScheduledEventId)
                {
                    Debug.Assert(x.TimeOfOccurenceSec == y.TimeOfOccurenceSec, "There are two events with the same ID, but different times.");
                    return 0;
                }
                if (x.IsEmpty() && !y.IsEmpty()) return 1;
                if (!x.IsEmpty() && y.IsEmpty()) return -1;
                int timeComp = x.TimeOfOccurenceSec.CompareTo(y.TimeOfOccurenceSec);
                if (timeComp != 0) return timeComp;
                return x.ScheduledEventId.CompareTo(y.ScheduledEventId);
            }
        }
    }
}
