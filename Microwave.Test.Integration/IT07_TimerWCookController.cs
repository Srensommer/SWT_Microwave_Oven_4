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
    public class IT07_TimerWCookController
    {
        private ITimer timer;
        private ICookController cookController;
        private IPowerTube powerTube;
        private IOutput output;
        private IDisplay display;

        [SetUp]
        public void Setup()
        {
            timer = Substitute.For<ITimer>();
            output = Substitute.For<IOutput>();
            display = Substitute.For<IDisplay>();
            powerTube = Substitute.For<PowerTube>(output);
            cookController = Substitute.For<CookController>(timer, display, powerTube);
        }
        [Test]
        public void OnTimerTickCookControllerReceivedTickEvent()
        {
            cookController.StartCooking(50, 1);

            Thread.Sleep(1500);
            //Make sure we Receive OnTimerTick event
        }
        [Test]
        public void OnTimerExpiredControllerReceivedStop()
        {
            cookController.StartCooking(50, 2);
            Thread.Sleep(2500);
            cookController.Received(1).Stop();
        }
        [Test]
        public void OnTimerExpiredControllerReceivedExpiredEvent()
        {
            cookController.StartCooking(50, 2);
            Thread.Sleep(2500);
            //Make sure we received timer event
        }
    }
}