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
    public class IT06_CookController_With_Timer
    {
        private ICookController _sut;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IOutput _output;
        private IDisplay _display;

        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _timer = new MicrowaveOvenClasses.Boundary.Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _sut = new CookController(_timer, _display, _powerTube);
        }
        [Test]
        public void CookingControllerGetsTickEventAfter1Second()
        {
            //Vi bruger her ManualResetEvent. Vi er inspirerede af https://docs.microsoft.com/en-us/dotnet/api/system.threading.manualresetevent?view=netframework-4.8
            ManualResetEvent pause = new ManualResetEvent(false);
            _timer.TimerTick += (timer, tick) => pause.Set();
            _sut.StartCooking(50, 2);
            //Venter til vi får et tick, og tjekker derefter at vi rigtig nok har modtaget et event.
            Assert.AreEqual(true, pause.WaitOne(1100));
        }
        [Test]
        public void CookingControllerGetsNoTickEventAfter1MilliSecond()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            _timer.TimerTick += (timer, tick) => pause.Set();
            _sut.StartCooking(50, 2);
            //Venter, men ikke lang nok tid til vi får et tick, og tjekker derefter at vi rigtig nok ikke har modtaget et event.
            Assert.AreEqual(false, pause.WaitOne(1));
        }
    }
}