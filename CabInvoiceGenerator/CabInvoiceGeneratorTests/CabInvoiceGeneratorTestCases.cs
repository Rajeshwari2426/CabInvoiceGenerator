using CabInvoiceGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;



namespace CabInvoiceGeneratorTests
{

    [TestClass]
    public class CabInvoiceGeneratorTestCases
    {
        public GenerateCabInvoice generateNormalFare;

        [TestInitialize]
        public void SetUp()
        {
            generateNormalFare = new GenerateCabInvoice(RideType.NORMAL);
        }

        // TC1.1 Positive Testcase
        [TestMethod]
        [TestCategory("Calculate Fare")]
        [DataRow(1, 1.0, 11)]
        [DataRow(10, 15, 160)]
        public void GivenDistanceAndTimeReturnTotalFare(int time, double distance, double expected)
        {
            double actual = generateNormalFare.CalculateFare(time, distance);
            Assert.AreEqual(actual, expected);
        }

        // TC 1.2 Given Invalid Time And Distance Return Custom Exception
        [TestMethod]
        [TestCategory("Calculate Fare")]
        public void GivenInvalidTimeAndDistanceReturnCustomException()
        {
            var invalidTimeException = Assert.ThrowsException<CabInvoiceGeneratorException>(() => generateNormalFare.CalculateFare(0, 5));
            Assert.AreEqual(CabInvoiceGeneratorException.ExceptionType.INVALID_TIME, invalidTimeException.exceptionType);
            var invalidDistanceException = Assert.ThrowsException<CabInvoiceGeneratorException>(() => generateNormalFare.CalculateFare(12, -1));
            Assert.AreEqual(CabInvoiceGeneratorException.ExceptionType.INVALID_DISTANCE, invalidDistanceException.exceptionType);
        }
        // TC2.1 - Given multiple rides should return aggregate fare
        [TestMethod]
        [TestCategory("Multiple Rides")]
        public void GivenMultipleRidesReturnAggregateFare()
        {
            //Arrange
            double actual, expected = 320;
            Ride[] cabRides = { new Ride(10, 15), new Ride(10, 15) };
            //Act
            actual = generateNormalFare.CalculateAgreegateFare(cabRides);
            //Assert
            Assert.AreEqual(actual, expected);
        }

        // TC2.2 - given no rides return custom exception
        [TestMethod]
        [TestCategory("Multiple Rides")]
        public void GivenNoRidesReturnCustomException()
        {
            Ride[] cabRides = { };
            var nullRidesException = Assert.ThrowsException<CabInvoiceGeneratorException>(() => generateNormalFare.CalculateAgreegateFare(cabRides));
            Assert.AreEqual(CabInvoiceGeneratorException.ExceptionType.NULL_RIDES, nullRidesException.exceptionType);
        }
        [TestMethod]
        [TestCategory("Enhanced Invoice")]
        public void GivenNoOfRides_ShouldReturnEnhancedInvoice()
        {
            Ride[] cabRides = { new Ride(10, 15), new Ride(10, 15) };
            InvoiceSummary expected = new InvoiceSummary(cabRides.Length, 320);
            InvoiceSummary actual = generateNormalFare.CalculateAgregateFare(cabRides);
            Assert.AreEqual(expected, actual);
        }
    }
}
