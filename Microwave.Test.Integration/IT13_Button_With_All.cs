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
    class IT13_Button_With_All
    {
        private IOutput _output;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private ILight _light;
        private ITimer _timer1;
        private CookController _cookController;
        private IUserInterface _userInterface;
        private IButton _sut_powerButton, _sut_startCancelButton, _sut_timeButton;
        private IDoor _door;


        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _powerTube = new PowerTube(_output);
            _display = new Display(_output);
            _light = new Light(_output);
            _timer1 = new Timer();
            _cookController = new CookController(_timer1, _display, _powerTube);
            _door = new Door();
            _sut_powerButton = new Button();
            _sut_startCancelButton = new Button();
            _sut_timeButton = new Button();
            _userInterface = new UserInterface(_sut_powerButton, _sut_timeButton, _sut_startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void PressPowerButtonOncePrint50()
        {
            _sut_powerButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 50 W"));
        }

        [Test]
        public void PressPowerButtonTwicePrint100()
        {
            _sut_powerButton.Press();
            _sut_powerButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 100 W"));
        }


        [Test]
        public void PressTimeButtonOncePrint0100()
        {
            _sut_powerButton.Press();
            _sut_timeButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 01:00"));
        }

        [Test]
        public void PressTimeButtonTwicePrint0200()
        {
            _sut_powerButton.Press();
            _sut_timeButton.Press();
            _sut_timeButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 02:00"));
        }

        [Test] public void PressStartCancelButtonToStartCookingAfterPowerAndTimerPressed()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            _timer1.Expired += (sender, args) => pause.Set();

            _sut_powerButton.Press();
            _sut_timeButton.Press();
            _sut_startCancelButton.Press();


            _output.Received().OutputLine(Arg.Is<string>(x => x == "Light is turned on"));
            _output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube works with 7 %"));
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 01:00"));
            _output.ClearReceivedCalls();

            Thread.Sleep(1000*70);
            //Assert.That((pause.WaitOne(1000 * 125))); //60 sec bør være nok

           
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 00:59"));
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 00:01"));
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 00:00"));
            _output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube turned off"));
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display cleared"));
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Light is turned off"));
        }

        [Test]
        public void PressStartCancelButtonToStopCooking()
        {
            _sut_powerButton.Press();
            _sut_timeButton.Press();
            _sut_startCancelButton.Press();
            _sut_startCancelButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display cleared"));
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Light is turned off"));
            _output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube turned off"));
        }


        [Test]
        public void PressStartCancelButtonInPowerStateGoBackToReadyState()
        {
            _sut_powerButton.Press();
            _sut_startCancelButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display cleared"));
        }
    }
}