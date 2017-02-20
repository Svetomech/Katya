using System;

namespace LR4
{
    // Заявки.
    internal static class Applications
    {
        private static Random _random = new Random();   // Генератор случайных чисел


        // Возвращает сколько единиц времени будет выполняться заявка.
        public static float Application => (float)_random.NextDouble();

        // Возвращает сколько единиц времени осталось до новой заявки.
        public static float NextApplication
        {
            get
            {
                double range = 6.0 - 0.0;   // Диапазон е.в. от 0 до 6
                double sample = _random.NextDouble();
                double scaled = (sample * range) + 0.0;

                return (float)scaled;
            }
        }

        // Определяет, должна ли заявка поступить вновь.
        public static bool ShouldReapply
        {
            get
            {
                double sample = _random.NextDouble();

                return sample < 0.8;    // Перепоступать в 80% случаев
            }
        }
    }
}
