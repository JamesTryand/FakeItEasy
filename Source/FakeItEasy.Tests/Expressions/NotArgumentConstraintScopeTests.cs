namespace FakeItEasy.Tests.Expressions
{
    using FakeItEasy.Expressions;
    using NUnit.Framework;

    [TestFixture]
    public class NotArgumentConstraintScopeTests
    {
        private ArgumentConstraintScope<string> parentScope;

        [SetUp]
        public void SetUp()
        {
            this.parentScope = A.Fake<ArgumentConstraintScope<string>>();
            A.CallTo(() => this.parentScope.IsValid(A<string>.Ignored)).Returns(true);
        }

        private NotArgumentConstraintScope<string> CreateValidations()
        {
            return new NotArgumentConstraintScope<string>(this.parentScope);
        }

        [Test]
        public void IsValid_should_return_false_when_parent_validations_is_not_valid()
        {
            // Arrange
            A.CallTo(() => this.parentScope.IsValid("foo")).Returns(false);
            var validations = this.CreateValidations();

            // Act
            
            // Assert
            Assert.That(validations.IsValid("foo"), Is.False);
        }

        [Test]
        public void IsValid_should_return_true_when_parent_validations_is_valid()
        {
            // Arrange
            A.CallTo(() => this.parentScope.IsValid("foo")).Returns(true);
            var validations = this.CreateValidations();

            // Act

            // Assert
            Assert.That(validations.IsValid("foo"), Is.True);
        }

        [TestCase("", Result = "not")]
        [TestCase("Foo AND", Result = "Foo AND not")]
        public string ToString_should_return_parent_validations_ToString_concatenated_with_NOT(string parentToString)
        {
            // Arrange
            A.CallTo(() => this.parentScope.Description).Returns(parentToString);
            var validations = this.CreateValidations();

            // Act

            // Assert
            return validations.ToString();
        }

        [TestCase(true, Result = false)]
        [TestCase(false, Result = true)]
        public bool ResultOfChildValidatorIsValid_should_reverse_value(bool validatorIsValid)
        {
            // Arrange
            var validations = this.CreateValidations();

            // Act

            // Assert
            return validations.ResultOfChildConstraintIsValid(validatorIsValid);
        }
    }
}
