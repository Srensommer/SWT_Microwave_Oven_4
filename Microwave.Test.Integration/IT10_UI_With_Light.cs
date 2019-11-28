using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT10
    {
        private IDoor _door;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IUserInterface _sut;
        private CookController _cookController;
        private ITimer _timer;
        private ILight _light;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _door = Substitute.For<IDoor>();
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();

            _output = Substitute.For<IOutput>();

            _display = new Display(_output);
            _light = new Light(_output);

            _timer = new Timer();

            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);


            _sut = new UserInterface(
                _powerButton,
                _timeButton,
                _startCancelButton,
                _door,
                _display,
                _light,
                _cookController);

            _cookController.UI = _sut;
        }


        [Test]
        public void UserInterface_OnDoorOpened_DisplaysLightOn()
        {
            _sut.OnDoorOpened(new object(), EventArgs.Empty);

            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void UserInterface_OnDoorClosed_DisplaysLightOff()
        {
            _sut.OnDoorOpened(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _sut.OnDoorClosed(new object(), EventArgs.Empty);

            _output.Received().OutputLine("Light is turned off");
        }

        [Test]
        public void UserInterface_CancelPressedWhenCooking_LightsOff()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);
            _sut.OnStartCancelPressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _sut.OnStartCancelPressed(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Light is turned off");
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_OpensDoorWhenStateIsSetPower_ClearDisplayAndLightsOn()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _sut.OnDoorOpened(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Light is turned on");
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_OpensDoorWhenStateIsSetTime_ClearDisplayAndLightsOn()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _sut.OnDoorOpened(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Light is turned on");
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_OpensDoorWhenStateIsCooking_ClearDisplayAndLightsOn()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);
            _sut.OnStartCancelPressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _sut.OnDoorOpened(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Light is turned on");
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_CookingFinishes_ClearDisplayAndLightsOn()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);
            _sut.OnStartCancelPressed(new object(), EventArgs.Empty);

            Thread.Sleep(1000*65);

            _output.Received().OutputLine("Light is turned on");
            _output.Received().OutputLine("Display cleared");
        }
        [Test]
        public void UserInterface_ChangeStateToCooking_LightsOn()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);
            _sut.OnStartCancelPressed(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Light is turned on");
        }

    }
}
