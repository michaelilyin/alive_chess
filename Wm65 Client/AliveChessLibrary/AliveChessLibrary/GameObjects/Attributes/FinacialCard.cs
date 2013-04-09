//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using AliveChessLibrary.Entities.Resources;

//namespace AliveChessLibrary.Entities.Attributes
//{
//    /* Класс Финансовая карта
//     * Позволяет игроку производить различные манипуляции с деньгами и ресурсами
//     */
//    public class FinancialCard
//    {
//        // поля
//        private int idFinancialCard; // идентификатор карты
//        private Vault vaultResource; // указатель на Хранилище ресурсов
//        private Purse purseGame; // указатель на Кошелек игрока
//        private Auction auctionGame; // указатель на Игровой Аукцион
//        //private List<Lot> claimedLots; // массив заявленных лотов Игрока

//        // конструкторы
//        public FinancialCard()
//        {
//            //this.claimedLots = new List<Lot>();
//        }

//        public FinancialCard(int id, Vault vault, Purse purse)
//        {
//            //this.announcedLot = new List<Lot>();
//            this.idFinancialCard = id;
//            this.vaultResource = vault;
//            this.purseGame = purse;
//            //this.auctionGame = auction;

//        }

//        //свойства

//        public Vault VaultResource
//        {
//            get { return this.vaultResource; }
//        }
//        public Purse PurseGame
//        {
//            get { return this.purseGame; }
//        }

//        //мтоды

//        /* Метод позволяет выставлять игровые ресурсы на аукцион
//         * На вход метод принимает тип, количество и цену выставляемых ресурсов, а так же указатель на финансовую карту игрока
//         */
//        public void ExposeResourceOnAuction(ResourceTypes typeRes, int countResource, int priceResource)
//        {
//            // Получить необходимый ресурс 
//            Resource tmpRes = this.vaultResource.GetResource(typeRes);
//            // Если необходимый ресурс получен
//            if (tmpRes != null)
//            {
//                // забрать необходимую часть ресурса 
//                tmpRes.CountResource -= countResource;
//                // Задать цену ресурса
//                Resource res = new Resource();
//                res.ResourceType = tmpRes.ResourceType;
//                res.CountResource = tmpRes.CountResource;
//                PricesResource priceRes = new PricesResource(res, priceResource);
//                // Создать новй лот 
//                Lot tmpLot = this.auctionGame.CreateLot(priceRes);
//                //Если лот создан
//                if (tmpLot != null)
//                {
//                    // передать в лот финансовую карту
//                    tmpLot.FinancialCardBuyer = this;
//                }
//                // Если лот не создан
//                else
//                {
//                    // сообщить об ошибке с ценой
//                }
//            }
//            // Если необходимого ресурса нет
//            // Вернуть сообщение   
//        }

//    }
//}
