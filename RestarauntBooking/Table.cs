using System;

namespace RestarauntBooking
{
    public class Table
    {
        public State State { get; private set; }
        public int SeatsCount { get; }
        public int Id { get; }

        public Table(int id)
        {
            Id = id;
            State = State.Free;
            SeatsCount = Random.Next(2, 5);
        }

        public bool SetState(State state)
        {
            lock (_lock)
            {
                if (state == State)
                {
                    return false;
                }

                State = state;
                return true;
            }
        }

        private readonly object _lock = new object();
        private static readonly Random Random = new Random();
    }
}