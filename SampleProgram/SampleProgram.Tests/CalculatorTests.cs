using System;
using FluentAssertions;
using Xunit;

namespace SampleProgram.Tests;

public class CalculatorTests
{
    [Theory]
    [InlineData( 1, 1, 2 )]
    [InlineData( -6, -9, -15 )]
    [InlineData( -30, 20, -10 )]
    public void AddTest( int x, int y, int expected )
    {
        new Calculator().Add( x, y ).Should().Be( expected );
    }

    [Theory]
    [InlineData( 5, 5, 0 )]
    [InlineData( 20, 30, -10 )]
    [InlineData( 50, 10, 40 )]
    public void SubtractTest( int x, int y, int expected )
    {
        new Calculator().Subtract( x, y ).Should().Be( expected );
    }

    [Theory]
    [InlineData( 3, 8, 24 )]
    [InlineData( 0, 42, 0 )]
    [InlineData( 30, 0, 0 )]
    [InlineData( 1, 1, 1 )]
    [InlineData( -1, -1, 1 )]
    public void MultiplyTest( int x, int y, int expected )
    {
        new Calculator().Multiply( x, y ).Should().Be( expected );
    }

    [Theory]
    [InlineData( 9, 3, 3 )]
    [InlineData( 5, 2, 2 )]
    [InlineData( 0, 2, 0 )]
    public void DivideTest( int x, int y, int expected )
    {
        y.Should().NotBe( 0 );
        new Calculator().Divide( x, y ).Should().Be( expected );
    }

    [Fact]
    public void Divide_ShouldThrow_WhenSecondNumberIsZero()
    {
        Action illegalDivision = () => { new Calculator().Divide( 1, 0 ); };
        illegalDivision.Should().Throw<DivideByZeroException>();
    }
}