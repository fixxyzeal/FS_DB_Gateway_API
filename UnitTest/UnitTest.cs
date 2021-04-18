using BO.StaticModels;
using BO.ViewModels;
using FS_DB_GatewayAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceLB.LogService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class LogServiceUnitTest
    {
        private LogServiceController _logServiceController;

        [TestMethod]
        public async Task TestGetLog()
        {
            //Arrange
            var MockLogService = new Mock<ILogService>();

            IList<BO.Models.Mongo.LogService> logData = new List<BO.Models.Mongo.LogService>()
            {
                new BO.Models.Mongo.LogService
                {
                    Id = "dasdsd-dagg-sdfaf",
                    AppName = "test1",
                    Detail = "test1",
                    AppType ="PC",
                    LogType = Formatter.InformationLogType,
                    CreatedDate = Formatter.DB_CreatedDate,
                    IP = "123.4255.32424",
                    User = "1"
                },
                  new BO.Models.Mongo.LogService
                {
                    Id = "dasdsd-dagg-sdfaf2",
                    AppName = "test2",
                    Detail = "test2",
                    AppType ="Mobile",
                    LogType = Formatter.ErrorLogType,
                    CreatedDate = Formatter.DB_CreatedDate,
                    IP = "123.4255.32425",
                    User = "2"
                },
            };

            MockLogService.Setup(repo => repo.GetLog("", "")).ReturnsAsync(logData);

            _logServiceController = new LogServiceController(MockLogService.Object);
            //Act
            var result = await _logServiceController.GetLog(string.Empty, string.Empty).ConfigureAwait(false);

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var actual = okObjectResult.Value as IList<BO.Models.Mongo.LogService>;
            Assert.IsNotNull(actual);

            Assert.AreEqual(logData, actual);
        }

        [TestMethod]
        public async Task TestAddInformationLog()
        {
            //Arrange
            var MockLogService = new Mock<ILogService>();

            BO.ViewModels.LogViewModel logData = new BO.ViewModels.LogViewModel
            {
                AppName = "Test",
                AppType = "PC",
                Detail = "test",
                IP = "123.5252345",
                User = "test1"
            };

            MockLogService.Setup(repo => repo.AddInformationLog(logData));

            _logServiceController = new LogServiceController(MockLogService.Object);

            //Act
            var result = await _logServiceController.AddInformationLog(logData).ConfigureAwait(false);

            //Assert
            MockLogService.Verify(s => s.AddInformationLog(logData));
            var okObjectResult = result as OkObjectResult;
            Assert.IsNull(okObjectResult);
        }

        [TestMethod]
        public async Task TestAddErrorLog()
        {
            //Arrange
            var MockLogService = new Mock<ILogService>();

            BO.ViewModels.LogViewModel logData = new BO.ViewModels.LogViewModel
            {
                AppName = "",
                AppType = "PC",
                Detail = "test",
                IP = "123.5252345",
                User = "test1"
            };

            MockLogService.Setup(repo => repo.AddErrorLog(logData));

            _logServiceController = new LogServiceController(MockLogService.Object);

            //Act
            var result = await _logServiceController.AddErrorLog(logData).ConfigureAwait(false);

            //Assert
            MockLogService.Verify(s => s.AddErrorLog(logData));
            var okObjectResult = result as OkObjectResult;
            Assert.IsNull(okObjectResult);
        }

        [TestMethod]
        public async Task TestDeleteLog()
        {
            //Arrange
            var MockLogService = new Mock<ILogService>();

            string id = "aa-bb";

            MockLogService.Setup(repo => repo.DeleteLog(id));

            _logServiceController = new LogServiceController(MockLogService.Object);

            //Act
            var result = await _logServiceController.Delete(id).ConfigureAwait(false);

            //Assert
            MockLogService.Verify(s => s.DeleteLog(id));
            var okObjectResult = result as OkObjectResult;
            Assert.IsNull(okObjectResult);
        }

        [TestMethod]
        public void VerifyModelsValidate()
        {
            LogViewModel req = new LogViewModel();
            Assert.IsTrue(ValidateModel(req).Any(
              v => v.MemberNames.Contains("AppName") &&
                   v.ErrorMessage.Contains("required")

                   ));
            Assert.IsTrue(ValidateModel(req).Any(
             v =>
                  v.MemberNames.Contains("AppType") &&
                  v.ErrorMessage.Contains("required")

                  ));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}