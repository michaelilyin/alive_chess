using System;

namespace AliveChessLibrary.Interfaces
{
    /* Интерфейс Астивность
     * Данный интерфейс должны реализовывть все объекты подразумевающие работу с таймером
     */
    public interface IActive
    {
        void Activate(); // активировать работу объекта
        void Deactivatе(); // деактивировать работу объекта
        void DoWork(DateTime tmpDateTime); // заставить объект работать
    }
}
