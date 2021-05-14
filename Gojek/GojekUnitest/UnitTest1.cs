using Autofac.Extras.Moq;
using FluentAssertions;
using Gojek.Views.HomePage;
using NUnit.Framework;

namespace GojekUnitest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void TestNothing()
        {
            using var mock = AutoMock.GetLoose();
            var viewModel = mock.Create<GojekV2HomePageViewModel>();
            viewModel.UserName.Should().BeNullOrEmpty();
            viewModel.LoginCommand.Execute(null);
        }
    }
}