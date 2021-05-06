using FrisdrankAutomaat;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FrisdrankAutomat.Test
{

    /// <summary>
    /// Vending Machine Test class
    /// </summary>
    [TestFixture("Coca Cola", "fanta", "Thumsup")]
    public class VendingMachineTest
    {
        //Global Variable 
        public Vendingmachine machine;
        public Drink drink1;
        public Drink drink2;
        public Drink drink3;
        public List<Drink> listDrink;


        public VendingMachineTest(string firstDrink, string secondDrink, string thirdDrink) 
        {
            drink1 = new Drink(firstDrink, 40);
            drink2 = new Drink(secondDrink, 20);
            drink3 = new Drink(thirdDrink, 50);
        }

        [SetUp]
        public void Setup()
        {
            /*Machine is initialized*/
            machine = new Vendingmachine();

            //Drinks Container is ready
            listDrink = new List<Drink>();
            listDrink.Add(drink1);
            listDrink.Add(drink2);
            listDrink.Add(drink3);
        }

        [Test,Order(1)]
        public void CheckVendingMachineHasAllDrinksPlaced()
        {
            machine.FillRandomly(listDrink);
          
            //Is not empty
            Assert.IsNotNull(machine.Inventory);

            //Check inventory/vending machine holds total 50 places
            Assert.IsTrue(machine.Inventory.Count == 50);

            //Check whether initialized object above i.e. Coca Cola is placed inside vending machine/inventory
            Assert.IsTrue(machine.Inventory.ContainsValue(drink1));

            //Check whether initialized object above i.e. fanta is placed inside vending machine/inventory
            Assert.IsTrue(machine.Inventory.ContainsValue(drink2));

            //Check whether initialized object above i.e. Thumsup is placed inside vending machine/inventory
            Assert.IsTrue(machine.Inventory.ContainsValue(drink3));
        }


        [Test, Order(2)]
        public void CheckVendingMachineHasWhenNoDrinksProvided()
        {
            machine.FillRandomly(new List<Drink>());

            //Is not empty
            Assert.IsNotNull(machine.Inventory);

            //Check inventory/vending machine holds total 50 places
            Assert.IsTrue(machine.Inventory.Count == 50);

            int count = 0;
            foreach(Drink drink in machine.Inventory.Values) 
            {
                if (drink == null)
                {
                    count += 1;
                }
            }
            Assert.AreEqual(50, count);
        }


        [Test, Order(3)]
        public void CheckAddDrink() 
        {
            //Add Drink Coca Cola to row number 1 & column 3
            machine.AddDrink(drink1, 1, 3);

            //Get the Drink placed on row 1 & column 3
            var drink = machine.Inventory[(1, 3)];

            Assert.IsNotNull(drink);
            Assert.IsInstanceOf(typeof(Drink), drink);
            Assert.AreEqual(drink1, drink);
        }

        [TestCase(Coins.ONEEURO, ExpectedResult = (double)1),Order(5)]
        [TestCase(Coins.TWOEURO, ExpectedResult = (double)2)]
        [TestCase(Coins.FIVECENTS, ExpectedResult = (double)0.05)]
        [TestCase(Coins.TENCENTS, ExpectedResult = (double)0.1)]
        [TestCase(Coins.TWENTYCENTS, ExpectedResult = (double)0.2)]
        [TestCase(Coins.FIFTYCENTS, ExpectedResult = (double)0.5)]

        public double CheckInsertCoin(Coins coin) 
        {
            machine.InsertCoins(coin);
            return machine.Budget;
        }

        [Test, Order(6)]
        public void CheckRefund_When_NoPurchaseDone() 
        {
            machine.InsertCoins(Coins.ONEEURO);
            machine.InsertCoins(Coins.TWOEURO);

            var refundAmount = machine.Refund();

            Assert.AreEqual(3, refundAmount);
        }

        [Test, Order(7)]
        public void CheckRefund_When_AtleastOnePurchaseDone()
        {
            Drink cocaCola = new Drink("Coca Cola", 1.5);
            machine.AddDrink(cocaCola, 1, 1);

            machine.InsertCoins(Coins.ONEEURO);
            machine.InsertCoins(Coins.TWOEURO);

            machine.buy(1, 1);

            var refundAmount = machine.Refund();

            Assert.AreEqual(1.5, refundAmount);
        }

        [Test, Order(8)]
        public void CheckBuy_Success_Transaction() 
        {
            /*Before buy Add few drinks to the Vending machine at particular row & column*/
            Drink cocaCola = new Drink("Coca Cola", 8);
            machine.AddDrink(cocaCola, 1, 1);

            Drink fanta = new Drink("Fanta", 5);
            machine.AddDrink(fanta, 1, 2);

            Drink Pepsi = new Drink("Pepsi", 7);
            machine.AddDrink(Pepsi, 1, 3);


            /*Before buy customer has to insert few coins*/
            machine.InsertCoins(Coins.TWOEURO);
            machine.InsertCoins(Coins.TWOEURO);
            machine.InsertCoins(Coins.TWOEURO);
            machine.InsertCoins(Coins.TWOEURO);


            /*Place an order to buy*/
            Drink productPurchased = machine.buy(1, 2);
            double refundAmount = machine.Refund();


            /*Check the results according to your expectations*/
            Assert.AreEqual(fanta, productPurchased);
            Assert.AreEqual(3, refundAmount);
        }

        [Test, Order(8)]
        public void CheckBuy_When_InsertCoinsAreLessThanPrice()
        {
            /*Before buy Add few drinks to the Vending machine at particular row & column*/
            Drink cocaCola = new Drink("Coca Cola", 8);
            machine.AddDrink(cocaCola, 1, 1);

            Drink fanta = new Drink("Fanta", 5);
            machine.AddDrink(fanta, 1, 2);

            Drink Pepsi = new Drink("Pepsi", 7);
            machine.AddDrink(Pepsi, 1, 3);


            /*Before buy customer has to insert few coins*/
            machine.InsertCoins(Coins.TWOEURO);
            machine.InsertCoins(Coins.TWOEURO);
            machine.InsertCoins(Coins.TWOEURO);
            
            /*Place an order to buy*/
        
            /*Here we have inserted 6 coins but the product we are trying to purchase is for price 8 Euros*/
            var exception = Assert.Throws<NotEnougMoneyException>(() => machine.buy(1, 1));
            Assert.That(exception.Message, Is.EqualTo("Exception of type 'FrisdrankAutomaat.NotEnougMoneyException' was thrown."));
        }

        [Test, Order(9)]
        public void CheckBuy_When_WrongRowAndColumnNoProvided()
        {
            /*Before buy Add few drinks to the Vending machine at particular row & column*/
            Drink cocaCola = new Drink("Coca Cola", 8);
            machine.AddDrink(cocaCola, 1, 1);

            Drink fanta = new Drink("Fanta", 5);
            machine.AddDrink(fanta, 1, 2);

            Drink Pepsi = new Drink("Pepsi", 7);
            machine.AddDrink(Pepsi, 1, 3);


            /*Before buy customer has to insert few coins*/
            machine.InsertCoins(Coins.TWOEURO);
            machine.InsertCoins(Coins.TWOEURO);
            machine.InsertCoins(Coins.TWOEURO);
            machine.InsertCoins(Coins.TWOEURO);

            /*Place an order to buy*/

            /*Here we have inserted 8 coins to buy product of price 8 Euros, but here we are not providing correct row & column number*/
            var exception = Assert.Throws<IndexOutOfRangeException>(() => machine.buy(10, 10));
            Assert.That(exception.Message, Is.EqualTo("Index was outside the bounds of the array."));
        }

        [Test, Order(10)]
        public void CheckBuy_When_NoProductPresentAtRowAndColumnNumber()
        {
            /*Before buy Add few drinks to the Vending machine at particular row & column*/
            Drink cocaCola = new Drink("Coca Cola", 8);
            machine.AddDrink(cocaCola, 1, 1);

            Drink fanta = new Drink("Fanta", 5);
            machine.AddDrink(fanta, 1, 2);

            Drink Pepsi = new Drink("Pepsi", 7);
            machine.AddDrink(Pepsi, 1, 3);


            /*Before buy customer has to insert few coins*/
            machine.InsertCoins(Coins.TWOEURO);
            machine.InsertCoins(Coins.TWOEURO);
            machine.InsertCoins(Coins.TWOEURO);
            machine.InsertCoins(Coins.TWOEURO);

            /*Place an order to buy*/

            /*Here we have inserted 8 coins to buy product of price 8 Euros, but here we are not providing correct row & column number*/
            var exception = Assert.Throws<NoInventoryException>(() => machine.buy(2, 1));
            Assert.That(exception.Message, Is.EqualTo("Exception of type 'FrisdrankAutomaat.NoInventoryException' was thrown."));
        }
    }
}