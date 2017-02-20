using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LR4.Applications;

/*
Выдавать после обслуживания каждых 100 заявок информацию о текущей
и средней длине очереди. В конце процесса выдать общее время моделирования
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
        // ...

        private const int _TimeFactor = 100;   // Скорость симуляции

        private ServiceUnit() { }

        public static ServiceUnit Instance => _instance ?? (_instance = new ServiceUnit());


        // Общее время выполнения
        public float TimeSinceStart { get; private set; }

        // Счётчик обслуженных заявок
        public int ServedApplicationsCount { get; private set; }

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

            queue.Enqueue(Application);
        }

        // Обслуживает одну заявку.
        private async Task ServeApplicationAsync(IQueue<float> queue, float timeUnits)
        {
            await WorkAsync(timeUnits);

            queue.Dequeue();

            if (!ShouldReapply) return;

            queue.Enqueue(Application);
        }

        // Симулирует работу.
        private async Task WorkAsync(float timeUnits)
        {
            float time = timeUnits * _TimeFactor;

            await Task.Delay((int) Math.Round(time));
        }
    }
}
