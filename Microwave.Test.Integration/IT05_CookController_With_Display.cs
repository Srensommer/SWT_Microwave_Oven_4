using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;


namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT05_CookController_With_Display
    {
        private CookController _sut;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IOutput _output;
        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _timer = Substitute.For<ITimer>();
            _powerTube = new PowerTube(_output);
            _display = new Display(_output);
            _sut = new CookController(_timer, _display, _powerTube);

            _timer.TimeRemaining.Returns(10);
        }

        [Test]
        public void CookingControllerCallsDisplayWithShowTimerDisplayCallOutput()
        {
            _sut.StartCooking(50, 11);
            _sut.OnTimerTick(new object(), EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 00:10"));
        }
    }
}