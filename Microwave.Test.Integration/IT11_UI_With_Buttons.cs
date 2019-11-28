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
    class IT11_UI_With_Buttons
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
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();

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
        public void UserInterface_StartButtonPressed_DoesStuff()
        {
            _powerButton.Press();
            _timeButton.Press();

            _output.ClearReceivedCalls();
            _startCancelButton.Press();

            _output.Received().OutputLine("PowerTube works with 7 %");
        }

        [Test]
        public void UserInterface_CancelPressedWhenSetPower_ClearDisplay()
        {
            _powerButton.Press();

            _output.ClearReceivedCalls();
            _startCancelButton.Press();

            _output.Received().OutputLine("Display cleared");
        }
    }
}
