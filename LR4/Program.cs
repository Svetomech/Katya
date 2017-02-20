using System.Threading;

namespace LR4
{
    class Program
    {
        static void Main(string[] args)
        {
            // Инициализация...
            var serviceUnit = ServiceUnit.Instance;     // обслуживающего аппарата
            var arrayQueue = new ArrayQueue<float>();   // очереди на основе массива
            var listQueue = new ListQueue<float>();     // очереди на основе списка

            serviceUnit.AddQueue(arrayQueue);   // добавление очереди-массива в ОА
            serviceUnit.AddQueue(listQueue);    // добавление очереди-списка в ОА

            serviceUnit.Start();   // запуск обслуживающего аппарата

            while (serviceUnit.ServedApplicationsCount < 1000)
            {
                Thread.Sleep(5);   // ожидание ухода из системы 1000 заявок
            }

            // вывод итоговых результатов
        }
    }
}
