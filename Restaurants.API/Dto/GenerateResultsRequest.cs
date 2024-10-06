namespace Restaurants.API.Dto;

public record GenerateResultsRequest(int ResultsCount, int MinTemp, int MaxTemp);