using Google.GenAI;
using System.Diagnostics;
using System.Text.RegularExpressions;

// ============================================================
// 0. Gemini API Key
// ============================================================

var apiKey =
    System.Environment.GetEnvironmentVariable("GEMINI_API_KEY");

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("GEMINI_API_KEY is not set.");
    return;
}

// ============================================================
// 1. AI UNIT TEST AGENT
// ============================================================

Console.WriteLine("================================================");
Console.WriteLine("AI UNIT TEST AGENT");
Console.WriteLine("================================================");
Console.WriteLine();

// ============================================================
// 2. Locate IMS.Application
// ============================================================

string applicationDirectory =
    System.IO.Path.GetFullPath(@"..\IMS.Application");

if (!System.IO.Directory.Exists(applicationDirectory))
{
    Console.WriteLine("IMS.Application directory not found:");
    Console.WriteLine(applicationDirectory);

    return;
}

// ============================================================
// 3. Search ALL C# Files
// ============================================================

string[] allCSharpFiles =
    System.IO.Directory.GetFiles(
        applicationDirectory,
        "*.cs",
        System.IO.SearchOption.AllDirectories
    );

// ============================================================
// 4. Discover Handler Classes
// ============================================================

var handlers =
    new List<(string Name, string Path)>();

foreach (var file in allCSharpFiles)
{
    try
    {
        string content =
            await System.IO.File.ReadAllTextAsync(file);

        var matches =
            Regex.Matches(
                content,
                @"\bclass\s+(\w*Handler)\b"
            );

        foreach (Match match in matches)
        {
            string name =
                match.Groups[1].Value;

            handlers.Add(
                (name, file)
            );
        }
    }
    catch
    {
        // Ignore files that cannot be read.
    }
}

// ============================================================
// 5. Remove Duplicate Handlers
// ============================================================

handlers =
    handlers
        .GroupBy(x => x.Name)
        .Select(x => x.First())
        .OrderBy(x => x.Name)
        .ToList();

// ============================================================
// 6. Check Handler Count
// ============================================================

if (handlers.Count == 0)
{
    Console.WriteLine(
        "No handler classes were found."
    );

    return;
}

// ============================================================
// 7. Select Handler
// ============================================================

string? handlerName = null;

string? handlerPath = null;

// ============================================================
// 8. Handler Supplied Through Command Line
// ============================================================

if (args.Length > 0)
{
    handlerName = args[0];

    var selectedHandler =
        handlers.FirstOrDefault(
            x => x.Name.Equals(
                handlerName,
                StringComparison.OrdinalIgnoreCase
            )
        );

    if (string.IsNullOrWhiteSpace(
            selectedHandler.Name))
    {
        Console.WriteLine();

        Console.WriteLine(
            $"Handler '{handlerName}' was not found."
        );

        Console.WriteLine();

        Console.WriteLine(
            "Available handlers:"
        );

        for (int i = 0; i < handlers.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1}. {handlers[i].Name}"
            );
        }

        return;
    }

    handlerName =
        selectedHandler.Name;

    handlerPath =
        selectedHandler.Path;
}

// ============================================================
// 9. Display Handler Selection Menu
// ============================================================

else
{
    Console.WriteLine(
        "AVAILABLE HANDLERS"
    );

    Console.WriteLine(
        "================================================"
    );

    for (int i = 0; i < handlers.Count; i++)
    {
        Console.WriteLine(
            $"{i + 1}. {handlers[i].Name}"
        );
    }

    Console.WriteLine();

    Console.Write(
        "Select handler: "
    );

    string? selection =
        Console.ReadLine();

    if (!int.TryParse(
            selection,
            out int selectedNumber))
    {
        Console.WriteLine();

        Console.WriteLine(
            "Invalid selection."
        );

        return;
    }

    if (
        selectedNumber < 1 ||
        selectedNumber > handlers.Count)
    {
        Console.WriteLine();

        Console.WriteLine(
            "Invalid handler number."
        );

        return;
    }

    var selectedHandler =
        handlers[selectedNumber - 1];

    handlerName =
        selectedHandler.Name;

    handlerPath =
        selectedHandler.Path;
}

