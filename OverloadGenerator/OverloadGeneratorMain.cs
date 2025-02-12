using System;
using System.Linq;

namespace OverloadGenerator;

internal static class OverloadGeneratorMain {

    private const int MAX_TYPE_PARAMS = 16;

    private static void Main() {
        // Console.WriteLine(generateBuilderMethods(false));
        // Console.WriteLine(generateBuilderMethods(true));
        // Console.WriteLine(generateInterfaces());
        Console.WriteLine(generateImplementationMethods());
        // Console.WriteLine(generateTests());
    }

    private static string generateInterfaces() {
        string actions = string.Join("\n\n", Enumerable.Range(1, MAX_TYPE_PARAMS)
            .Select(paramCount => $"public interface RateLimitedAction<{joinNumbers(paramCount, "in T")}>: IDisposable {{\n" +
                // $"\tAction<{joinNumbers(i)}> RateLimitedAction {{ get; }}\n" +
                $"\tvoid Invoke({joinNumbers(paramCount, i => $"T{i} arg{i}")});\n" +
                "}"));

        string funcs = string.Join("\n\n", Enumerable.Range(1, MAX_TYPE_PARAMS)
            .Select(paramCount => $"public interface RateLimitedFunc<{joinNumbers(paramCount, "in T")}, out TResult>: IDisposable {{\n" +
                // $"\tFunc<{joinNumbers(i)}, TResult> RateLimitedFunc {{ get; }}\n" +
                $"\tTResult? Invoke({joinNumbers(paramCount, i => $"T{i} arg{i}")});\n" +
                "}"));

        return actions + funcs;
    }

    private static string generateBuilderMethods(bool throttle) {
        string methodName = throttle ? "Throttle" : "Debounce";
        bool   leading    = throttle;
        string maxWait    = throttle ? ", wait" : "";
        string actions = string.Join("\n\n", Enumerable.Range(1, MAX_TYPE_PARAMS)
            .Select(i =>
                $"public static RateLimitedAction<{joinNumbers(i)}> {methodName}<{joinNumbers(i)}>(Action<{joinNumbers(i)}> action, TimeSpan wait, bool leading = {leading.ToString().ToLowerInvariant()}, bool trailing = true) {{\n" +
                $"\treturn new RateLimiter<{joinNumbers(i)}{string.Join(null, Enumerable.Repeat(", object", MAX_TYPE_PARAMS - i))}, object>(action, wait, leading, trailing{maxWait});\n" +
                "}"));

        string funcs = string.Join("\n\n", Enumerable.Range(1, MAX_TYPE_PARAMS)
            .Select(i =>
                $"public static RateLimitedFunc<{joinNumbers(i)}, TResult> {methodName}<{joinNumbers(i)}, TResult>(Func<{joinNumbers(i)}, TResult> func, TimeSpan wait, bool leading = {leading.ToString().ToLowerInvariant()}, bool trailing = true) {{\n" +
                $"\treturn new RateLimiter<{joinNumbers(i)}{string.Join(null, Enumerable.Repeat(", object", MAX_TYPE_PARAMS - i))}, TResult>(func, wait, leading, trailing{maxWait});\n" +
                "}"));

        return actions + "\n\n" + funcs;
    }

    private static string generateImplementationMethods() {
        string actions = string.Join("\n\n", Enumerable.Range(1, MAX_TYPE_PARAMS)
            .Select(paramCount =>
                $"void RateLimitedAction<{joinNumbers(paramCount)}>.Invoke({joinNumbers(paramCount, i => $"T{i} arg{i}")}) {{\n    object[] parameters = parameterArrayPool.Borrow();\n    {joinNumbers(paramCount, i => $"parameters[{i - 1}] = arg{i}!;", "\n    ")} \n    OnUserInvocation(parameters);\n}}"));

        string funcs = string.Join("\n\n", Enumerable.Range(1, MAX_TYPE_PARAMS)
            .Select(paramCount =>
                $"TResult? RateLimitedFunc<{joinNumbers(paramCount)}, TResult>.Invoke({joinNumbers(paramCount, i => $"T{i} arg{i}")}) {{\n    object[] parameters = parameterArrayPool.Borrow();\n    {joinNumbers(paramCount, i => $"parameters[{i - 1}] = arg{i}!;", "\n    ")} \n    return OnUserInvocation(parameters);\n}}"));

        return actions + "\n\n" + funcs;
    }

    private static string generateTests() {
        static string generateTestMethods(bool throttle, bool actions) {
            string testMethodNamePrefix = (throttle ? "Throttle" : "Debounce") + (actions ? "Action" : "Func");
            string rateLimitedType      = actions ? "Action" : "Func";
            string classAndMethodName   = throttle ? "Throttler.Throttle" : "Debouncer.Debounce";
            string propertyName         = actions ? "RateLimitedAction" : "RateLimitedFunc";
            string returnStatement      = actions ? "" : "return \"\";";
            string returnType           = actions ? "" : ", string";
            string expectation          = throttle ? "1" : "0";
            return string.Join("\n\n", Enumerable.Range(1, MAX_TYPE_PARAMS)
                .Select(i => $"public class {testMethodNamePrefix}{i}InputsClass: OverloadTests {{\n" +
                    $"\t[Fact]\n\tpublic void {testMethodNamePrefix}{i}Inputs() {{\n" +
                    $"\t\t{rateLimitedType}<{joinRepeat("int", i)}{returnType}> rateLimited = {classAndMethodName}<{joinRepeat("int", i)}{returnType}>(({joinNumbers(i, "arg")}) => {{ ++executionCount; {returnStatement} }}, WAIT_TIME).{propertyName};\n" +
                    joinRepeat($"\t\trateLimited({joinRepeat("8", i)});", 2, "\n") +
                    $"\n\t\texecutionCount.Should().Be({expectation});\n\t}}\n}}"));
        }

        return string.Join("\n",
            generateTestMethods(true, true),
            generateTestMethods(true, false),
            generateTestMethods(false, true),
            generateTestMethods(false, false));
    }

    private static string joinNumbers(int max, string prefix = "T", string joiner = ", ", string suffix = "", int min = 1) {
        return joinNumbers(max, i => $"{prefix}{i}{suffix}", joiner, min);
    }

    private static string joinNumbers(int max, Func<int, string> template, string joiner = ", ", int min = 1) {
        return string.Join(joiner, Enumerable.Range(min, max - min + 1).Select(template));
    }

    private static string joinRepeat(string element, int count, string joiner = ", ") {
        return string.Join(joiner, Enumerable.Repeat(element, count));
    }

}