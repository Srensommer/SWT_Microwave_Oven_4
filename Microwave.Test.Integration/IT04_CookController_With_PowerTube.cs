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
    public class IT04_CookController_With_PowerTube
    {
        private ICookController _sut;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IOutput _output;
        private IDisplay _display;

        [SetUp]
        public void Setup()
        {
            _timer = Substitute.For<ITimer>();
            _display = Substitute.For<IDisplay>();
            _output = Substitute.For<IOutput>();
            _powerTube = new PowerTube(_output);
            _sut = new CookController(_timer, _display, _powerTube);
        }

        [Test]
        public void CookControllerStartCookingCorrectPrintOut()
        {
            _sut.StartCooking(50, 2);
            _output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube works with 7 %"));
        }
        [Test]
        public void CookControllerStartStopCookingCorrectPrintOut()
        {
            _sut.StartCooking(50, 1);
            _sut.Stop();
            _output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube turned off"));
        }

    }
}