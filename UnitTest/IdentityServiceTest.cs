using AutoMapper;
using BO.AutoMapperProFile;
using BO.StaticModels;
using BO.ViewModels;
using DAL;
using FS_DB_GatewayAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceLB.IdentityService;
using ServiceLB.LogService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class IdentityServiceTest
    {
        private IdentityController _identityController;

        [TestMethod]
        public async Task TestLogin()
        {
            //Arrange
            var MockIdentityService = new Mock<IIdentityService>();

            var mockResultData = new AuthResultViewModel
            {
                Access_Token = string.Empty,
                Expires_In = 0
            };

            var mockReq = new AuthRequestModel
            {
                AppName = "Test",
                Password = "1234",
                IP = "1234.51234"
            };

            MockIdentityService.Setup(repo => repo.Identity(mockReq)).ReturnsAsync(mockResultData);

            _identityController = new IdentityController(MockIdentityService.Object);
            //Act
            var result = await _identityController.Login(mockReq).ConfigureAwait(false);

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var actual = okObjectResult.Value as AuthResultViewModel;
            Assert.IsNotNull(actual);

            Assert.AreEqual(mockResultData, actual);
        }

        [TestMethod]
        public void VerifyModelsValidate()
        {
            AuthRequestModel req = new AuthRequestModel();
            Assert.IsTrue(ValidateModel(req).Any(
              v => v.MemberNames.Contains("AppName") &&
                   v.ErrorMessage.Contains("required")

                   ));
            Assert.IsTrue(ValidateModel(req).Any(
             v =>
                  v.MemberNames.Contains("Password") &&
                  v.ErrorMessage.Contains("required")

                  ));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}