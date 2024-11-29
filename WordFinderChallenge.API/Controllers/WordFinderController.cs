using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WordFinderChallenge.API.Configuration;
using WordFinderChallenge.Core.Services;
using WordFinderChallenge.Utilities;

namespace WordFinderChallenge.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WordFinderController : ControllerBase
{
    private readonly ApplicationOptions _applicationOptions;

    public WordFinderController(IOptions<ApplicationOptions> applicationOptions)
    {
        _applicationOptions = applicationOptions.Value;
    }

    [HttpPost]
    [Route(nameof(CharacterMatricesRepository.Matrix16x16))]
    public async Task<IActionResult> Matrix16x16([FromBody] IEnumerable<string> wordStream)
    {
        var wordFinder = new WordFinder(CharacterMatricesRepository.Matrix16x16, _applicationOptions.MatrixSize, _applicationOptions.TopMostRepeatedWordsCount);

        var topFoundWords = await wordFinder.FindAsync(wordStream);

        return Ok(topFoundWords ?? []);
    }

    [HttpPost]
    [Route(nameof(CharacterMatricesRepository.Matrix64x64))]
    public async Task<IActionResult> Matrix64x64([FromBody] IEnumerable<string> wordStream)
    {
        var wordFinder = new WordFinder(CharacterMatricesRepository.Matrix64x64, _applicationOptions.MatrixSize, _applicationOptions.TopMostRepeatedWordsCount);

        var topFoundWords = await wordFinder.FindAsync(wordStream);

        return Ok(topFoundWords ?? []);
    }

    [HttpPost]
    [Route(nameof(CharacterMatricesRepository.InvalidMatrixEmpty))]
    public async Task<IActionResult> InvalidMatrixEmpty([FromBody] IEnumerable<string> wordStream)
    {
        var wordFinder = new WordFinder(CharacterMatricesRepository.InvalidMatrixEmpty, _applicationOptions.MatrixSize, _applicationOptions.TopMostRepeatedWordsCount);

        var topFoundWords = await wordFinder.FindAsync(wordStream);

        return Ok(topFoundWords ?? []);
    }

    [HttpPost]
    [Route(nameof(CharacterMatricesRepository.InvalidMatrixDifferentLengths))]
    public async Task<IActionResult> InvalidMatrixDifferentLengths([FromBody] IEnumerable<string> wordStream)
    {
        var wordFinder = new WordFinder(CharacterMatricesRepository.InvalidMatrixDifferentLengths, _applicationOptions.MatrixSize, _applicationOptions.TopMostRepeatedWordsCount);

        var topFoundWords = await wordFinder.FindAsync(wordStream);

        return Ok(topFoundWords ?? []);
    }

    [HttpPost]
    [Route(nameof(CharacterMatricesRepository.InvalidMatrix65x65))]
    public async Task<IActionResult> InvalidMatrix65x65([FromBody] IEnumerable<string> wordStream)
    {
        var wordFinder = new WordFinder(CharacterMatricesRepository.InvalidMatrix65x65, _applicationOptions.MatrixSize, _applicationOptions.TopMostRepeatedWordsCount);

        var topFoundWords = await wordFinder.FindAsync(wordStream);

        return Ok(topFoundWords ?? []);
    }

    [HttpPost]
    [Route(nameof(CharacterMatricesRepository.InvalidMatrix2x65))]
    public async Task<IActionResult> InvalidMatrix2x65([FromBody] IEnumerable<string> wordStream)
    {
        var wordFinder = new WordFinder(CharacterMatricesRepository.InvalidMatrix2x65, _applicationOptions.MatrixSize, _applicationOptions.TopMostRepeatedWordsCount);

        var topFoundWords = await wordFinder.FindAsync(wordStream);

        return Ok(topFoundWords ?? []);
    }

    [HttpPost]
    [Route(nameof(CharacterMatricesRepository.InvalidMatrix65x2))]
    public async Task<IActionResult> InvalidMatrix65x2([FromBody] IEnumerable<string> wordStream)
    {
        var wordFinder = new WordFinder(CharacterMatricesRepository.InvalidMatrix65x2, _applicationOptions.MatrixSize, _applicationOptions.TopMostRepeatedWordsCount);

        var topFoundWords = await wordFinder.FindAsync(wordStream);

        return Ok(topFoundWords ?? []);
    }
}
