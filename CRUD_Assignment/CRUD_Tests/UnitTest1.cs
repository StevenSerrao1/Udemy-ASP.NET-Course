namespace CRUD_Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            // Arrange // Declaring variables and collecting inputs
            MyMath myMath = new MyMath();
            int input1 = 3, input2 = 4;
            int expected = 7;

            // Act // Call the testable method
            int actual = myMath.Add(input1, input2);

            // Assert // Compare the expected value with actual value
            Assert.Equal(expected, actual);

        }
    }
}