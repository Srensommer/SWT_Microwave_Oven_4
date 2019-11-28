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

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT08_Timer_With_CookController
    {
        private ITimer _sut;
        private CookController _cookController;
        private IPowerTube _powerTube;
        private IOutput _output;
        private IDisplay _display;
        private IUserInterface _userInterface;
        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _powerTube = new PowerTube(_output);
            _display = new Display(_output);
            _sut = new MicrowaveOvenClasses.Boundary.Timer();
            _cookController = new CookController(_sut, _display, _powerTube);
            _userInterface = Substitute.For<IUserInterface>();
            _cookController.UI = _userInterface;
        }
        [Test]
        public void OnTimerTickCookControllerReceivedTickEvent()
        {
            _cookController.StartCooking(50, 120);
            Thread.Sleep(1100);
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 01:59"));
        }
        [Test]
        public void OnTimerExpiredControllerReceivedStop()
        {
            _cookController.StartCooking(50, 2);
            Thread.Sleep(2100);
            _output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube turned off"));
        }
    }
}