namespace Model

//[<CLIMutable>] // experiment with this
/// A standard F# record of a Car.
type Car =
    { /// The number of wheels on the car.
      Wheels : int
      /// The brand of the car.
      Brand : string
      /// The x/y of the car in meters
      Dimensions : float * float }

/// A vehicle of some sort.
type Vehicle =
  /// A car is a type of vehicle.
| Motorcar of Car
  /// A bike is also a type of vehicle.
| Motorbike of Name:string * EngineSize:float

module Functions =
    /// Describes a vehicle.
    let Describe vehicle =
        match vehicle with
        | Motorcar c -> printfn "This is a car with %d wheels!" c.Wheels
        | Motorbike(_, size) -> printfn "This is a bike with engine %f" size

    let CreateCar wheels brand x y = { Wheels = wheels; Brand = brand; Dimensions = x, y }
    /// Creates a car with four wheels.
    let CreateFourWheeledCar = CreateCar 4