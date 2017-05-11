using Microsoft.FSharp.Core;
using Model;
using System;

namespace CSharpApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var car = new Car(4, "Supacars", Tuple.Create(1.5, 3.5));
            var bike = Vehicle.NewMotorbike("MyBike", 1.5);
            var motorcar = Vehicle.NewMotorcar(car);

            Functions.Describe(bike);
            Functions.Describe(motorcar);

            var someWheeledCar = Functions.CreateCar(4, "Supacars", 1.5, 3.5);
            var fourWheeledCar =
                Functions.CreateFourWheeledCar
                         .Invoke("Supacars")
                         .Invoke(1.5)
                         .Invoke(3.5);

            // Working with Options
            var optionalCar = FSharpOption<Car>.Some(car);
            var isNone = FSharpOption<Car>.get_IsNone(optionalCar);
            var isNotSome = FSharpOption<Car>.get_IsSome(optionalCar);

            // With extension methods
            var optionalCarExt = car.AsOption();
            var isNoneExt = optionalCarExt.IsNone();
            var isSomeExt = optionalCarExt.IsSome();
        }
    }

    public static class OptionExtensions
    {
        public static bool IsSome<T>(this FSharpOption<T> option) => FSharpOption<T>.get_IsSome(option);
        public static bool IsNone<T>(this FSharpOption<T> option) => FSharpOption<T>.get_IsNone(option);
        public static FSharpOption<T> AsOption<T>(this T item)
        {
            if (item == null)
                return FSharpOption<T>.None;
            return FSharpOption<T>.Some(item);
        }
    }
}
