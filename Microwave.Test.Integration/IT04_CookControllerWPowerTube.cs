﻿using System;
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
    public class IT04_CookControllerWPowerTube
    {
        private ICookController cookController;
        private IPowerTube powerTube;
        private ITimer timer;
        private IOutput output;
        private IDisplay display;

        [SetUp]
        public void Setup()
        {
            timer = Substitute.For<ITimer>();
            display = Substitute.For<IDisplay>();
            output = Substitute.For<IOutput>();
            powerTube = new PowerTube(output);
            cookController = new CookController(timer, display, powerTube);
        }

        [Test]
        public void CookControllerStartCookingCorrectPrintOut()
        {
            cookController.StartCooking(50, 2);
            output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube works with 7 %"));
        }
        [Test]
        public void CookControllerStartStopCookingCorrectPrintOut()
        {
            cookController.StartCooking(50, 1);
            cookController.Stop();
            output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube turned off"));
        }
    }
}