using System.Diagnostics.CodeAnalysis;

namespace UtilityDelta.Stepper
{
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IStepperMotor
    {
        void Move(int steps, Direction direction);
        void SetSpeed(int revolutionsPerMinute);
    }
}