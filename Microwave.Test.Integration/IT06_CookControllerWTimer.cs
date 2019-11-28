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
    public class IT06_CookControllerWTimer
    {
        private ICookController cookController;
        private IPowerTube powerTube;
        private ITimer timer;
        private IOutput output;
        private IDisplay display;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            timer = new MicrowaveOvenClasses.Boundary.Timer();
            display = new Display(output);
            powerTube = new PowerTube(output);
            cookController = new CookController(timer, display, powerTube);
        }
        [Test]
        public void CookingControllerGetsTickEventAfter1Second()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            timer.TimerTick += (timer, tick) => pause.Set();
            cookController.StartCooking(50, 2);
            //Venter til vi får et tick, og tjekker derefter at vi rigtig nok har modtaget et event.
            Assert.AreEqual(true, pause.WaitOne(1100));

        }
        [Test]
        public void CookingControllerGetsNoTickEvent()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            timer.TimerTick += (timer, tick) => pause.Set();
            cookController.StartCooking(50, 2);
            //Venter, men ikke lang nok tid til vi får et tick, og tjekker derefter at vi rigtig nok ikke har modtaget et event.
            Assert.AreEqual(false, pause.WaitOne(1));

        }
    }
}