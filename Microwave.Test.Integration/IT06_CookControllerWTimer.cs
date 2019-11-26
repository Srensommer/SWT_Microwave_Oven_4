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
    public class IT06_CookControllerWTimer
    {
        private CookController cookController;
        private IPowerTube powerTube;
        private ITimer timer;
        private IOutput output;
        private IDisplay display;

        [SetUp]
        public void Setup()
        {
            timer = Substitute.For<ITimer>();
            output = Substitute.For<IOutput>();
            display = new Display(output);
            powerTube = Substitute.For<PowerTube>(output);
            cookController = new CookController(timer, display, powerTube);
        }

        [Test]
        public void CookControllerStartCooking2CorrectCallStart2()
        {
            cookController.StartCooking(50, 2);
            timer.Received().Start(2);
        }
        [Test]
        public void CookControllerStartStopCookingCorrectStopCall()
        {
            cookController.StartCooking(50, 2);
            cookController.Stop();
            timer.Received().Stop();
        }
    }
}