// ============================================================
// 10. Handler Selected
// ============================================================

Console.WriteLine();

Console.WriteLine(
    $"Selected handler: {handlerName}"
);

Console.WriteLine();

Console.WriteLine(
    "Handler found:"
);

Console.WriteLine(
    handlerPath
);

Console.WriteLine();

// ============================================================
// 11. Read Handler File
// ============================================================

string handlerCode =
    await System.IO.File.ReadAllTextAsync(
        handlerPath!
    );

Console.WriteLine(
    "Handler loaded successfully."
);

Console.WriteLine(
    $"Characters: {handlerCode.Length}"
);

Console.WriteLine();

// ============================================================
// 12. Locate Test Project
// ============================================================

string testProjectDirectory =
    System.IO.Path.GetFullPath(@"..\IMSUntTest");

if (!System.IO.Directory.Exists(
        testProjectDirectory))
{
    Console.WriteLine(
        "Test project directory not found:"
    );

    Console.WriteLine(
        testProjectDirectory
    );

    return;
}

// ============================================================
// 13. Find Test Project
// ============================================================

string[] projectFiles =
    System.IO.Directory.GetFiles(
        testProjectDirectory,
        "*.csproj",
        System.IO.SearchOption.TopDirectoryOnly
    );

if (projectFiles.Length == 0)
{
    Console.WriteLine(
        "No .csproj file found inside:"
    );

    Console.WriteLine(
        testProjectDirectory
    );

    return;
}

string testProjectPath =
    projectFiles[0];

Console.WriteLine(
    "Test project found:"
);

Console.WriteLine(
    testProjectPath
);

Console.WriteLine();

// ============================================================
// 14. Determine Test File Name
// ============================================================

string testClassName =
    $"{handlerName}Tests";

string testFileName =
    $"{testClassName}.cs";

// ============================================================
// 15. Determine Test Directory
// ============================================================

string featuresDirectory =
    System.IO.Path.Combine(
        testProjectDirectory,
        "Features"
    );

string handlerDirectory =
    System.IO.Path.GetDirectoryName(
        handlerPath!
    )!;

string featuresRoot =
    System.IO.Path.Combine(
        applicationDirectory,
        "Features"
    );

string relativeFeaturePath =
    System.IO.Path.GetRelativePath(
        featuresRoot,
        handlerDirectory
    );

string[] pathParts =
    relativeFeaturePath.Split(
        new[]
        {
            System.IO.Path.DirectorySeparatorChar,
            System.IO.Path.AltDirectorySeparatorChar
        },
        StringSplitOptions.RemoveEmptyEntries
    );

var testPathParts =
    new List<string>();

foreach (var part in pathParts)
{
    if (!part.Equals(
            "Command",
            StringComparison.OrdinalIgnoreCase) &&
        !part.Equals(
            "Commands",
            StringComparison.OrdinalIgnoreCase) &&
        !part.Equals(
            "Query",
            StringComparison.OrdinalIgnoreCase) &&
        !part.Equals(
            "Queries",
            StringComparison.OrdinalIgnoreCase))
    {
        testPathParts.Add(part);
    }
}

string testDirectory =
    featuresDirectory;

foreach (var part in testPathParts)
{
    testDirectory =
        System.IO.Path.Combine(
            testDirectory,
            part
        );
}

// ============================================================
// 16. Create Test Directory
// ============================================================

System.IO.Directory.CreateDirectory(
    testDirectory
);

string testFilePath =
    System.IO.Path.Combine(
        testDirectory,
        testFileName
    );

string fullTestFilePath =
    System.IO.Path.GetFullPath(
        testFilePath
    );

Console.WriteLine(
    "Test file:"
);

Console.WriteLine(
    fullTestFilePath
);

Console.WriteLine();

// ============================================================
// 17. Create Gemini Client
// ============================================================

using var client =
    new Client(
        apiKey: apiKey
    );

