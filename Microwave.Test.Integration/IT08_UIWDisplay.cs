using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT08_UIWDisplay
    {
        private IUserInterface userInterface;
        private IDisplay display;
        private IOutput output;
        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;
        private IDoor door;
        private ILight light;
        private ICookController cookController;
        [SetUp]
        public void Setup()
        {
            powerButton = Substitute.For<IButton>();
            output = Substitute.For<IOutput>();
            powerButton = Substitute.For<IButton>();
            timeButton = Substitute.For<IButton>();
            startCancelButton = Substitute.For<IButton>();
            door = Substitute.For<IDoor>();
            light = Substitute.For<ILight>();
            cookController = Substitute.For<ICookController>();

            display = new Display(output);
            userInterface = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cookController);
        }

        [Test]
        public void PowerButtonPressedOutputOutputsCorrect()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            output.Received(1).OutputLine("Display shows: 50 W");
        }
        [Test]
        public void MultiplePowerButtonPressedOutputOutputsCorrect()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            output.Received(1).OutputLine("Display shows: 100 W");
        }
        [Test]
        public void TimeButtonPressedBeforeSetPowerNothingInOutput()
        {
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            output.Received(0);
        }
        [Test]
        public void TimeButtonPressedAfterSetPowerOutputOutputs01()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            output.Received(1).OutputLine("Display shows: 01:00");
        }
        [Test]
        public void MultipleTimeButtonPressedAfterSetPowerOutputOutputs02()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            output.Received(1).OutputLine("Display shows: 02:00");
        }
        [Test]
        public void startCancelButtonPressedAfterSetPowerAndTimeButtonPressedCookControllerReceivesStartsCooking()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            cookController.Received().StartCooking(50,60);
        }
    }
}