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
    class IT14_Door_With_All
    {

        private IOutput _output;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private ILight _light;
        private ITimer _timer;
        private CookController _cookController;
        private IUserInterface _userInterface;
        private IButton _powerButton, _startCancelButton, _timeButton;
        private IDoor _sut;


        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<Output>();
            _powerTube = new PowerTube(_output);
            _display = new Display(_output);
            _light = new Light(_output);
            _timer = new Timer();
            _cookController = new CookController(_timer, _display, _powerTube);
            _sut = new Door();
            _powerButton = new Button();
            _startCancelButton = new Button();
            _timeButton = new Button();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _sut, _display, _light, _cookController);

            _cookController.UI = _userInterface;
        }

        [Test]
        public void CloseDoor()
        {
            _sut.Close();
            _output.Received().OutputLine("Light is turned off");
        }

        [Test]
        public void OpenDoor()
        {
            _sut.Open();
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void OpenDoorWhenCook()
        {
            _cookController.StartCooking(50, 50);
            _sut.Open();
            _output.Received().OutputLine("Light is turned off");
        }

    }
}
