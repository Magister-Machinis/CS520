namespace SimulationCore
{
    class BussRoute //class and subclasses for the route stops as a doubly linked list
    {
        public class RouteWrapper
        {
            public BussStop stop = new BussStop();
            int stopnumber;
            BussStop previous;
            BussStop next;

            public void SetPrev (BussStop subject)
            {
                previous = subject;
            }
            public void SetNext(BussStop subject)
            {
                next = subject;
            }
            public void SetNum (int subject)
            {
                stopnumber = subject;
            }



            public BussStop GetPrev ()
            {
                return previous;
            }
            public BussStop GetNext()
            {
                return next;
            }
            public int Getnum()
            {
                return stopnumber;
            }
        }
    }
}
 