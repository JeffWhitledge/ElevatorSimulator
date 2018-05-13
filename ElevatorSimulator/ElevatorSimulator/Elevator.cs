using SimCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    class Elevator : SimObject
    {
        private readonly List<Person> _occupants = new List<Person>();
        private double _heightFeet;
        private double _doorOpenWidthFeet;


        protected override void AdvanceSec(double advanceAmountSec)
        {
            throw new NotImplementedException();
        }

        protected override void OnEventOccurred(ScheduledEvent scheduledEvent)
        {
            throw new NotImplementedException();
        }
    }
}
