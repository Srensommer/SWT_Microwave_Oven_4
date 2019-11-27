using System;
using System.Collections.Generic;
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
    public class IT05_CookControllerWUI
    {
        private CookController cookController;
        private IUserInterface userInterface;
        private IDisplay display;
        private IPowerTube powerTube;
        private ITimer timer;
        [SetUp]
        public void Setup()
        {
            timer = Substitute.For<ITimer>();
            display = Substitute.For<IDisplay>();
            powerTube = Substitute.For<IPowerTube>();
            cookController = new CookController(timer, display, powerTube);
            userInterface = Substitute.For<IUserInterface>();
            cookController.UI = userInterface;
        }

        [Test]
        public void CookingControllerCallsUIWithCookingIsDone()
        {
            cookController.StartCooking(50, 2);
            cookController.OnTimerExpired(this, EventArgs.Empty);
            userInterface.Received(1).CookingIsDone();
        }
    }
}