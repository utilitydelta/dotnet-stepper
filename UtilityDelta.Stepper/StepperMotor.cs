using System;
using System.Diagnostics;
using UtilityDelta.Gpio.Interfaces;

namespace UtilityDelta.Stepper
{
    public class StepperMotor : IStepperMotor
    {
        private const string StepNbrError = "Specify number of steps > 0 for stepper.";
        private const int DefaultRevsPerMinute = 10;
        private readonly int _nbrSteps;
        private readonly IGpioPin[] _pins;
        private readonly bool[][] _positions;

        private readonly Stopwatch _timer = new Stopwatch();
        private long _lastStepTime;

        private int _position = -1;
        private long _stepDelayInMilliseconds;

        public StepperMotor(int nbrSteps, IGpioPin pin1, IGpioPin pin2, IGpioPin pin3, IGpioPin pin4, IGpioPin pin5)
        {
            if (nbrSteps <= 0) throw new Exception(StepNbrError);
            _nbrSteps = nbrSteps;

            _pins = new[] {pin1, pin2, pin3, pin4, pin5};
            _positions = new[]
            {
                new[] {false, true, true, false, true},
                new[] {false, true, false, false, true},
                new[] {false, true, false, true, true},
                new[] {false, true, false, true, false},
                new[] {true, true, false, true, false},
                new[] {true, false, false, true, false},
                new[] {true, false, true, true, false},
                new[] {true, false, true, false, false},
                new[] {true, false, true, false, true},
                new[] {false, false, true, false, true}
            };

            SetSpeed(DefaultRevsPerMinute);
        }

        public StepperMotor(int nbrSteps, IGpioPin pin1, IGpioPin pin2, IGpioPin pin3, IGpioPin pin4)
        {
            if (nbrSteps <= 0) throw new Exception(StepNbrError);
            _nbrSteps = nbrSteps;

            _pins = new[] {pin1, pin2, pin3, pin4};
            _positions = new[]
            {
                new[] {true, false, true, false},
                new[] {false, true, true, false},
                new[] {false, true, false, true},
                new[] {true, false, false, true}
            };

            SetSpeed(DefaultRevsPerMinute);
        }

        public StepperMotor(int nbrSteps, IGpioPin pin1, IGpioPin pin2)
        {
            if (nbrSteps <= 0) throw new Exception(StepNbrError);
            _nbrSteps = nbrSteps;

            _pins = new[] {pin1, pin2};
            _positions = new[]
            {
                new[] {false, true},
                new[] {true, true},
                new[] {true, false},
                new[] {false, false}
            };

            SetSpeed(DefaultRevsPerMinute);
        }

        public void SetSpeed(int revolutionsPerMinute)
        {
            if (revolutionsPerMinute <= 0) throw new Exception("Revolutions Per Minute must be greater than 0.");
            _stepDelayInMilliseconds = 60L * 1000L / _nbrSteps / revolutionsPerMinute;
        }

        public void Move(int steps, Direction direction)
        {
            if (!_timer.IsRunning)
            {
                _timer.Start();
                _lastStepTime = -1 * _stepDelayInMilliseconds;
            }

            while (steps > 0)
            {
                //Continue to spin in this while loop until we pass the delay time span
                if (_timer.ElapsedMilliseconds - _stepDelayInMilliseconds < _lastStepTime) continue;

                if (direction == Direction.Clockwise)
                    _position++;
                else
                    _position--;

                SetToNextPosition();

                steps--;

                _lastStepTime = _timer.ElapsedMilliseconds;
            }
        }

        private void SetToNextPosition()
        {
            if (_position >= _positions.Length)
                _position = 0;
            if (_position < 0)
                _position = _positions.Length - 1;
            for (var i = 0; i < _pins.Length; i++)
                _pins[i].PinValue = _positions[_position][i];
        }
    }
}