// ============================================================
// 18. Generate Initial Unit Test
// ============================================================

Console.WriteLine(
    "Generating unit test with Gemini..."
);

Console.WriteLine();

var initialPrompt = $"""
You are an expert C# unit testing developer.

Analyze the following C# source file.

It contains the handler class:

{handlerName}

Generate a complete unit test class for that handler.

Requirements:

- Use xUnit.
- Use Moq.
- Generate a complete compilable test class.
- Test successful execution.
- Test important validation or business logic.
- Test dependency failures where appropriate.
- Mock all dependencies.
- Use the ACTUAL return types of methods.
- Do not invent methods.
- Do not invent return types.
- Pay special attention to async methods.
- If a method returns Task<T>, use the correct ReturnsAsync type.
- If a method returns Task, use the correct setup.
- Analyze interfaces used by the handler.
- Handle nullable reference types correctly.
- Handle IFormFile or other framework types correctly when needed.
- Use the actual namespace from the source code.
- Include all required using statements.
- Do not modify production code.
- The test must compile in the existing IMSUntTest project.
- Return ONLY C# code.
- Do not use markdown code fences.

================================================
SOURCE CODE
================================================

{handlerCode}

================================================
HANDLER CLASS
================================================

{handlerName}
""";

string generatedTest;

try
{
    var response =
        await client.Models.GenerateContentAsync(
            model: "gemini-3.5-flash-lite",
            contents: initialPrompt
        );

    generatedTest =
        response.Text ?? "";

    if (string.IsNullOrWhiteSpace(
            generatedTest))
    {
        Console.WriteLine(
            "Gemini returned an empty response."
        );

        return;
    }
}
catch (TaskCanceledException)
{
    Console.WriteLine(
        "Gemini request timed out."
    );

    return;
}
catch (Exception ex)
{
    Console.WriteLine(
        "Gemini request failed:"
    );

    Console.WriteLine(
        ex.Message
    );

    return;
}

// ============================================================
// 19. Clean Gemini Response
// ============================================================

generatedTest =
    CleanCode(generatedTest);

// ============================================================
// 20. Self-Correction + Coverage Improvement Loop
// ============================================================

const int maxAttempts = 3;

string lastTestError = "";

