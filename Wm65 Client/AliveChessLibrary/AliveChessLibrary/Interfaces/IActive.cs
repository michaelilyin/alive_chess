using System;

namespace AliveChessLibrary.Interfaces
{
    /* Интерфейс Астивность
     * Данный интерфейс должны реализовывть все объекты подразумевающие работу с таймером
     */
    public interface IActive
    {
        void Activation(); // активировать работу объекта
        void Deactivation(); // деактивировать работу объекта
        void DoWork(DateTime tmpDateTime); // заставить объект работать
    }
}
