using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Ddd.Infrastructure;

namespace Ddd.Taxi.Domain
{
	// In real aplication it whould be the place where database is used to find driver by its Id.
	// But in this exercise it is just a mock to simulate database
	public class DriversRepository
	{
		public Driver FillDriverToOrder(int driverId, TaxiOrder order)
		{
			if (driverId == 15)
			{
				return new Driver(
					driverId,
					new PersonName("Drive", "Driverson"),
					new Car("A123BT 66", "Lada sedan", "Baklazhan"));
			}
			else
				throw new Exception("Unknown driver id " + driverId);
		}
	}

	public class TaxiApi : ITaxiApi<TaxiOrder>
	{
		private readonly DriversRepository driversRepo;
		private readonly Func<DateTime> currentTime;
		private int idCounter;

		public TaxiApi(DriversRepository driversRepo, Func<DateTime> currentTime)
		{
			this.driversRepo = driversRepo;
			this.currentTime = currentTime;
		}

		public TaxiOrder CreateOrderWithoutDestination(string firstName, string lastName, string street, string building)
		{
			return
				new TaxiOrder
				(
					idCounter++,
					new PersonName(firstName, lastName),
					new Address(street, building),
					currentTime()
				);
		}

		public void UpdateDestination(TaxiOrder order, string street, string building)
		{
			order.UpdateDestination(new Address(street, building));
		}

		public void AssignDriver(TaxiOrder order, int driverId)
		{
			var driver = driversRepo.FillDriverToOrder(driverId, order);
			order.AssignDriver(currentTime(), driver);
		}

		public void UnassignDriver(TaxiOrder order)
		{
			order.UnassignDriver();
		}

		public string GetDriverFullInfo(TaxiOrder order)
		{
			if (order.Status == TaxiOrderStatus.WaitingForDriver) return null;
			return string.Join(" ",
				"Id: " + order.Driver.Id,
				"DriverName: " + FormatName(order.Driver.PersonName.FirstName, order.Driver.PersonName.LastName),
				"Color: " + order.Driver.Car.Color,
				"CarModel: " + order.Driver.Car.Model,
				"PlateNumber: " + order.Driver.Car.PlateNumber);
		}

		public string GetShortOrderInfo(TaxiOrder order)
		{
			return string.Join(" ",
				"OrderId: " + order.Id,
				"Status: " + order.Status,
				"Client: " + FormatName(order?.ClientName?.FirstName, order?.ClientName?.LastName),
				"Driver: " + FormatName(order?.Driver?.PersonName.FirstName, order?.Driver?.PersonName.LastName),
				"From: " + FormatAddress(order?.Start.Street, order?.Start.Building),
				"To: " + FormatAddress(order?.Destination?.Street, order?.Destination?.Building),
				"LastProgressTime: " + GetLastProgressTime(order).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		}

		private DateTime GetLastProgressTime(TaxiOrder order)
		{
			if (order.Status == TaxiOrderStatus.WaitingForDriver) return order.CreationTime;
			if (order.Status == TaxiOrderStatus.WaitingCarArrival) return order.DriverAssignmentTime;
			if (order.Status == TaxiOrderStatus.InProgress) return order.StartRideTime;
			if (order.Status == TaxiOrderStatus.Finished) return order.FinishRideTime;
			if (order.Status == TaxiOrderStatus.Canceled) return order.CancelTime;
			throw new NotSupportedException(order.Status.ToString());
		}

		private string FormatName(string firstName, string lastName)
		{
			return string.Join(" ", new[] { firstName, lastName }.Where(n => n != null));
		}

		private string FormatAddress(string street, string building)
		{
			return string.Join(" ", new[] { street, building }.Where(n => n != null));
		}

		public void Cancel(TaxiOrder order)
		{
			order.Cancel(currentTime());
		}

		public void StartRide(TaxiOrder order)
		{
			order.StartRide(currentTime());
		}

		public void FinishRide(TaxiOrder order)
		{
			order.FinishRide(currentTime());
		}
    }

	public class TaxiOrder : Entity<int>
	{
		public PersonName ClientName { get; private set; }
        public Address Start { get; private set; }
        public Address Destination { get; private set; }
        public Driver Driver { get; private set; }
		public TaxiOrderStatus Status { get; private set; }
		public DateTime CreationTime { get; private set; }
		public DateTime DriverAssignmentTime { get; private set; }
		public DateTime CancelTime { get; private set; }
		public DateTime StartRideTime { get; private set; }
		public DateTime FinishRideTime { get; private set; }
		
		public TaxiOrder(int id, PersonName client, DateTime creationTime) : base(id)
		{
			ClientName = client;
			CreationTime = creationTime;
		}

		public TaxiOrder(int id, PersonName client, Address start, DateTime creationTime) 
			: this(id, client, creationTime)
		{
			Start = start;
		}

        public void Cancel(DateTime currentTime)
        {
			if (Status == TaxiOrderStatus.InProgress || Status == TaxiOrderStatus.Finished)
				throw new InvalidOperationException();

			Status = TaxiOrderStatus.Canceled;
            CancelTime = currentTime;
        }

        public void StartRide(DateTime currentTime)
        {
            if (Status != TaxiOrderStatus.WaitingCarArrival)
                throw new InvalidOperationException();

            Status = TaxiOrderStatus.InProgress;
            StartRideTime = currentTime;
        }

        public void FinishRide(DateTime currentTime)
        {
            if (Status != TaxiOrderStatus.InProgress)
                throw new InvalidOperationException();

            Status = TaxiOrderStatus.Finished;
            FinishRideTime = currentTime;
        }

		public void UpdateDestination(Address destination)
		{
			Destination = destination;
		}

        public void AssignDriver(DateTime currentTime, Driver driver)
        {
            if(Status != TaxiOrderStatus.WaitingForDriver)
				throw new InvalidOperationException();

			Driver = driver;
            DriverAssignmentTime = currentTime;
            Status = TaxiOrderStatus.WaitingCarArrival;
        }

        public void UnassignDriver()
        {
            if (Status == TaxiOrderStatus.WaitingForDriver || Status == TaxiOrderStatus.InProgress)
                throw new InvalidOperationException("Taxi order status is WaitingForDriver");

            Driver = new Driver(-1, new PersonName(null, null), new Car(null, null, null));
            Status = TaxiOrderStatus.WaitingForDriver;
        }
    }

	/// <summary>
	/// Information about driver include First Name, Last Name and using Car.
	/// </summary>
	public class Driver : Entity<int>
	{
		public PersonName PersonName { get; private set; }
		public Car Car { get; private set; }

		public Driver(int id, PersonName personName, Car car) : base(id)
		{
			PersonName = personName;
			Car = car;
		}
	}

	/// <summary>
	/// Information about car include its model and color.
	/// </summary>
	public class Car : ValueType<Car>
	{
        public string PlateNumber { get; private set; }
		public string Model { get; private set; }
        public string Color { get; private set; }

		public Car(string plateNumber, string model, string color)
		{
            PlateNumber = plateNumber;
			Model = model;
            Color = color;
		}
	}
}