for (
    int attempt = 1;
    attempt <= maxAttempts;
    attempt++)
{
    Console.WriteLine();

    Console.WriteLine(
        "================================================"
    );

    Console.WriteLine(
        $"ATTEMPT {attempt} OF {maxAttempts}"
    );

    Console.WriteLine(
        "================================================"
    );

    // ========================================================
    // Save Test
    // ========================================================

    await System.IO.File.WriteAllTextAsync(
        testFilePath,
        generatedTest
    );

    Console.WriteLine();

    Console.WriteLine(
        "Test saved."
    );

    Console.WriteLine(
        fullTestFilePath
    );

    // ========================================================
    // Run Tests
    // ========================================================

    Console.WriteLine();

    Console.WriteLine(
        "Running unit tests..."
    );

    Console.WriteLine(
        "--------------------------------"
    );

    var testResult =
        await RunTests(
            testProjectPath
        );

    Console.WriteLine(
        testResult.Output
    );

    if (!string.IsNullOrWhiteSpace(
            testResult.Error))
    {
        Console.WriteLine();

        Console.WriteLine(
            "TEST ERROR"
        );

        Console.WriteLine(
            "--------------------------------"
        );

        Console.WriteLine(
            testResult.Error
        );
    }

    Console.WriteLine();

    Console.WriteLine(
        $"Exit Code: {testResult.ExitCode}"
    );

    // ========================================================
    // Read Handler Coverage
    // ========================================================

    var coverage =
        ReadCoverage(
            testProjectDirectory,
            handlerName!
        );

    Console.WriteLine();

    Console.WriteLine(
        "CODE COVERAGE"
    );

    Console.WriteLine(
        "--------------------------------"
    );

    Console.WriteLine(
        $"Line Coverage: " +
        $"{coverage.LineCoverage ?? "Not available"}"
    );

    Console.WriteLine(
        $"Branch Coverage: " +
        $"{coverage.BranchCoverage ?? "Not available"}"
    );

    Console.WriteLine();

    Console.WriteLine(
        "Uncovered Lines:"
    );

    if (coverage.UncoveredLines.Count == 0)
    {
        Console.WriteLine(
            "None"
        );
    }
    else
    {
        Console.WriteLine(
            string.Join(
                ", ",
                coverage.UncoveredLines
            )
        );
    }

    if (coverage.CoverageFile != null)
    {
        Console.WriteLine();

        Console.WriteLine(
            $"Coverage file: {coverage.CoverageFile}"
        );
    }

    // ========================================================
    // CHECK WHETHER TESTS PASSED
    // ========================================================

    bool testsPassed =
        testResult.ExitCode == 0;

    // ========================================================
    // CHECK WHETHER FULL COVERAGE WAS ACHIEVED
    // ========================================================

    bool fullCoverage =
        coverage.LineCoverage == "100.00%" &&
        coverage.BranchCoverage == "100.00%";

    // ========================================================
    // SUCCESS
    // ========================================================

    if (testsPassed && fullCoverage)
    {
        Console.WriteLine();

        Console.WriteLine(
            "================================"
        );

        Console.WriteLine(
            "ALL TESTS PASSED."
        );

        Console.WriteLine(
            "FULL COVERAGE ACHIEVED."
        );

        Console.WriteLine(
            "================================"
        );

        return;
    }

    // ========================================================
    // TESTS PASSED BUT COVERAGE IS INCOMPLETE
    // ========================================================

    if (testsPassed && !fullCoverage)
    {
        Console.WriteLine();

        Console.WriteLine(
            "TESTS PASSED."
        );

        Console.WriteLine(
            "Coverage is incomplete."
        );

        Console.WriteLine();

        Console.WriteLine(
            "Requesting additional tests from Gemini..."
        );

        // ====================================================
        // Coverage Improvement Prompt
        // ====================================================

        var coveragePrompt = $"""
You are an expert C# unit testing developer.

The existing unit tests for the following handler PASS,
but code coverage is incomplete.

Your job is to improve the existing test class by adding
tests that cover the uncovered lines and branches.

================================================
HANDLER
================================================

{handlerName}

================================================
SOURCE CODE
================================================

{handlerCode}

================================================
CURRENT TEST CLASS
================================================

{generatedTest}

================================================
CURRENT COVERAGE
================================================

Line Coverage:
{coverage.LineCoverage}

Branch Coverage:
{coverage.BranchCoverage}

================================================
UNCOVERED LINES
================================================

{string.Join(", ", coverage.UncoveredLines)}

================================================
IMPORTANT RULES
================================================

- Do NOT modify production code.
- Do NOT remove useful existing tests.
- Keep all currently passing tests.
- Add additional tests.
- Focus specifically on uncovered lines.
- Focus specifically on uncovered branches.
- Analyze the handler source code carefully.
- Cover validation paths where appropriate.
- Cover exception paths where appropriate.
- Cover null/empty conditions where appropriate.
- Cover dependency behavior where appropriate.
- Cover conditional branches.
- Use xUnit.
- Use Moq.
- Use the actual interfaces.
- Use the actual method signatures.
- Do not invent methods.
- Do not invent return types.
- Pay attention to Task versus Task<T>.
- Use ReturnsAsync correctly.
- Use ThrowsAsync correctly.
- Keep nullable reference types correct.
- Keep the existing namespace.
- Keep useful existing tests.
- Generate a COMPLETE replacement test class.
- The final test class must compile.
- Return ONLY C# code.
- Do not use markdown code fences.

================================================

Generate the improved complete test class now.
""";

        try
        {
            var coverageResponse =
                await client.Models.GenerateContentAsync(
                    model: "gemini-3.5-flash-lite",
                    contents: coveragePrompt
                );

            string improvedTest =
                coverageResponse.Text ?? "";

            if (string.IsNullOrWhiteSpace(
                    improvedTest))
            {
                Console.WriteLine(
                    "Gemini returned an empty coverage test."
                );

                return;
            }

            generatedTest =
                CleanCode(
                    improvedTest
                );

            Console.WriteLine();

            Console.WriteLine(
                "Gemini generated additional coverage tests."
            );
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine(
                "Gemini coverage request timed out."
            );

            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "Gemini coverage request failed:"
            );

            Console.WriteLine(
                ex.Message
            );

            return;
        }

        // Continue to next attempt.
        continue;
    }

    // ========================================================
    // TESTS FAILED
    // ========================================================

    lastTestError =
        testResult.Output +
        System.Environment.NewLine +
        testResult.Error;

    if (attempt == maxAttempts)
    {
        Console.WriteLine();

        Console.WriteLine(
            "Maximum attempts reached."
        );

        Console.WriteLine(
            "The AI could not produce a passing test."
        );

        return;
    }

    // ========================================================
    // SEND FAILURE TO GEMINI
    // ========================================================

    Console.WriteLine();

    Console.WriteLine(
        "Sending test failure to Gemini..."
    );

    Console.WriteLine();

    var fixPrompt = $"""
You are an expert C# unit testing developer.

You previously generated a unit test for this handler:

{handlerName}

The test was compiled and executed using the actual
IMSUntTest project.

The test FAILED.

Your job is to FIX THE TEST.

================================================
IMPORTANT RULES
================================================

- Analyze the compiler/test error carefully.
- Use the actual source code.
- Use the actual generated test.
- Use the actual compiler/test output.
- Do not modify production code.
- Do not invent methods.
- Do not invent return types.
- Check every mocked method against its actual interface.
- Check Task versus Task<T>.
- Check ReturnsAsync types.
- Check ThrowsAsync types.
- Check constructor dependencies.
- Check namespaces.
- Check nullable reference types.
- Check xUnit assertions.
- Check IFormFile and other framework types if used.
- Keep useful existing tests.
- Add missing tests where appropriate.
- Generate a complete replacement test class.
- The final test must compile.
- Return ONLY C# code.
- Do not use markdown code fences.

================================================
SOURCE CODE
================================================

{handlerCode}

================================================
CURRENT TEST
================================================

{generatedTest}

================================================
COMPILER / TEST OUTPUT
================================================

{lastTestError}

================================================

Generate the corrected complete test class now.
""";

    try
    {
        var fixResponse =
            await client.Models.GenerateContentAsync(
                model: "gemini-3.5-flash-lite",
                contents: fixPrompt
            );

        generatedTest =
            fixResponse.Text ?? "";

        if (string.IsNullOrWhiteSpace(
                generatedTest))
        {
            Console.WriteLine(
                "Gemini returned an empty corrected test."
            );

            return;
        }

        generatedTest =
            CleanCode(
                generatedTest
            );

        Console.WriteLine(
            "Gemini generated a corrected test."
        );
    }
    catch (TaskCanceledException)
    {
        Console.WriteLine(
            "Gemini correction request timed out."
        );

        return;
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            "Gemini correction failed:"
        );

        Console.WriteLine(
            ex.Message
        );

        return;
    }
}

