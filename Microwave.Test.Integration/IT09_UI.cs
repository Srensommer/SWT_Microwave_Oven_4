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
    class IT09_UI
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
            _light = Substitute.For<ILight>();

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
        public void UserInterface_OnPowerPressed_Output50W()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);

            _output.Received().OutputLine("Display shows: 50 W");
        }

        [Test]
        public void UserInterface_OnPowerPressedTwice_Output100W()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _sut.OnPowerPressed(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Display shows: 100 W");
        }

        [Test]
        public void UserInterface_OnTimePressed_ShowsTime()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Display shows: 01:00");
        }

        [Test]
        public void UserInterface_OnTimePressedTwice_ShowsTime()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _sut.OnTimePressed(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Display shows: 02:00");
        }

        [Test]
        public void UserInterface_OnDoorOpened_DisplaysLightOn()
        {
            _sut.OnDoorOpened(new object(), EventArgs.Empty);

            _light.Received().TurnOn();
        }

        [Test]
        public void UserInterface_OnDoorClosed_DisplaysLightOff()
        {
            _sut.OnDoorOpened(new object(), EventArgs.Empty);

            _light.ClearReceivedCalls();
            _sut.OnDoorClosed(new object(), EventArgs.Empty);

            _light.Received().TurnOff();
        }

        [Test]
        public void UserInterface_StartButtonPressed_DoesStuff()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _sut.OnStartCancelPressed(new object(), EventArgs.Empty);


            _output.Received().OutputLine("PowerTube works with 7 %");
        }

        [Test]
        public void UserInterface_CancelPressedWhenCooking_LightsOffAndDisplayCleared()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);
            _sut.OnStartCancelPressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _light.ClearReceivedCalls();
            _sut.OnStartCancelPressed(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Display cleared");
            _light.TurnOff();
        }

        [Test]
        public void UserInterface_CancelPressedWhenSetPower_ClearDisplay()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _sut.OnStartCancelPressed(new object(), EventArgs.Empty);


            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_OpensDoordWhenSetPower_ClearDisplayAndLightsOn()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _light.ClearReceivedCalls();
            _sut.OnDoorOpened(new object(), EventArgs.Empty);


            _light.Received().TurnOn();
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_OpensDoordWhenSetTime_ClearDisplayAndLightsOn()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _light.ClearReceivedCalls();
            _sut.OnDoorOpened(new object(), EventArgs.Empty);


            _light.Received().TurnOn();
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_OpensDoorWhenCooking_ClearDisplayAndLightsOn()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);
            _sut.OnStartCancelPressed(new object(), EventArgs.Empty);

            _output.ClearReceivedCalls();
            _sut.OnDoorOpened(new object(), EventArgs.Empty);
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void UserInterface_CookingFinishes_ClearDisplayAndLightsOn()
        {
            _sut.OnPowerPressed(new object(), EventArgs.Empty);
            _sut.OnTimePressed(new object(), EventArgs.Empty);
            _sut.OnStartCancelPressed(new object(), EventArgs.Empty);

            Thread.Sleep(1000*65);
           _output.Received().OutputLine("Display cleared");
            _light.Received().TurnOn();
        }
    }
}
