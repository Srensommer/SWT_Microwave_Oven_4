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
    public class IT08_TimerWCookController
    {
        private ITimer timer;
        private ICookController cookController;
        private IPowerTube powerTube;
        private IOutput output;
        private IDisplay display;
        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            powerTube = new PowerTube(output);
            display = new Display(output);
            timer = new MicrowaveOvenClasses.Boundary.Timer();
            cookController = new CookController(timer, display, powerTube);
        }
        [Test]
        public void OnTimerTickCookControllerReceivedTickEvent()
        {
            cookController.StartCooking(50, 120);
            Thread.Sleep(1100);
            output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 01:59"));
        }
        [Test]
        public void OnTimerExpiredControllerReceivedStop()
        {
            cookController.StartCooking(50, 2);
            Thread.Sleep(2000);
            output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube turned off"));
            //TODO: Burde jeg teste med "light is turned off"?
        }
    }
}