// ============================================================
// Helper: Run Tests
// ============================================================

async Task<(int ExitCode, string Output, string Error)>
    RunTests(string projectPath)
{
    var process =
        new Process();

    process.StartInfo =
        new ProcessStartInfo
        {
            FileName = "dotnet",

            Arguments =
                $"test \"{projectPath}\" --no-restore --collect:\"XPlat Code Coverage\"",

            WorkingDirectory =
                System.Environment.CurrentDirectory,

            RedirectStandardOutput = true,

            RedirectStandardError = true,

            UseShellExecute = false,

            CreateNoWindow = true
        };

    process.Start();

    string output =
        await process.StandardOutput.ReadToEndAsync();

    string error =
        await process.StandardError.ReadToEndAsync();

    await process.WaitForExitAsync();

    return (
        process.ExitCode,
        output,
        error
    );
}

// ============================================================
// Helper: Read Handler-Specific Coverage
// ============================================================

(
    string? LineCoverage,
    string? BranchCoverage,
    string? CoverageFile,
    List<int> UncoveredLines
)
ReadCoverage(
    string testProjectDirectory,
    string handlerName)
{
    string testResultsDirectory =
        System.IO.Path.Combine(
            testProjectDirectory,
            "TestResults"
        );

    if (!System.IO.Directory.Exists(
            testResultsDirectory))
    {
        return (
            null,
            null,
            null,
            new List<int>()
        );
    }

    string[] coverageFiles =
        System.IO.Directory.GetFiles(
            testResultsDirectory,
            "coverage.cobertura.xml",
            System.IO.SearchOption.AllDirectories
        );

    if (coverageFiles.Length == 0)
    {
        return (
            null,
            null,
            null,
            new List<int>()
        );
    }

    string coverageFile =
        coverageFiles
            .OrderByDescending(
                System.IO.File.GetLastWriteTime
            )
            .First();

    try
    {
        var document =
            System.Xml.Linq.XDocument.Load(
                coverageFile
            );

        // ====================================================
        // Find Selected Handler Class
        // ====================================================

        var handlerClass =
            document
                .Descendants("class")
                .FirstOrDefault(
                    x =>
                    {
                        string? className =
                            x.Attribute("name")?.Value;

                        if (string.IsNullOrWhiteSpace(
                                className))
                        {
                            return false;
                        }

                        return
                            className.EndsWith(
                                handlerName,
                                StringComparison.Ordinal
                            );
                    }
                );

        if (handlerClass == null)
        {
            return (
                null,
                null,
                coverageFile,
                new List<int>()
            );
        }

        // ====================================================
        // Read Line Coverage
        // ====================================================

        string? lineRate =
            handlerClass
                .Attribute("line-rate")
                ?.Value;

        // ====================================================
        // Read Branch Coverage
        // ====================================================

        string? branchRate =
            handlerClass
                .Attribute("branch-rate")
                ?.Value;

        string? lineCoverage = null;

        string? branchCoverage = null;

        // ====================================================
        // Convert Line Coverage
        // ====================================================

        if (double.TryParse(
                lineRate,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out double lineRateValue))
        {
            lineCoverage =
                $"{lineRateValue * 100:F2}%";
        }

        // ====================================================
        // Convert Branch Coverage
        // ====================================================

        if (double.TryParse(
                branchRate,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out double branchRateValue))
        {
            branchCoverage =
                $"{branchRateValue * 100:F2}%";
        }

        // ====================================================
        // Find Uncovered Lines
        // ====================================================

        var uncoveredLines =
            handlerClass
                .Descendants("line")
                .Where(
                    x =>
                    x.Attribute("hits")?.Value == "0"
                )
                .Select(
                    x =>
                    {
                        string? number =
                            x.Attribute("number")?.Value;

                        if (int.TryParse(
                                number,
                                out int lineNumber))
                        {
                            return lineNumber;
                        }

                        return -1;
                    }
                )
                .Where(
                    x => x >= 0
                )
                .Distinct()
                .OrderBy(
                    x => x
                )
                .ToList();

        return (
            lineCoverage,
            branchCoverage,
            coverageFile,
            uncoveredLines
        );
    }
    catch
    {
        return (
            null,
            null,
            coverageFile,
            new List<int>()
        );
    }
}

// ============================================================
// Helper: Clean Gemini Response
// ============================================================

string CleanCode(string code)
{
    code =
        code.Trim();

    // ========================================================
    // Remove Markdown Code Fence
    // ========================================================

    if (code.StartsWith("```"))
    {
        int firstNewLine =
            code.IndexOf('\n');

        if (firstNewLine >= 0)
        {
            code =
                code[
                    (firstNewLine + 1)..
                ];
        }

        int lastFence =
            code.LastIndexOf("```");

        if (lastFence >= 0)
        {
            code =
                code[..lastFence];
        }
    }

    return code.Trim();
}