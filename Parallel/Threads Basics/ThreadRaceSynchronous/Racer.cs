using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadRaceSynchronous
{
    public class Racer
    {
        private readonly Predicate<Racer> _isWinPredicate;
        public Racer(
            string title,
            int offsetY,
            int finishOffset,
            Predicate<Racer> isWin)
        {
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
            Console.ForegroundColor = _color;
            OffsetX++;
            for (int i = 0; i < 5; i++)
            {
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
                Thread.Sleep(50);
            }
            Console.SetCursorPosition(OffsetX, OffsetY);
            Console.Write("->");
            Console.ResetColor();

            if (OffsetX >= FinishOffset + 3)
                Complete();
        }

        public void Complete()
        {
            bool win = _isWinPredicate(this);
            OffsetX++;
            for (int i = 0; i < 5; i++)
            {
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
                Thread.Sleep(50);
            }
            Console.SetCursorPosition(OffsetX, OffsetY);
            Console.Write("=> ");
            if (win)
            {
                Console.Write("*");
                RaceState = State.Win;
            }
            else
                RaceState = State.Lose;

            Console.ResetColor();
        }
    }
}
