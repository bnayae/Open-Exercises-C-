using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadRaceSynchronous
{
    public class Racer
    {
        private readonly Predicate<Racer> _isWinPredicate;
        private readonly object _gate;
        public Racer(
            object gate,
            string title,
            int offsetY,
            int finishOffset,
            Predicate<Racer> isWin)
        {
            _gate = gate;
            Console.SetCursorPosition(0, offsetY);
            Console.WriteLine(title);

            Title = title;
            switch (offsetY % 4)
            {
                case 0:
                    _color = ConsoleColor.White;
                    break;
                case 1:
                    _color = ConsoleColor.Yellow;
                    break;
                case 2:
                    _color = ConsoleColor.Green;
                    break;
                case 3:
                    _color = ConsoleColor.Magenta;
                    break;
            }
            OffsetY = offsetY;
            FinishOffset = finishOffset;
            _isWinPredicate = isWin;
        }

        public string Title { get; }
        public ConsoleColor _color { get; }
        public int OffsetX = 3;
        public int OffsetY { get; }
        public int FinishOffset { get; }
        public State RaceState { get; private set; }

        public void Advance()
        {
            if (RaceState != State.Running)
                return;
            OffsetX++;
            for (int i = 0; i < 5; i++)
            {
                lock (_gate)
                {
                    Console.ForegroundColor = _color;
                    Console.SetCursorPosition(OffsetX, OffsetY);
                    switch (i % 2)
                    {
                        case 0:
                            Console.Write("->");
                            break;
                        case 1:
                            Console.Write(" >");
                            break;
                    }
                    Console.ResetColor();
                }
                Thread.Sleep(50);
            }
            lock (_gate)
            {
                Console.ForegroundColor = _color;
                Console.SetCursorPosition(OffsetX, OffsetY);
                Console.Write("->");
                Console.ResetColor();
            }

            if (OffsetX >= FinishOffset + 3)
                Complete();
        }

        public void Complete()
        {
            OffsetX++;
            for (int i = 0; i < 5; i++)
            {
                lock (_gate)
                {
                    Console.ForegroundColor = _color;
                    Console.SetCursorPosition(OffsetX, OffsetY);
                    switch (i % 2)
                    {
                        case 0:
                            Console.Write("=>");
                            break;
                        case 1:
                            Console.Write(" >");
                            break;
                    }
                }
                Thread.Sleep(50);
            }
            lock (_gate)
            {
                Console.ForegroundColor = _color;
                Console.SetCursorPosition(OffsetX, OffsetY);
                Console.Write("=> ");
                bool win = _isWinPredicate(this);
                if (win)
                {
                    Console.Write("*");
                    RaceState = State.Win;
                }
                else
                    RaceState = State.Lose;
            }

            Console.ResetColor();
        }
    }
}
