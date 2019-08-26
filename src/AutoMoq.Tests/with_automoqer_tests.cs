using AutoMoq.Helpers;
using Moq;
using Shouldly;
using Xunit;

namespace AutoMoq.Tests
{
    public class with_automoqer_Tests
    {
        private AutoMoqer mocker;

        public with_automoqer_Tests()
        {
            mocker = new AutoMoqer();
        }

        [Fact]
        public void Instantiating_the_automoqer_sets_the_static_mocker()
        {
            with_automoqer.mocker = null;
            var test = new with_automoqer();
            with_automoqer.mocker.ShouldNotBeNull();
        }

        [Fact]
        public void A_config_option_can_be_provided_when_setting_up()
        {
            with_automoqer.mocker = null;
            new with_automoqer(new Config {MockBehavior = MockBehavior.Loose});

            with_automoqer.Create<Test>().DoSomething(); // should not fail

            new with_automoqer(new Config {MockBehavior = MockBehavior.Strict});
            var errorHit = false;
            try
            {
                with_automoqer.Create<Test>().DoSomething(); // should fail
            }
            catch
            {
                errorHit = true;
            }
            errorHit.ShouldBeTrue();
        }

        [Fact]
        public void GetMock_returns_the_mock()
        {
            var test = new with_automoqer();
            with_automoqer.mocker = new AutoMoqer();

            with_automoqer.GetMock<IDependency>().ShouldBeSameAs(with_automoqer.mocker.GetMock<IDependency>());
        }

        [Fact]
        public void Create_returns_the_class_resolved_from_automoqer()
        {
            var test = new with_automoqer();
            with_automoqer.mocker = new AutoMoqer();

            with_automoqer.Create<Test>()
                .Dependency.ShouldBeSameAs(with_automoqer.GetMock<IDependency>().Object);
        }

        [Fact]
        public void SetInstance_sets_the_instance()
        {
            with_automoqer.mocker = new AutoMoqer();

            var instance = new Mock<IDependency>().Object;
            with_automoqer.SetInstance(instance);

            with_automoqer.Create<Test>().Dependency.ShouldBeSameAs(instance);
        }

        public interface IDependency
        {
            void DoSomething();
        }

        public class Test
        {
            public readonly IDependency Dependency;

            public Test(IDependency Dependency)
            {
                this.Dependency = Dependency;
            }

            public void DoSomething()
            {
                Dependency.DoSomething();
            }
        }
    }
}