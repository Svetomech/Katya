using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LR4.Applications;

/*
Осталось добавить: среднее время пребывания
заявки в очереди, количество срабатываний ОА.
*/

namespace LR4
{
    // Обслуживающий аппарат. Использует шаблон проектирования Singleton.
    internal class ServiceUnit
    {
        private static ServiceUnit _instance;

        private List<IQueue<float>> _queues;    // Обслуживаемые очереди
        // задачи, выполняющие очереди
        // таймер для времени выполнения
        // таймер для времени простоя

        private const int _TimeFactor = 10;   // Скорость симуляции, [1;1000] (меньше - лучше)
        private const int _LogRate = 100;     // Частота информации, [1;1000] (больше - лучше)

        private ServiceUnit()
        {
            _queues = new List<IQueue<float>>();
        }

        public static ServiceUnit Instance => _instance ?? (_instance = new ServiceUnit());


        // Общее время выполнения.
        public float TimeSinceStart { get; private set; }

        // Общее время простоя.
        public float TimeSpentIdle { get; private set; }

        // Счётчик вышедших заявок.
        public int ServedApplicationsCount { get; private set; }

        // Счётчик вошедших заявок.
        public int ApplicationsCount { get; private set; }

        // Добавляет очередь на обслуживание.
        public void AddQueue(IQueue<float> queue)
        {
            _queues.Add(queue);
        }

        // Начинает обслуживание.
        public void Start()
        {
            if (_queues.Count == 0)
                throw new InvalidOperationException("Нечего обслуживать");

            foreach (var queue in _queues)
            {
                Task.Run(() => ServeQueueAsync(queue));
            }

            // запустить таймер времени
            throw new NotImplementedException();
        }

        // Заканчивает обслуживание.
        public void Stop()
        {
            // остановить задачи и глобальный таймер
            throw new NotImplementedException();
        }


        // Обслуживает очередь заявок.
        private async Task ServeQueueAsync(IQueue<float> queue)
        {
            var acceptTask = AcceptApplicationAsync(queue, NextApplication);
            var serveTask = ServeApplicationAsync(queue, Application);

            // дождаться результатов задач
            throw new NotImplementedException();
        }

        // Принимает одну заявку.
        private async Task AcceptApplicationAsync(IQueue<float> queue, float timeUnits)
        {
            await WorkAsync(timeUnits); // добавить в общее время простоя, учесть _TimeFactor
            queue.Enqueue(Application); // добавить в счетчик всех заявок
            throw new NotImplementedException();
        }

        // Обслуживает одну заявку.
        private async Task ServeApplicationAsync(IQueue<float> queue, float timeUnits)
        {
            await WorkAsync(timeUnits);
            queue.Dequeue();

            if (++ServedApplicationsCount % _LogRate == 0)
            {
                Console.WriteLine(); // информация о текущей и средней длине очереди.
                throw new NotImplementedException();
            }

            if (!ShouldReapply) return;

            queue.Enqueue(Application); // ?добавить в счетчик всех заявок
            throw new NotImplementedException();
        }

        // Симулирует работу.
        private async Task WorkAsync(float timeUnits)
        {
            float time = timeUnits * _TimeFactor;

            await Task.Delay((int) Math.Round(time));
        }
    }
}
