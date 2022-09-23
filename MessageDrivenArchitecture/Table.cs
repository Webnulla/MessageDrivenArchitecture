using System;

namespace MessageDrivenArchitecture
{
    public class Table
    {
        public State State { get; private set; }

        public int SeatsCount { get; }

        public int Id { get; }

        public Random random = new Random();

        public Table(int id)
        {
            Id = id;
            State = State.Free;
            SeatsCount = random.Next(2, 5);
        }

        public bool SetState(State state)
        {
            if (state == State)
            {
                return false;
            }

            State = state;
            return true;
        }
    }
}