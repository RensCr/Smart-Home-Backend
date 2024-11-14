using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartHome_Backend.Controllers;
using SmartHome_Backend.Data;
using SmartHome_Backend.Model;
using Microsoft.EntityFrameworkCore.InMemory;

namespace Smart_Home_UnitTest.Controllers
{
    [TestClass]
    public class HuisControllerTests
    {
        private DataContext _context;
        private HuisController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DataContext(options);
            _controller = new HuisController(_context);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim("id", "1") // Mock user with ID 1
                    }))
                }
            };

            // Clear the in-memory database
            _context.Huizen.RemoveRange(_context.Huizen);
            _context.SaveChanges();

            // Seed the in-memory database
            _context.Huizen.AddRange(new List<Huis>
            {
                new Huis { Id = 1, GebruikersId = 1 ,Locatie = "Eindhoven",Beschrijving = "Test", ApparatenJson = "[{apparaat:Wasmachine}]",KamersJson="[{Kamer:Keuken}]"},
                new Huis { Id = 2, GebruikersId = 1 ,Locatie = "Tilburg",Beschrijving = "Test2",ApparatenJson = "[{apparaat:Droger},{apparaat:Lamp}]",KamersJson="[{Kamer:Woonkamer}]"},
                new Huis { Id = 3, GebruikersId = 2 ,Locatie = "Eindhoven",Beschrijving = "Test3",ApparatenJson="[{apparaat:Lamp},{Apparaat:Rolluik}]",KamersJson="[{Kamer:Woonkamer},{Kamer:slaapkamer Pietje}]"},
            });
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task AlleHuizen_ValidUser_ReturnTheAmountOfHouses()
        {
            // Act
            var result = await _controller.AlleHuizen();

            // Assert
            var okResult = result.Result as OkObjectResult;
            var returnedHuizen = okResult.Value as List<Huis>;
            Assert.AreEqual(2, returnedHuizen.Count);
        }

        [TestMethod]
        public async Task AlleHuizen_ValidUser_ReturnCorrectHouseInformations()
        {
            // Act
            var result = await _controller.AlleHuizen();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult");

            var returnedHuizen = okResult.Value as List<Huis>;
            Assert.IsNotNull(returnedHuizen, "Verwachte een lijst met huizen");
            Assert.AreEqual(2, returnedHuizen.Count, "Verwachte dat gebruiker 1, 2 huizen heeft");

            // Verify the details of the returned houses
            var huis1 = returnedHuizen.FirstOrDefault(h => h.Id == 1);
            var huis2 = returnedHuizen.FirstOrDefault(h => h.Id == 2);

            Assert.IsNotNull(huis1, "HuisId moet 1 zijn");
            Assert.AreEqual("Eindhoven", huis1.Locatie, "Locatie moet 'eindhoven' zijn");
            Assert.AreEqual("Test", huis1.Beschrijving, "Beschrijvring moet 'test' zijn ");
            Assert.AreEqual("[{apparaat:Wasmachine}]", huis1.ApparatenJson, "ApparatenJson moet '[{apparaat:Wasmachine}]' zijn");
            Assert.AreEqual("[{Kamer:Keuken}]", huis1.KamersJson, "KamersJson moet '[{Kamer:Keuken}]' zijn");

            Assert.IsNotNull(huis2, "HuisId moet 2 zijn");
            Assert.AreEqual("Tilburg", huis2.Locatie, "Locatie moet 'Tilburg' zijn");
            Assert.AreEqual("Test2", huis2.Beschrijving, "Beschrijving moet 'Test2' zijn ");
            Assert.AreEqual("[{apparaat:Droger},{apparaat:Lamp}]", huis2.ApparatenJson, "ApparatenJson moet '[{apparaat:Droger},{apparaat:Lamp}]' zijn");
            Assert.AreEqual("[{Kamer:Woonkamer}]", huis2.KamersJson, "KamersJson moet '[{Kamer:Woonkamer}]' zijn");

        }
        [TestMethod]
        public async Task VoegHuisToe_validUser_ShouldAddHouseToCorrectUser()
        {
            // Arrange
            var huis = new HuisToevoegen
            {
                Locatie = "Amsterdam",
                Beschrijving = "Test",
            };

            // Act
            var result = await _controller.VoegHuisToe(huis);

            // Assert
            var addedHuis = _context.Huizen.FirstOrDefault(h => h.Locatie == "Amsterdam" && h.Beschrijving == "Test");
            Assert.IsNotNull(addedHuis, "Huis moest toegevoegd zijn aan de database");
            Assert.AreEqual(1, addedHuis.GebruikersId, "Huis moest toegevoegd zijn op gebruikersId 1");
        }
        [TestMethod]
        public async Task VoegPlattegrondToe_ValidUser_ShouldAddPlattegrondToCorrectHouse()
        {
            // Arrange
            var plattegrond = new PlattegrondToevoegen
            {
                HuisID = 1,
                KamersJson = "[{\"Kamer\":\"Keuken\"},{\"Kamer\":\"Woonkamer\"}]",
                ApparatenJson = "[{\"Apparaat\":\"Wasmachine\"},{\"Apparaat\":\"Droger\"}]"
            };

            // Act
            var result = await _controller.VoegPlattegrondToe(plattegrond);

            // Assert
            var updatedHouse = _context.Huizen.FirstOrDefault(h => h.Id == 1);
            Assert.IsNotNull(updatedHouse, "Huis moest gevonden worden");
            Assert.AreEqual(1, updatedHouse.GebruikersId, "Huis moest toegevoegd zijn op gebruikersId 1");
            Assert.AreEqual("[{\"Kamer\":\"Keuken\"},{\"Kamer\":\"Woonkamer\"}]", updatedHouse.KamersJson, "KamersJson moest geupdate zijn");
            Assert.AreEqual("[{\"Apparaat\":\"Wasmachine\"},{\"Apparaat\":\"Droger\"}]", updatedHouse.ApparatenJson, "ApparatenJson moest geupdate zijn");
        }
        [TestMethod]
        public async Task VerkrijgPlattegrond_ValidUser_ShouldReturnCorrectInformation()
        {
            // Act
            var result = await _controller.VerkrijgPlattegrond(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult");

            var returnedPlattegrond = okResult.Value as dynamic;
            Assert.IsNotNull(returnedPlattegrond, "Expected a Plattegrond object");

            Assert.AreEqual("[{Kamer:Keuken}]", returnedPlattegrond.KamersJson, "KamersJson moest geupdate zijn");
            Assert.AreEqual("[{apparaat:Wasmachine}]", returnedPlattegrond.ApparatenJson, "ApparatenJson moest geupdate zijn");
        }


    }
}
