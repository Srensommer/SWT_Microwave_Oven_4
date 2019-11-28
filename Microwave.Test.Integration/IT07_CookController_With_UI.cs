using System;
using System.Collections.Generic;
using System.IO;
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
        public class IT07_CookController_With_UI
        {
            private CookController _sut;
            private IUserInterface _userInterface;
            private IOutput _output;
            private IDisplay _display;
            private IButton _powerButton;
            private IButton _timeButton;
            private IButton _startCancelButton;
            private IDoor _door;
            private ILight _light;
            private IPowerTube _powerTube;
            private ITimer _timer;

            [SetUp]
            public void SetUp()
            { 
                _output = Substitute.For<IOutput>();
                _light = Substitute.For<ILight>();
                _powerButton = Substitute.For<IButton>();
                _timeButton = Substitute.For<IButton>();
                _startCancelButton = Substitute.For<IButton>(); 
                _door = Substitute.For<IDoor>();
                _timer = new MicrowaveOvenClasses.Boundary.Timer();
                _display = new Display(_output);
                _powerTube = new PowerTube(_output);
                _sut = new CookController(_timer, _display, _powerTube);
                _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _sut);
                _sut.UI = _userInterface;
            }
            [Test]
            public void OnTimeExpiredOutputsCorrect()
            {
                _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
                _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
                _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
                _sut.OnTimerExpired(this, EventArgs.Empty);
                _output.Received().OutputLine("Display cleared");
            }
        }
}
