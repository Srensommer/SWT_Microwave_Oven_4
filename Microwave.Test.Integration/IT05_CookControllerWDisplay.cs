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
    public class IT05_CookControllerWDisplay
    {
        private CookController cookController;
        private IDisplay display;
        private IPowerTube powerTube;
        private ITimer timer;
        private IOutput output;
        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            timer = Substitute.For<ITimer>();
            powerTube = new PowerTube(output);
            display = new Display(output);
            cookController = new CookController(timer, display, powerTube);

            timer.TimeRemaining.Returns(118);
        }

        [Test]
        public void CookingControllerMakesDisplayCallOutput()
        {
            cookController.StartCooking(50, 2);
            cookController.OnTimerTick(new object(), EventArgs.Empty);
            output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 01:58"));
        }
    }
}