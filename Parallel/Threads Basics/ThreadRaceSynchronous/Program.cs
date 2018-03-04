using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace ThreadRaceSynchronous
{
    class Program
    {
        private const int THREADS = 3;
        private static Racer[] _racers;
        private static List<Timer> _timers = new List<Timer>();


        static void Main(string[] args)
        {
            var gate = new object();
            int finish = 15;// 85;
            _racers = new Racer[THREADS];
            for (int i = 0; i < THREADS; i++)
            {
                Predicate<Racer> isWin = race =>
                                        !_racers
                                            .Where(r => r != race)
                                            .Any(r => r.RaceState == State.Win);
                string title = ((char)('A' + i)).ToString();
                var racer = new Racer(gate, title, i, finish, isWin);
                _racers[i] = racer;
                switch (i % 3)
                {
                    case 0:
                        var trd = new Thread(RunRacerAsync);
                        trd.Start(racer);
                        break;
                    case 1:
                        ThreadPool.QueueUserWorkItem(RunRacerAsync, racer);
                        break;
                    case 2:
                        var tmr = new Timer(s =>
                        {
                            if (racer.RaceState == State.Running)
                                racer.Advance();
                        }, racer, 1, 500);
                        _timers.Add(tmr);
                        break;
                    default:
                        break;
                }

            }

            // should be on:
            //  - A = Thread
            //  - B = ThreadPool


            //  - C = Timer (bonus)

            //int step = 0;
            //while (true)
            //{
            //    var racer = _racers[step++ % _racers.Length];
            //    racer.Advance();
            //}

            // single color per racer
            // only one win

            Console.ReadKey();

        }

        private static void RunRacerAsync(object r)
        {
            var racer = (Racer)r;
            while (racer.RaceState == State.Running)
            {
                racer.Advance();
            }
        }

    }
}
