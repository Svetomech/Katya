using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LR4.Applications;

/*
В конце процесса выдать общее время моделирования
и количество вошедших в систему и вышедших из нее заявок, среднее время
пребывания заявки в очереди, время простоя аппарата, количество срабатываний
ОА.
*/

namespace LR4
{
    // Обслуживающий аппарат. Использует шаблон проектирования Singleton.
    internal class ServiceUnit
    {
        private static ServiceUnit _instance;

        private List<IQueue<float>> _queues;    // Обслуживаемые очереди
        // задачи, выполняющие очереди
        // таймер для общего времени
        // ...

        private const int _TimeFactor = 100;   // Скорость симуляции

        private ServiceUnit()
        {
            _queues = new List<IQueue<float>>();

            TimeSinceStart = 0.0f;
            ServedApplicationsCount = 0;
        }

        public static ServiceUnit Instance => _instance ?? (_instance = new ServiceUnit());


        // Общее время выполнения.
        public float TimeSinceStart { get; private set; }

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
                throw new InvalidOperationException();

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
            throw new NotImplementedException();
        }


        // Обслуживает очередь заявок.
        private async Task ServeQueueAsync(IQueue<float> queue)
        {
            var acceptTask = AcceptApplicationAsync(queue, NextApplication);
            var serveTask = ServeApplicationAsync(queue, Application);

            // вывод промежуточных результатов

            // дождаться результатов задач

            throw new NotImplementedException();
        }

        // Принимает одну заявку.
        private async Task AcceptApplicationAsync(IQueue<float> queue, float timeUnits)
        {
            await WorkAsync(timeUnits);
            queue.Enqueue(Application); // добавить в счетчик всех заявок
            throw new NotImplementedException();
        }

        // Обслуживает одну заявку.
        private async Task ServeApplicationAsync(IQueue<float> queue, float timeUnits)
        {
            await WorkAsync(timeUnits);
            queue.Dequeue();

            if (++ServedApplicationsCount % 100 == 0)
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
