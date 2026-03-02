using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_OOPGenericExam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Motorcycle:");
            MotorCycle m1 = new MotorCycle(model: "R15V5",yearMake: 2024,cc: 160,noOfGear: 6,vehicleType: VehicleType.Personal,startingMethod: "Self+Kick",maxPowerBHP: 250,maxTorqueNM: 22000,mileage: 45,cooling: "Air Cooled",frontBrake: "Disk",rearBrake: "ABS");
            m1.AddExteriorDesign("VIP Horn","LED Display", "LED Fog Light");
            GenericDetailImplement<MotorCycle> genericMotor = new GenericDetailImplement<MotorCycle>();
            Console.WriteLine(genericMotor.GetDetail(m1));
            Console.WriteLine(m1.GetExteriorDesign());
            Console.WriteLine();

            Console.WriteLine("Car:");
            Car c1 = new Car(model: "Land Cruiser",yearMake: 2017,cc: 1500,noOfGear: 6,vehicleType: VehicleType.Racing,noOfSeat: 5,noOfDoor: 5);
            c1.AddInteriorDesign("GPS", "Auto Drive", "Music System");
            VehicleDetailsImplementement<Car> vehicleCar = new VehicleDetailsImplementement<Car>();
            Console.WriteLine(vehicleCar.GetDetail(c1));
            Console.WriteLine(c1.GetInteriorDesign());
            Console.WriteLine();

            Console.ReadKey();
        }
    }
    //Enum
    public enum VehicleType { Personal = 1, Racing }
    //Abstract
    public abstract class Vehicle
    {
        protected Vehicle(string model, int yearMake, int cc, int noOfGear, VehicleType vehicleType)
        {
            Model = model;
            YearMake = yearMake;
            CC = cc;
            NoOfGear = noOfGear;
            VehicleType = vehicleType;
        }
        public string Model { get; set; }
        public int YearMake { get; set; }
        public int CC { get; set; }
        public int NoOfGear { get; set; }
        public VehicleType VehicleType { get; set; }
        public abstract string Details();
    }
    //Interfaces for Design
    public interface IExteriorDesign
    {
        void AddExteriorDesign(params string[] designs);
        string GetExteriorDesign();
    }
    public interface IInteriorDesign
    {
        void AddInteriorDesign(params string[] designs);
        string GetInteriorDesign();
    }
    //TwoWheeler
    public class TwoWheeler : Vehicle, IExteriorDesign
    {
        private readonly List<string> exteriorDesigns = new List<string>();
        protected TwoWheeler(string model, int yearMake, int cc, int noOfGear, VehicleType vehicleType, string startingMethod, int maxPowerBHP, int maxTorqueNM, int mileage, string cooling, string frontBrake, string rearBrake) : base(model, yearMake, cc, noOfGear, vehicleType)
        {
            StartingMethod = startingMethod;
            MaxPowerBHP = maxPowerBHP;
            MaxTorqueNM = maxTorqueNM;
            Mileage = mileage;
            Cooling = cooling;
            FrontBrake = frontBrake;
            RearBrake = rearBrake;
        }
        public string StartingMethod { get; set; }
        public int MaxPowerBHP { get; set; }
        public int MaxTorqueNM { get; set; }
        public int Mileage { get; set; }
        public string Cooling { get; set; }
        public string FrontBrake { get; set; }
        public string RearBrake { get; set; }
        public void AddExteriorDesign(params string[] designs)
        {
            this.exteriorDesigns.AddRange(designs);
        }
        public string GetExteriorDesign() => string.Join(", ", exteriorDesigns);
        public override string Details()
        {
            return $"{Model}, {CC}cc, Year: {YearMake}, Mileage: {Mileage} KMPL, Start: {StartingMethod}, Gear: {NoOfGear}, Cooling: {Cooling}, Brakes: {FrontBrake}/{RearBrake}, Type: {VehicleType}, Power: {MaxPowerBHP} BHP, Torque: {MaxTorqueNM} NM";
        }
    }
    // FourWheeler
    public class FourWheeler : Vehicle, IInteriorDesign
    {
        private readonly List<string> interiorDesigns = new List<string>();
        protected FourWheeler(string model, int yearMake, int cc, int noOfGear, VehicleType vehicleType, int noOfSeat, int noOfDoor) : base(model, yearMake, cc, noOfGear, vehicleType)
        {
            NoOfSeat = noOfSeat;
            NoOfDoor = noOfDoor;
        }
        public int NoOfSeat { get; set; }
        public int NoOfDoor { get; set; }
        public void AddInteriorDesign(params string[] designs)
        {
            this.interiorDesigns.AddRange(designs);
        }
        public string GetInteriorDesign() => string.Join(", ", interiorDesigns);
        public override string Details()
        {
            return $"{Model}, {CC}cc, Year: {YearMake}, Gear: {NoOfGear}, Seats: {NoOfSeat}, Doors: {NoOfDoor}, Type: {VehicleType}";
        }
    }
    //Sealed Classes
    public sealed class MotorCycle : TwoWheeler
    {
        public MotorCycle(string model, int yearMake, int cc, int noOfGear, VehicleType vehicleType, string startingMethod, int maxPowerBHP, int maxTorqueNM, int mileage, string cooling, string frontBrake, string rearBrake) : base(model, yearMake, cc, noOfGear, vehicleType, startingMethod, maxPowerBHP, maxTorqueNM, mileage, cooling, frontBrake, rearBrake)
        { 
        }
    }
    public sealed class Car : FourWheeler
    {
        public Car(string model, int yearMake, int cc, int noOfGear, VehicleType vehicleType, int noOfSeat, int noOfDoor) : base(model, yearMake, cc, noOfGear, vehicleType, noOfSeat, noOfDoor) 
        {
        }
    }
    //Generic Interfaces
    public interface IGenericDetail<T>
    {
        string GetDetail<T1>(T1 obj);
    }
    public class GenericDetailImplement<T> : IGenericDetail<T>
    {
        public string GetDetail<T1>(T1 obj)
        {
            return obj is Vehicle vehicle ? vehicle.Details() : "Not a Vehicle";
        }
    }
    public interface IVehicleDetail<T>
    {
        string GetDetail<T1>(T1 obj) where T1 : Vehicle;
    }
    public class VehicleDetailsImplementement<T> : IVehicleDetail<T>
    {
        public string GetDetail<T1>(T1 obj) where T1 : Vehicle
        {
            return obj.Details();
        }
    }
}