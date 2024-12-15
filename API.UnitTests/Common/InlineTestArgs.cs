namespace API.UnitTests.Common;

public class InlineTestArgs<TInput, TExpectedResult>
{
    public TInput? InputValue { get; set; }
    public required TExpectedResult ExpectedResult { get; set; }
}

public class InlineTestArgs<TInput1, TInput2, TExpectedResult>
{
    public TInput1? InputValue1 { get; set; }
    public TInput2? InputValue2 { get; set; }
    public required TExpectedResult ExpectedResult { get; set; }
}
