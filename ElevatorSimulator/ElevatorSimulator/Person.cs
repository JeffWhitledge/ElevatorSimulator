using SimCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    class Person : SimObject
    {
        private int _destinationFloor;
        private Floor _currentFloor;
        private Elevator _currentElevator;
        private PersonGroup _group;

        protected override void AdvanceSec(double advanceAmountSec)
        {
            throw new NotImplementedException();
        }

        protected override void OnEventOccurred(ScheduledEvent scheduledEvent)
        {
            throw new NotImplementedException();
        }
    }

    class PersonGroup
    {
        private readonly List<Person> _members = new List<Person>();

    }
}
