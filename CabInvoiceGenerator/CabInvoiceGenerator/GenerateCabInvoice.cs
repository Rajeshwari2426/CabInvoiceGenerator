using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabInvoiceGenerator
{
    public class GenerateCabInvoice
    {
        public  int MINIMUM_FARE;
        public  int COST_PER_KM;
        public  int COST_PER_MINUTE;
        private Ride[] cabRides;
        Dictionary<int, List<Ride>> userCabRides;
        Dictionary<int, InvoiceSummary> userCabInvoice;

        public GenerateCabInvoice(RideType type)
        {
            if (type.Equals(RideType.NORMAL))
            {
                COST_PER_KM =10;
                COST_PER_MINUTE = 1;
                MINIMUM_FARE = 5;
            }
        }
        //Method to Calculate Fare
        public double CalculateFare(int time, double distance)
        {
            try
            {
                if (time <= 0)
                    throw new CabInvoiceGeneratorException(CabInvoiceGeneratorException.ExceptionType.INVALID_TIME, "Invalid Time");
                if (distance <= 0)
                    throw new CabInvoiceGeneratorException(CabInvoiceGeneratorException.ExceptionType.INVALID_DISTANCE, "Invalid Distance");
                //Fare for single ride
                double totalFare = (distance * COST_PER_KM) + (time * COST_PER_MINUTE);
                //Comparing minimum fare and calculated fare to return the maximum fare
                return Math.Max(totalFare, MINIMUM_FARE);
            }
            catch (CabInvoiceGeneratorException ex)
            {
                throw ex;
            }
        }
        //  Method to calculate agregate fare for multiple rides
        public double CalculateAgreegateFare(Ride[] rides)
        {
            double totalFare = 0;
            if (rides.Length == 0)
                throw new CabInvoiceGeneratorException(CabInvoiceGeneratorException.ExceptionType.NULL_RIDES, "No Rides Found");
            foreach (var ride in rides)
                totalFare += CalculateFare(ride.time, ride.distance);
            double agreegateFare = Math.Max(totalFare, MINIMUM_FARE);
            return agreegateFare;
        }
        //Method to Genetare Enhanced Invoice.
        public InvoiceSummary CalculateAgregateFare(Ride[] rides)
        {
          //  InvoiceSummary agregateFare = null;
            double totalFare=0;
            if (rides.Length == 0)
                throw new CabInvoiceGeneratorException(CabInvoiceGeneratorException.ExceptionType.NULL_RIDES, "No Rides Found");
            foreach (var ride in rides)
            {
                totalFare += CalculateFare(ride.time, ride.distance);
                double agregateFare = Math.Max(totalFare, MINIMUM_FARE);
                totalFare=agregateFare;
            }
           
             return new InvoiceSummary(rides.Length, totalFare);
        }
        //Method to Get Invoice For a user

        public GenerateCabInvoice()
        {
            this.userCabRides = new Dictionary<int, List<Ride>>();
            this.userCabInvoice = new Dictionary<int, InvoiceSummary>();
        }

        //UC4 - Method to store invoice of all rides by user id
        public void AddUserRidesToRepository(int userId, Ride[] rides, RideType rideType)
        {
            bool rideList = userCabRides.ContainsKey(userId);
            try
            {
                if (!rideList)
                {
                    List<Ride> list = new List<Ride>();
                    list.AddRange(rides);
                    this.userCabRides.Add(userId, list);
                     GenerateCabInvoice generateCabInvoice = new GenerateCabInvoice(rideType);
                     InvoiceSummary invoiceSummary =this.CalculateAgregateFare(rides);                  
                    userCabInvoice.Add(userId, invoiceSummary);
                }
            }
            catch (CabInvoiceGeneratorException)
            {
                throw new CabInvoiceGeneratorException(CabInvoiceGeneratorException.ExceptionType.NULL_RIDES, "No Rides Found");
            }
        }

        public User ReturnInvoicefromRidesList(int userId)
        {
            return new User(userCabRides[userId], userCabInvoice[userId]);
        }
    }
}
