using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidenceExamOop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MotorCycle m1 = new MotorCycle
            {
                Model = "Yamaha R15",
                YearMake = 2015,
                CC = 160,
                StartingMethod = "Self start",
                VehicleType = VehicleType.Personal,
                Mileage = 13.5
            };
            m1.AddExteriorFeatures("Fog Light", "Hydralic Horn");
            Console.WriteLine(m1.Details());
            Console.WriteLine(m1.GetExteriorFeatures());

            Console.WriteLine();
            MotorCycle m2 = new MotorCycle("TVS Metro", 2017, VehicleType.Personal, 125, "Kick Start", 60.00);
            m2.AddExteriorFeatures(new string[] { "Head Light", "Back light" });
            Console.WriteLine(m2.Details());
            Console.WriteLine(m2.GetExteriorFeatures());

            Console.WriteLine();
            Car c1 = new Car 
            { 
                Model = "Toyota XCorolla", 
                YearMake = 2019, 
                CC = 1500, 
                Doors = 4, 
                VehicleType = VehicleType.Personal 
            };
            c1.AddInteriorFeatures("Auto Gear", "GPS", "AC");
            Console.WriteLine(c1.Details());
            Console.WriteLine(c1.GetInteriorFeatures());

            Console.WriteLine();
            Car c2 = new Car("Ferrari", 2022, VehicleType.Racing, 2, 3500);
            c2.AddInteriorFeatures("Italian Leather Seats", "6 Part Dashboard", "Bend Alert");
            Console.WriteLine(c2.Details());
            Console.WriteLine(c2.GetInteriorFeatures());

            Console.WriteLine("===========INPUT CAR============");
            Console.Write("Model: ");
            string model = Console.ReadLine();
            Console.Write("Make year: ");
            int yearMake = int.Parse(Console.ReadLine());
            Console.Write("CC: ");
            int cc = int.Parse(Console.ReadLine());
            Console.Write("Number of doors: ");
            int doors = int.Parse(Console.ReadLine());
            Console.Write("Vehicle type [Racing, Personal, Mountain]: ");
            VehicleType vehicleType = (VehicleType)Enum.Parse(typeof(VehicleType), Console.ReadLine());         /////////
            Console.Write("Interior features [Separate by ,]: ");
            string[] features = Console.ReadLine().Split(',');                    /////////
            Car myCar = new Car(model, yearMake, vehicleType, doors, cc);                         /////////
            myCar.AddInteriorFeatures(features);            ////////

            Console.WriteLine();
            Console.WriteLine(myCar.Details());
            Console.WriteLine(myCar.GetInteriorFeatures());
            
            Console.ReadKey();
        }
    }
    public enum VehicleType { Racing = 1, Personal, Public }

    public abstract class Vehicle           //MotorCycle → TwoWheeler → Vehicle
    {
        public Vehicle() { }
        public Vehicle(string model, int yearMake, VehicleType vehicleType)
        {
            this.Model = model;
            this.YearMake = yearMake;
            this.VehicleType = vehicleType;
        }
        public string Model { get; set; }
        public int YearMake { get; set; }
        public VehicleType VehicleType { get; set; }
        public abstract string Details();
    }

    public class TwoWheeler : Vehicle
    {
        public TwoWheeler() { }
        public TwoWheeler(string model, int yearMake, VehicleType vehicleType, int cc, string startingMethod) : base(model, yearMake, vehicleType)
        {
            this.CC = cc;
            this.StartingMethod = startingMethod;
        }
        public int CC { get; set; }
        public string StartingMethod { get; set; }
        public override string Details()
        {
            return $"{VehicleType} vehicle\n{Model}, {YearMake}\n{CC}cc, {StartingMethod}";
        }
    }
    public interface IExteriorDesign
    {
        void AddExteriorFeatures(params string[] features);
        string GetExteriorFeatures();
    }
    public sealed class MotorCycle : TwoWheeler, IExteriorDesign
    {
        public MotorCycle() { }
        public MotorCycle(string model, int yearMake, VehicleType vehicleType, int cc, string startingMethod, double mileage) : base(model, yearMake, vehicleType, cc, startingMethod)
        {
            this.Mileage = mileage;
        }
        public double Mileage { get; set; }
        public override string Details()
        {
            return $"{base.Details()} Mileage: {Mileage}KMPL";
        }
        private string[] features;
        public void AddExteriorFeatures(params string[] features)
        {
            this.features = features;
        }
        public string GetExteriorFeatures()
        {
            return string.Join(",", features);
        }
    }

    public class FourWheeler : Vehicle
    {
        public FourWheeler() { }
        public FourWheeler(string model, int yearMake, VehicleType vehicleType, int doors, int cc) : base(model, yearMake, vehicleType)
        {
            this.Doors = doors;
            this.CC = cc;
        }

        public int Doors { get; set; }
        public int CC { get; set; }
        public override string Details()
        {
            return $"{VehicleType} car\n{Model}, {YearMake}\n{CC}cc, {Doors} doors";
        }
    }

    public sealed class Car : FourWheeler, IInteriorDesign
    {
        public Car() { }
        public Car(string model, int yearMake, VehicleType vehicleType, int doors, int cc) : base(model, yearMake, vehicleType, doors, cc)
        {

        }
        string[] features;
        public void AddInteriorFeatures(params string[] features)
        {
            this.features = features;
        }

        public string GetInteriorFeatures()
        {
            return string.Join(",", features);
        }
    }

    public interface IInteriorDesign
    {
        void AddInteriorFeatures(params string[] features);
        string GetInteriorFeatures();
    }
}
