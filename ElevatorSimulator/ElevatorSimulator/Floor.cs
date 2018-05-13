using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    class Floor
    {
        public double Height { get; private set; }

        private List<Person> _occupants = new List<Person>();

        private bool _downButtonPressed;
        private bool _upButtonPressed;


        public IEnumerable<Person> Occupants
        {
            get
            {
                return _occupants;
            }
        }

    }
}
