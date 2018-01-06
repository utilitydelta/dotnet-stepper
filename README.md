# dotnet-stepper
This is a simple .NET Standard library for working with a stepper motor on linux devices. Mock out a IPinController and IGpioPin for your unit testing :)

## Usage
```c#

using System;
using System.Threading;
using UtilityDelta.Gpio.Implementation;
using UtilityDelta.Stepper;

namespace dotnet_stepper_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var pins = new PinController(new FileIo(), new ChipProPinMapper()))
            {
                using (var stepper = new StepperMotor(200,
                    pins.GetGpioPin("PE4"), pins.GetGpioPin("PE5"), pins.GetGpioPin("PE6"), pins.GetGpioPin("PE7")))
                {
                    stepper.SetSpeed(30);
                    var signalPin = pins.GetGpioPin("PE8");
                    Console.WriteLine("Move to initial pos");
                    stepper.SetInitialPosition(Direction.Anticlockwise, signalPin, 200);
                    Thread.Sleep(1000);
                    Console.WriteLine("200 clockwise");
                    stepper.Move(200, Direction.Clockwise);
                    Thread.Sleep(1000);
                    Console.WriteLine("200 Anticlockwise");
                    stepper.Move(200, Direction.Anticlockwise);
                    Thread.Sleep(1000);
                    Console.WriteLine("100 Clockwise");
                    stepper.Move(100, Direction.Clockwise);
                    Thread.Sleep(1000);
                    Console.WriteLine("50 Anticlockwise");
                    stepper.Move(50, Direction.Anticlockwise);
                }
            }
        }
    }
}


```
