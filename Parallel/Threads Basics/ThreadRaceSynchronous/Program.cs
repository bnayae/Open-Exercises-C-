using System;
using System.Threading;
using System.Linq;

namespace ThreadRaceSynchronous
{
    class Program
    {
        private const int THREADS = 3;
        private static Racer[] _racers;

        static void Main(string[] args)
        {
            int finish = 6;// 85;
            _racers = new Racer[THREADS];
            for (int i = 0; i < THREADS; i++)
            {
                Predicate<Racer> isWin = race => 
                                        !_racers
                                            .Where(r => r != race)
                                            .Any(r => r.RaceState == State.Win);
                string title = ((char)('A' + i)).ToString();
                _racers[i] = new Racer(title, i, finish, isWin);

            }

            // should be on:
            //  - A = Thread
            //  - B = ThreadPool
            
            
            //  - C = Timer (bonus)

            int step = 0;
            while (true)
            {
                var racer = _racers[step++ % _racers.Length];
                racer.Advance();
            }

            // single color per racer
            // only one win

        }

    }
}
