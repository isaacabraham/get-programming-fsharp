// Listing 18.1
type Disk = { SizeGb : int }
type Computer =
    { Manufacturer : string
      Disks: Disk list }

let myPc =
    { Manufacturer = "Computers Inc."
      Disks =
        [ { SizeGb = 100 }
          { SizeGb = 250 }
          { SizeGb = 500 } ] }

