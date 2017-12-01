using System;
using System.Diagnostics.CodeAnalysis;
using UtilityDelta.Gpio.Interfaces;

namespace UtilityDelta.Stepper
{
    /// <inheritdoc />
    /// <summary>
    /// Control the position of a 2, 4 or 5 pin stepper motor.
    /// When this object disposed, the motor will de-activate.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IStepperMotor : IDisposable
    {
        /// <summary>
        /// Move the stepper motor until a signal is received (value=On)
        /// from the signal pin. This could be a limit switch for example.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="signalPin">Will read the value of this pin on each step,
        /// when it reads true (on), this will return.</param>
        /// <param name="maxStepsToTake">Don't take any more steps than this - if still
        /// can't detect signal, give up.</param>
        /// <returns>Returns true if found signal during a complete rotation 
        /// of stepper motor. False if no signal was detected.</returns>
        bool SetInitialPosition(Direction direction, IGpioPin signalPin, int maxStepsToTake);

        /// <summary>
        /// Move the motor a number of steps clockwise or anticlockwise.
        /// It will continue from its current position, you can call this
        /// many times without losing the calibrated position.
        /// </summary>
        void Move(int steps, Direction direction);

        /// <summary>
        /// Determines how long to wait between steps. Don't set it
        /// to too fast, the motor may stall or miss steps entirely.
        /// </summary>
        /// <param name="revolutionsPerMinute">How many complete 360deg turns the stepper will perform in one minute.</param>
        void SetSpeed(int revolutionsPerMinute);
    }
}