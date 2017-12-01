using System;
using System.Diagnostics;
using Moq;
using UtilityDelta.Gpio.Interfaces;
using Xunit;

namespace UtilityDelta.Stepper.Test
{
    public class TestStepper
    {
        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        public void TestMovementTiming(int revsPerMinute)
        {
            var pin1 = new Mock<IGpioPin>();
            var pin2 = new Mock<IGpioPin>();

            var stepper = new StepperMotor(200, pin1.Object, pin2.Object);

            var timer = new Stopwatch();
            timer.Start();
            stepper.SetSpeed(revsPerMinute);
            var expectedDelay = revsPerMinute == 10 ? 30 : 15;

            stepper.Move(1, Direction.Clockwise);
            var elapsed1 = timer.ElapsedMilliseconds;
            Assert.InRange(elapsed1, 0, expectedDelay);

            stepper.Move(1, Direction.Clockwise);
            var elapsed2 = timer.ElapsedMilliseconds;
            Assert.InRange(elapsed2, elapsed1 + expectedDelay - 1, elapsed1 + 35);

            stepper.Move(10, Direction.Clockwise);
            var elapsed3 = timer.ElapsedMilliseconds;
            Assert.InRange(elapsed3, elapsed2 + expectedDelay * 10 - 1, elapsed2 + expectedDelay * 10 + 5);

            stepper.Move(5, Direction.Anticlockwise);
            var elapsed4 = timer.ElapsedMilliseconds;
            Assert.InRange(elapsed4, elapsed3 + 5 * expectedDelay - 1, elapsed3 + 5 * expectedDelay + 5);
        }


        [Theory]
        [InlineData(Direction.Clockwise)]
        [InlineData(Direction.Anticlockwise)]
        public void TestMovementTwoPin(Direction direction)
        {
            var pin1 = new Mock<IGpioPin>();
            var pin2 = new Mock<IGpioPin>();

            Assert.Throws<Exception>(() =>
                new StepperMotor(0, pin1.Object, pin2.Object));
            var stepper = new StepperMotor(200, pin1.Object, pin2.Object);

            stepper.Move(1, direction);
            if (direction == Direction.Clockwise)
            {
                pin1.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin2.VerifySet(x => x.PinValue = true, Times.Exactly(1));
            }
            else
            {
                pin1.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin2.VerifySet(x => x.PinValue = false, Times.Exactly(1));
            }
            stepper.Move(1, direction);
            if (direction == Direction.Clockwise)
            {
                pin1.VerifySet(x => x.PinValue = true, Times.Exactly(1));
                pin2.VerifySet(x => x.PinValue = true, Times.Exactly(2));
            }
            else
            {
                pin1.VerifySet(x => x.PinValue = true, Times.Exactly(1));
                pin2.VerifySet(x => x.PinValue = false, Times.Exactly(2));
            }
        }


        [Theory]
        [InlineData(Direction.Clockwise)]
        [InlineData(Direction.Anticlockwise)]
        public void TestMovementFourPin(Direction direction)
        {
            var pin1 = new Mock<IGpioPin>();
            var pin2 = new Mock<IGpioPin>();
            var pin3 = new Mock<IGpioPin>();
            var pin4 = new Mock<IGpioPin>();

            Assert.Throws<Exception>(() =>
                new StepperMotor(0, pin1.Object, pin2.Object, pin3.Object, pin4.Object));
            var stepper = new StepperMotor(10, pin1.Object, pin2.Object, pin3.Object, pin4.Object);

            stepper.Move(1, direction);
            if (direction == Direction.Clockwise)
            {
                pin1.VerifySet(x => x.PinValue = true, Times.Exactly(1));
                pin2.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin3.VerifySet(x => x.PinValue = true, Times.Exactly(1));
                pin4.VerifySet(x => x.PinValue = false, Times.Exactly(1));
            }
            else
            {
                pin1.VerifySet(x => x.PinValue = true, Times.Exactly(1));
                pin2.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin3.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin4.VerifySet(x => x.PinValue = true, Times.Exactly(1));
            }
            stepper.Move(1, direction);
            if (direction == Direction.Clockwise)
            {
                pin1.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin2.VerifySet(x => x.PinValue = true, Times.Exactly(1));
                pin3.VerifySet(x => x.PinValue = true, Times.Exactly(2));
                pin4.VerifySet(x => x.PinValue = false, Times.Exactly(2));
            }
            else
            {
                pin1.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin2.VerifySet(x => x.PinValue = true, Times.Exactly(1));
                pin3.VerifySet(x => x.PinValue = false, Times.Exactly(2));
                pin4.VerifySet(x => x.PinValue = true, Times.Exactly(2));
            }
        }


