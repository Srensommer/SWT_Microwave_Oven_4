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
    class IT14_Door_W_All
    {

        private IOutput output;
        private IPowerTube powerTube;
        private IDisplay display;
        private ILight light;
        private ITimer timer;
        private ICookController cookController;
        private IUserInterface userInterface;
        private IButton powerButton, startCancelButton, timeButton;
        private IDoor sut;


        [SetUp]
        public void Setup()
        {
            output = Substitute.For<Output>();
            powerTube = new PowerTube(output);
            display = new Display(output);
            light = new Light(output);
            timer = new Timer();
            cookController = new CookController(timer, display, powerTube);
            sut = new Door();
            powerButton = new Button();
            startCancelButton = new Button();
            timeButton = new Button();
            userInterface = new UserInterface(powerButton, timeButton, startCancelButton, sut, display, light, cookController);
        }

        [Test]
        public void CloseDoor()
        {
            sut.Close();
            output.Received().OutputLine("Light is turned off");
        }

        [Test]
        public void OpenDoor()
        {
            sut.Open();
            output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void OpenDoorWhenCook()
        {
            cookController.StartCooking(50, 50);
            sut.Open();
            output.Received().OutputLine("Light is turned off");
        }

    }
}
