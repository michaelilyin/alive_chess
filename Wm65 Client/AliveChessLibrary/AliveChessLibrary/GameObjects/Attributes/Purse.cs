namespace AliveChessLibrary.GameObjects.Attributes
{
    /* Класс Кошелек
    * Хранит в себе все денежные средства игрока и методы доступа к этим средствам
    */
    public class Purse
    {
        // поля
        private int moneyCount; // колличество денег в кошельке
        //private string purseMesseger; // сообщение от кошелька

        // конструкторы
        public Purse() { }
        public Purse(int moneyCount) // создает кошелек с определенным количеством денег
        {
            this.moneyCount = moneyCount;
        }

        // свойства
        public int MoneyCount
        {
            get { return this.moneyCount; }
        }

        // методы

        /* Метод добавляет денги в кошелек */
        public void PlaceMoney(int count)
        {
            // Добавить деньги в кошелек
            this.moneyCount += count;
        }

        /* Метод берет деньги из кошелька
         * На вход методу поступает необходимая сумма
         * Если денег хватает метод возвращает нужную сумму
         * В противном случае метод возвращает остаток средств в кошельке
         */
        public int TakeMoney(int count)
        {
            // Если денег в кошельке хватает
            if (this.monyWillBeEnough(count))
            {
                //снять необходимую сумму со счета
                this.moneyCount -= count;
                //вернуть необходимую сумму
                return count;
            }
            // Если денег не хватает
            else
            {
                //вернуть соответствующее сообщение от Кошелька
                //this.purseMesseger = "В кошельке не хватает денег"; // исключение
                return -1;
                //PurceExeption purceExeption = new PurceExeption(this.purseMesseger);
                //throw purceExeption;
            }
        }


        /* Метод проверяет может ли клиент взять требуемую сумму из кошелька
         * На вход метоу поступает необходимая сумма
         * Если денег достаточно для снятия необходимой суммы метод возвращает True
         * В противном случае False
         */
        private bool monyWillBeEnough(int count)
        {
            // Если денег в кошельке болтше чем требуется
            if (this.moneyCount >= count)
                return true;
            // Если меньше чем требуется
            else
                return false;


        }
    }
}
