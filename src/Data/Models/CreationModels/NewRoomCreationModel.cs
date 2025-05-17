namespace Iso.Data.Models.CreationModels;

/// <summary>
/// Room creation record.
/// </summary>
/// <param name="Name"></param>
/// <param name="Description"></param>
/// <param name="TagOne"></param>
/// <param name="TagTwo"></param>
/// <param name="TemplateId"></param>
public record NewRoomCreationModel(
    string Name,
    string Description,
    string TagOne,
    string TagTwo,
    string TemplateId);