        [Theory]
        [InlineData(Direction.Clockwise)]
        [InlineData(Direction.Anticlockwise)]
        public void TestMovementFivePin(Direction direction)
        {
            var pin1 = new Mock<IGpioPin>();
            var pin2 = new Mock<IGpioPin>();
            var pin3 = new Mock<IGpioPin>();
            var pin4 = new Mock<IGpioPin>();
            var pin5 = new Mock<IGpioPin>();

            Assert.Throws<Exception>(() =>
                new StepperMotor(0, pin1.Object, pin2.Object, pin3.Object, pin4.Object, pin5.Object));
            var stepper = new StepperMotor(1000, pin1.Object, pin2.Object, pin3.Object, pin4.Object, pin5.Object);

            stepper.Move(1, direction);
            if (direction == Direction.Clockwise)
            {
                pin1.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin2.VerifySet(x => x.PinValue = true, Times.Exactly(1));
                pin3.VerifySet(x => x.PinValue = true, Times.Exactly(1));
                pin4.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin5.VerifySet(x => x.PinValue = true, Times.Exactly(1));
            }
            else
            {
                pin1.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin2.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin3.VerifySet(x => x.PinValue = true, Times.Exactly(1));
                pin4.VerifySet(x => x.PinValue = false, Times.Exactly(1));
                pin5.VerifySet(x => x.PinValue = true, Times.Exactly(1));
            }
        }


        [Fact]
        public void TestSpeedException()
        {
            var pin1 = new Mock<IGpioPin>();
            var pin2 = new Mock<IGpioPin>();

            var stepper = new StepperMotor(200, pin1.Object, pin2.Object);

            Assert.Throws<Exception>(() => stepper.SetSpeed(-1));
            Assert.Throws<Exception>(() => stepper.SetSpeed(0));
        }


        [Fact]
        public void TestNbrSteps()
        {
            var pin1 = new Mock<IGpioPin>();
            var pin2 = new Mock<IGpioPin>();

            var stepper = new StepperMotor(200, pin1.Object, pin2.Object);
            
            stepper.Move(5, Direction.Clockwise);

            pin1.VerifySet(x => x.PinValue = It.IsAny<bool>(), Times.Exactly(5));
            pin2.VerifySet(x => x.PinValue = It.IsAny<bool>(), Times.Exactly(5));

            stepper.Move(15, Direction.Anticlockwise);

            pin1.VerifySet(x => x.PinValue = It.IsAny<bool>(), Times.Exactly(20));
            pin2.VerifySet(x => x.PinValue = It.IsAny<bool>(), Times.Exactly(20));
        }

        [Fact]
        public void TestInitialPosition()
        {
            var pin1 = new Mock<IGpioPin>();
            var pin2 = new Mock<IGpioPin>();
            var signal = new Mock<IGpioPin>();

            var stepper = new StepperMotor(200, pin1.Object, pin2.Object);

            var result1 = stepper.SetInitialPosition(Direction.Anticlockwise, signal.Object, 10);
            Assert.False(result1);
            signal.VerifyGet(x=>x.PinValue, Times.Exactly(10));
            pin1.VerifySet(x => x.PinValue = It.IsAny<bool>(), Times.Exactly(10));

            signal.Setup(x => x.PinValue).Returns(true);

            var result2 = stepper.SetInitialPosition(Direction.Clockwise, signal.Object, 100);
            Assert.True(result2);

            signal.VerifyGet(x => x.PinValue, Times.Exactly(11));
            pin1.VerifySet(x => x.PinValue = It.IsAny<bool>(), Times.Exactly(10));
        }

        [Fact]
        public void TestDispose()
        {
            var pin1 = new Mock<IGpioPin>();
            var pin2 = new Mock<IGpioPin>();

            using (var stepper = new StepperMotor(200, pin1.Object, pin2.Object))
            {
                
            }

            pin1.VerifySet(x => x.PinValue = false, Times.Once);
            pin2.VerifySet(x => x.PinValue = false, Times.Once);
        }
    }
}