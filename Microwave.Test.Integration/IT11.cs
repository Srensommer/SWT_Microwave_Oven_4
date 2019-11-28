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
    class IT11
    {
        private IDoor _door;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IUserInterface _userInterface;
        private ICookController _cookController;
        private ITimer _timer;
        private ILight _light;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _door = Substitute.For<IDoor>();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();

            _output = Substitute.For<IOutput>();

            _display = new Display(_output);
            _light = new Light(_output);

            _timer = new Timer();

            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);


            _userInterface = new UserInterface(
                _powerButton,
                _timeButton,
                _startCancelButton,
                _door,
                _display,
                _light,
                _cookController);
        }

        [Test]
        public void UserInterface_OnPowerPressed_Output50W()
        {
            _powerButton.Press();

            _output.Received().OutputLine("Display shows: 50 W");
        }

        [Test]
        public void UserInterface_OnPowerPressedTwice_Output100W()
        {
            _powerButton.Press();

            _output.ClearReceivedCalls();
            _powerButton.Press();

            _output.Received().OutputLine("Display shows: 100 W");
        }

        [Test]
        public void UserInterface_OnTimePressed_ShowsTime()
        {
            _powerButton.Press();
            _timeButton.Press();

            _output.Received().OutputLine("Display shows: 01:00");
        }

        [Test]
        public void UserInterface_OnTimePressedTwice_ShowsTime()
        {
            _powerButton.Press();
            _timeButton.Press();

            _output.ClearReceivedCalls();
            _timeButton.Press();


            _output.Received().OutputLine("Display shows: 02:00");
        }

        [Test]
        public void UserInterface_OnDoorOpened_DisplaysLightOn()
        {
            _userInterface.OnDoorOpened(new object(), EventArgs.Empty);

            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void UserInterface_OnDoorClosed_DisplaysLightOff()
        {
            _userInterface.OnDoorOpened(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _userInterface.OnDoorClosed(new object(), EventArgs.Empty);

            _output.Received().OutputLine("Light is turned off");
        }

        [Test]
        public void UserInterface_StartButtonPressed_DoesStuff()
        {
            _powerButton.Press();
            _timeButton.Press();

            _output.ClearReceivedCalls();
            _startCancelButton.Press();

            _output.Received().OutputLine("PowerTube works with 7 %");
        }

        [Test]
        public void UserInterface_CancelPressedWhenCooking_LightsOff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _output.ClearReceivedCalls();
            _startCancelButton.Press();

            _output.Received().OutputLine("Light is turned off");
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_CancelPressedWhenSetPower_ClearDisplay()
        {
            _powerButton.Press();

            _output.ClearReceivedCalls();
            _startCancelButton.Press();

            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_OpensDoorWhenSetPower_ClearDisplayAndLightsOn()
        {
            _powerButton.Press();

            _output.ClearReceivedCalls();
            _userInterface.OnDoorOpened(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Light is turned on");
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_OpensDoorWhenSetTime_ClearDisplayAndLightsOn()
        {
            _powerButton.Press();
            _timeButton.Press();

            _output.ClearReceivedCalls();
            _userInterface.OnDoorOpened(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Light is turned on");
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_OpensDoorWhenCooking_ClearDisplayAndLightsOn()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _output.ClearReceivedCalls();
            _userInterface.OnDoorOpened(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Light is turned on");
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_CookingFinishes_ClearDisplayAndLightsOn()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            for (int i = 0; i < 60; i++)
            {
                _timer.TimerTick += Raise.EventWith(new object(), EventArgs.Empty);
            }


            _output.Received().OutputLine("Light is turned on");
            _output.Received().OutputLine("Display cleared");
        }
    }
}
