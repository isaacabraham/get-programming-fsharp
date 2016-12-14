module Car

open System

/// Gets the distance to a given destination 
let getDistance (destination) =
    if destination = "Gas" then 10
    elif destination = "Stadium" then 25
    elif destination = "Office" then 50
    elif destination = "Home" then 25
    else failwith "Unknown destination!"

/// Calculates remaining petrol after driving
let calculateRemainingPetrol(currentPetrol, distance) =
    let remainingPetrol = currentPetrol - distance
    if remainingPetrol >= 0 then remainingPetrol
    else failwith "Ooops! You ran out of petrol!"

/// Drives to a given destination given a starting amount of petrol
let driveTo (petrol, destination) =
    let distanceToNextDestination = getDistance destination
    let petrolAfterDriving = calculateRemainingPetrol(petrol, distanceToNextDestination)
    if destination = "Gas" then petrolAfterDriving + 50
    else petrolAfterDriving