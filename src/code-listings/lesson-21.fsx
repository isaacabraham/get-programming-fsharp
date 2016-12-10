// Listing 21.1
type SimpleDisk = { SizeGb : int }
type Computer =
    { Manufacturer : string
      Disks: SimpleDisk list }

let myPc =
    { Manufacturer = "Computers Inc."
      Disks =
        [ { SizeGb = 100 }
          { SizeGb = 250 }
          { SizeGb = 500 } ] }

// Listing 21.2
type Disk =
| HardDisk of RPM:int * Platters:int
| SolidState
| MMC of NumberOfPins:int

// Listing 21.3
let myHardDisk = HardDisk(RPM = 250, Platters = 7)
let myHardDiskShort = HardDisk(250, 7)
let args = 250, 7
let myHardDiskTupled = HardDisk args
let myMMC = MMC 5
let mySsd = SolidState

// Listing 21.4
let seek disk =
    match disk with
    | HardDisk _ -> "Seeking loudly at a reasonable speed!"
    | MMC _ -> "Seeking quietly but slowly"
    | SolidState -> "Already found it!"

seek mySsd

// Listing 21.5
let seekWithValues disk =
    match disk with
    | HardDisk(5400, 5) -> "Seeking very slowly!"
    | HardDisk(rpm, 7) -> sprintf "I have 7 spindles and RPM %d!" rpm
    | MMC 3 -> "Seeking. I have 3 pins!"

seekWithValues (MMC 3)

// Now you try #1
let describe disk =
  match disk with
  | SolidState -> "I'm a new-fangled SSD."
  | MMC 1 -> "I've only got one pin."
  | MMC pins when pins < 5 -> "I'm an MMC with a few pins."
  | MMC pins -> sprintf "I'm an MMC with %d pins" pins
  | HardDisk (5400, _) -> "I'm a slow hard disk"
  | HardDisk (_, 7) -> "I have 7 spindles!"
  | HardDisk _ -> "I'm a hard disk"

// Listing 21.6
type MMCDisk = | RsMmc | MmcPlus | SecureMMC
type DiskWithMmcData = | MMC of MMCDisk * NumberOfPins:int
let disk = MMC(MmcPlus, 3)
match disk with
| MMC(MmcPlus, 3) -> "Seeking quietly but slowly"
| MMC(SecureMMC, 6) -> "Seeking quietly with 6 pins."

// Listing 21.7
type DiskInfo =
    { Manufacturer : string
      SizeGb : int
      DiskData : Disk }
type FullComputer = { Manufacturer : string;  Disks : DiskInfo list }
let myFullPc =
    { Manufacturer = "Computers Inc."
      Disks =
        [ { Manufacturer = "HardDisks Inc."
            SizeGb = 100
            DiskData = HardDisk(5400, 7) }
          { Manufacturer = "SuperDisks Corp."
            SizeGb = 250
            DiskData = SolidState } ] }

// Listing 21.8
type Printer =
| Injket = 0
| Laserjet = 1
| DotMatrix = 2
