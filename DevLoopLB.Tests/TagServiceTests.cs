using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services;
using Moq;
namespace DevLoopLB.Tests;

using DevLoopLB.Models;
using Xunit;

public class TagServiceTests
{
    private readonly Mock<ITagRepository> _mockRepository;
    private readonly TagService _tagService;

    public TagServiceTests()
    {
        _mockRepository = new Mock<ITagRepository>();
        _tagService = new TagService(_mockRepository.Object);
    }

    [Fact] 
    public async Task GetAllTagsAsync_ShouldReturnAllTags()
    {

        //arrange
        var expectedTags = new List<Tag>
        {
            new Tag{TagId = 1, Name = "C#"},

            new Tag{TagId = 2, Name = "Java"},
        };
        _mockRepository.Setup(r => r.GetAllTagsAsync())
                        .ReturnsAsync(expectedTags);

        //act
        var result = await _tagService.GetAllTagsAsync();

        //asert
        Assert.Equal(expectedTags, result);
        _mockRepository.Verify(r => r.GetAllTagsAsync(), Times.Once);
    }

    [Fact]
    public async Task AddTagAsync_ShouldCallRepositoryMethods()
    {
        var tag = new Tag { TagId = 1, Name = "React" };

        await _tagService.AddTagAsync(tag);

        _mockRepository.Verify(r => r.AddTagAsync(tag), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
